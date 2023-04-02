using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OneBunny
{
    public sealed class LineDrawer : MonoBehaviour
    {
        private LineController _line;
        private LineRenderer _lineRenderer;
        private EdgeCollider2D _edgeCollider;
        private PolygonCollider2D _polygonCollider;

        private Camera _mainCamera;

        //선을 이루는 점 저장
        private List<Vector2> _points = new();
        private Vector2 _point;

        private Rigidbody2D _rigidbody;

        //_points 배열 중 교차점에 해당하는 점들의 인덱스 저장
        private List<int> _crossingPointIndexes = new();

        private string sortingLayerName = "Progress";
        private int sortingOrder = 10;


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

                if (_points.Count == 0 || Vector2.Distance(_points[_points.Count - 1], _point) >= 0.1f)
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
                        if (Vector2.Distance(_points[i], _points[j]) <= 0.1f)
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

                    _rigidbody = _line.Rigidbody;
                    _rigidbody.bodyType = RigidbodyType2D.Dynamic;
                    _rigidbody.gravityScale = 1;
                    _rigidbody.drag = 1;

                    //_line.tag = "LINEOBJ";
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
            AddMesh(line, _polygonCollider);
        }

        private void AddMesh(GameObject line, PolygonCollider2D polygonCollider)
        {
            MeshFilter meshFilter = line.GetComponent<MeshFilter>();
            MeshRenderer meshRenderer = line.GetComponent<MeshRenderer>();
            Mesh mesh = new();

            List<Vector2> points = new();

            for (int i = 1; i < polygonCollider.pathCount - 1; i++)
            {
                Vector2[] pathPoints = polygonCollider.GetPath(i);
                Vector2[] nextPathPoints = polygonCollider.GetPath(i + 1);
                int crossingCount = 0;

                foreach (Vector2 pathPoint in pathPoints)
                {
                    foreach (Vector2 nextPathPoint in nextPathPoints)
                    {
                        if (Vector2.Distance(pathPoint, nextPathPoint) <= 0.1f)
                        {
                            crossingCount++;
                        }
                    }
                }
                Debug.Log(i + " / " + crossingCount);

                if (crossingCount == 2 || i % 2 != 0)
                {
                    for (int j = 0; j < polygonCollider.GetPath(i).Length; j++)
                    {
                        points.Add(pathPoints[j]);
                    }
                }
            }

            using (var vertexHelper = new VertexHelper())
            {
                int vertexCount = 0;

                for (int i = 0; i < points.Count; i++)
                {
                    Vector2 vertex = points[i];
                    UIVertex uiVertex = new UIVertex();

                    uiVertex.position = new Vector3(vertex.x, vertex.y, 0);
                    uiVertex.uv0 = new Vector2(vertex.x, vertex.y);

                    vertexHelper.AddVert(uiVertex);

                    if (((i > 1) && (i < points.Count)) ||
                        (i > points.Count + 1))
                    {
                        vertexHelper.AddTriangle(0, vertexCount - 1, vertexCount);
                    }

                    vertexCount++;
                }

                vertexHelper.FillMesh(mesh);

            }

            mesh.RecalculateBounds();
            mesh.RecalculateNormals();

            meshFilter.mesh = mesh;


            Color[] colors = new Color[mesh.vertices.Length];

            for (int i = 0; i < mesh.vertices.Length; i++)
            {
                colors[i] = Color.red;
            }

            mesh.SetColors(colors);
            meshRenderer.sortingLayerName = sortingLayerName;
            //meshRenderer.sortingOrder = sortingOrder;

        }

        private void ConvertMeshToPolygon(Mesh mesh, PolygonCollider2D polygonCollider)
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

                //Debug.Log("i/ endPointIndex / startPointIndex: " + i + " / " + endPointIndex + " / " + startPointIndex);
                Vector2[] pointPath = new Vector2[endPointIndex - startPointIndex + 1];
                projectedVertices.CopyTo(startPointIndex, pointPath, 0, endPointIndex - startPointIndex);

                pointPath[pointPath.Length - 1] = pointPath[pointPath.Length / 2];

                polygonCollider.SetPath(i, pointPath);

                //Debug.Log("i: " + i + "\n newPP Length: " + pointPath.Length
                //    + "\n startPidx / endPidx:" + startPointIndex + " / " + endPointIndex);

                startPointIndex = endPointIndex;
            }
        }
    }
}
