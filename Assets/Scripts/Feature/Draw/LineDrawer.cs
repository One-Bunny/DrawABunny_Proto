using System.Collections.Generic;
using UnityEngine;

namespace OneBunny
{
    public sealed class LineDrawer : MonoBehaviour
    {
        private LineController _line;
        private LineRenderer _lineRenderer;
        private EdgeCollider2D _edgeCollider;
        private PolygonCollider2D _polygonCollider;
        private MeshCollider _meshCollider;

        private Camera _mainCamera;

        //선을 이루는 점 저장
        private List<Vector2> _points = new();
        private Vector2 _point;

        private Rigidbody2D _lineRigidbody;

        //_points 배열 중 교차점에 해당하는 점들의 인덱스 저장
        private List<int> _crossingPointIndexes = new();


        void Start()
        {
            _mainCamera = Camera.main;
        }
        void Update()
        {
            InputMouseButton();
        }

        private void InputMouseButton()
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = _mainCamera.nearClipPlane;

            _point = _mainCamera.ScreenToWorldPoint(mousePos);


            if (Input.GetMouseButtonDown(0))
            {
                _line = LinePool.GetLine();
                _line.SetLoopFalse();

                Vector3 vZero = new Vector3(0, 0, 0);
                Quaternion qZero = new Quaternion(0, 0, 0, 0);
                _line.gameObject.transform.SetLocalPositionAndRotation(vZero, qZero);
                _lineRenderer = _line.GetComponent<LineRenderer>();
                _edgeCollider = _line.GetComponent<EdgeCollider2D>();

                _points.Add(_point);
                _lineRenderer.positionCount = 1;
                _lineRenderer.SetPosition(0, _points[0]);

                Debug.Log("_linePos: " + _line.gameObject.transform.position);
            }
            else if (Input.GetMouseButton(0))
            {
                if (_line.IsCollisionObj)
                {
                    Debug.Log("물체 위에(물체와 겹치게) 그릴 수 없습니다!");

                    LinePool.ReturnLineToPool(_line);
                    _line.SetTriggerObjFalse();
                    _points.Clear();
                    return;
                }

                if (_points.Count == 0 || Vector2.Distance(_points[_points.Count - 1], _point) > 0.1f)
                {
                    _points.Add(_point);
                    _lineRenderer.positionCount++;
                    _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, _point);
                    _edgeCollider.points = _points.ToArray();
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (_points.Count == 0)
                {
                    return;
                }

                for (int i = 0; i < _points.Count - 2; i++)
                {
                    for (int j = i + 1; j < _points.Count - 1; j++)
                    {
                        if (Vector2.Distance(_points[i], _points[j]) < 0.1f)
                        {
                            if (_crossingPointIndexes.Count == 0)
                            {
                                _crossingPointIndexes.Add(i);
                                _crossingPointIndexes.Add(j);
                            }
                            else
                            {
                                for (int index = 0; index < _crossingPointIndexes.Count; index++)
                                {
                                    if (Mathf.Abs(_crossingPointIndexes[index] - i) <= 1
                                        || Mathf.Abs(_crossingPointIndexes[index] - j) <= 1)
                                    {
                                        break;
                                    }

                                    if (index == _crossingPointIndexes.Count - 1)
                                    {
                                        _crossingPointIndexes.Add(i);
                                        _crossingPointIndexes.Add(j);
                                    }
                                }
                            }
                            _line.SetLoopTrue();
                            break;
                        }
                    }
                }

                if (_line.IsLoop)
                {
                    DestroyImmediate(_edgeCollider);

                    BakeMeshAndAddPolygonCollider(_line.gameObject);

                    _lineRigidbody = _line.GetComponent<Rigidbody2D>();
                    _lineRigidbody.bodyType = RigidbodyType2D.Dynamic;
                    _lineRigidbody.gravityScale = 1;
                    _lineRigidbody.drag = 1;

                    _line.tag = "LINEOBJ";
                    _line.name = "LineObj";
                }
                _points.Clear();
                _crossingPointIndexes.Clear();
            }
        }

        private void BakeMeshAndAddPolygonCollider(GameObject line)
        {
            _polygonCollider = line.AddComponent<PolygonCollider2D>();

            Mesh mesh = new();
            _lineRenderer.BakeMesh(mesh, true);

            Debug.Log("mesh vertexCount: " + mesh.vertexCount);

            ConvertMeshToPolygon(mesh, _polygonCollider);
        }

        void ConvertMeshToPolygon(Mesh mesh, PolygonCollider2D polygonCollider)
        {
            Vector3[] vertices = mesh.vertices;
            int numVertices = vertices.Length;

            List<Vector2> projectedVertices = new();
            for (int i = 0; i < numVertices; i++)
            {
                projectedVertices.Add(new Vector2(vertices[i].x, vertices[i].y));
            }

            int startPointIndex = 0;
            int endPointIndex = 0;
            polygonCollider.pathCount = _crossingPointIndexes.Count + 1; //1*2+1=3

            _crossingPointIndexes.Sort();

            for (int i = 0; i < polygonCollider.pathCount; i++)//0-3(4개), 3-10(8개). 10-13(4개) 16개
            {
                if (i == polygonCollider.pathCount - 1)
                {
                    endPointIndex = numVertices - 1;
                }
                else
                {
                    if (i == 0)
                    {
                        startPointIndex = 0;
                    }
                    endPointIndex = _crossingPointIndexes[i] * 2;
                }

                Debug.Log("i/ endPointIndex / startPointIndex: " + i + " / " + endPointIndex + " / " + startPointIndex);
                Vector2[] pointPath = new Vector2[endPointIndex - startPointIndex + 1];
                projectedVertices.CopyTo(startPointIndex, pointPath, 0, endPointIndex - startPointIndex);

                pointPath[pointPath.Length - 1] = pointPath[pointPath.Length / 2];

                polygonCollider.SetPath(i, pointPath);

                Debug.Log("i: " + i + "\n newPP Length: " + pointPath.Length
                    + "\n startPidx / endPidx:" + startPointIndex + " / " + endPointIndex);

                startPointIndex = endPointIndex;
            }
        }
    }
}
