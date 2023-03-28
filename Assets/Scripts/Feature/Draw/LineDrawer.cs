using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OneBunny
{
    public sealed class LineDrawer : MonoBehaviour
    {
        [SerializeField] private GameObject _linePrefab;

        private LineController _line;
        private GameObject _gameObject;
        private LineRenderer _lineRenderer;
        private EdgeCollider2D _edgeCollider;


        private Camera _mainCamera;
        //선의 시작점 / 끝점 저장
        private List<Vector2> _points = new();
        private Vector2 _point;

        private Rigidbody2D _lineRigidbody;

        void Start()
        {
            _mainCamera = Camera.main;
        }
        void Update()
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

                    LinePool.ReturnObject(_line);
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
                            Debug.Log("duplication");
                            _line.SetLoopTrue();
                            break;
                        }
                    }
                }

                if (_line.IsLoop)
                {
                    DestroyImmediate(_edgeCollider);
                    AddPolygonCollider2D(_points.ToArray(), _line.gameObject) ;
                    _lineRigidbody = _line.GetComponent<Rigidbody2D>();
                    _lineRigidbody.bodyType = RigidbodyType2D.Dynamic;
                    _lineRigidbody.gravityScale = 1;
                    _lineRigidbody.drag = 1;
                    //_line.tag = "LINEOBJ";
                    _line.name = "LineObj";
                }
                _points.Clear();
            }
        }


        private void AddPolygonCollider2D(Vector2[] points, GameObject line)
        {
            Vector2[] meshPoints = new Vector2[points.Length];
            System.Array.Copy(points, meshPoints, points.Length);

            Mesh mesh = new();

            using (var vertexHelper = new VertexHelper())
            {
                Debug.Log("VertexHelper");
                int vertexCount = 0;

                for(int sideCount=0; sideCount < 1; sideCount++)
                {
                    for(int i=0; i < meshPoints.Length; i++)
                    {
                        int iMapped = (sideCount == 0) ? i : ((meshPoints.Length - 1) - i);

                        Vector2 vertex = meshPoints[iMapped];

                        UIVertex uiVertex = new UIVertex();
                        uiVertex.position = new Vector2(vertex.x, vertex.y);
                        uiVertex.uv0 = new Vector2(vertex.x, vertex.y);

                        vertexHelper.AddVert(uiVertex);

                        if (((i > 1) && (i < meshPoints.Length)) ||
                        (i > meshPoints.Length + 1))
                        {
                            // topology is a fan
                            if (sideCount == 0)
                            {
                                vertexHelper.AddTriangle(0, vertexCount - 1, vertexCount);
                            }
                            if (sideCount == 1)
                            {
                                vertexHelper.AddTriangle(meshPoints.Length, vertexCount - 1, vertexCount);
                            }
                        }

                        vertexCount++;
                    }
                }

                vertexHelper.FillMesh(mesh);
            }

            Debug.Log("AddPolygon");

            PolygonCollider2D polygonnCollider = line.AddComponent<PolygonCollider2D>();
            polygonnCollider.points = meshPoints;

            mesh.RecalculateBounds();
            mesh.RecalculateNormals();

            MeshFilter meshFilter = line.AddComponent<MeshFilter>();
            meshFilter.mesh = mesh;

            line.AddComponent<MeshRenderer>();
        }
    }
}
