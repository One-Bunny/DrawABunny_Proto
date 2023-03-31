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
        private MeshCollider _meshCollider;

        private Camera _mainCamera;

        //���� �̷�� �� ����
        private List<Vector2> _points = new();
        private Vector2 _point;

        private Rigidbody2D _lineRigidbody;

        //_points �迭 �� �������� �ش��ϴ� ������ �ε��� ����
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
                    Debug.Log("��ü ����(��ü�� ��ġ��) �׸� �� �����ϴ�!");

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

                            //Debug.Log("duplication, i:" + i + " / j: " + j);
                            //Debug.Log("points Count: " + _points.Count);
                            //Debug.Log("distance: " + Vector2.Distance(_points[i], _points[j]));

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

            for (int i = 0; i < polygonCollider.pathCount; i++)//0-3(4��), 3-10(8��). 10-13(4��) 16��
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

                Debug.Log("i/ endPointIndex / startPointIndex: " + i+" / "+endPointIndex +" / "+ startPointIndex);
                Vector2[] pointPath = new Vector2[endPointIndex - startPointIndex + 1];
                projectedVertices.CopyTo(startPointIndex, pointPath, 0, endPointIndex - startPointIndex);

                pointPath[pointPath.Length - 1] = pointPath[pointPath.Length / 2];

                polygonCollider.SetPath(i, pointPath);

                Debug.Log("i: " + i + "\n newPP Length: " + pointPath.Length
                    + "\n startPidx / endPidx:" + startPointIndex + " / " + endPointIndex);

                startPointIndex = endPointIndex;
            }
        }



        //����� �Ⱦ��̴� �ڵ��
        private void BakeMeshAndAddMeshCollider(GameObject line)
        {
            DestroyImmediate(_lineRigidbody);
            line.AddComponent<Rigidbody>();
            line.AddComponent<MeshCollider>();
            _meshCollider = line.GetComponent<MeshCollider>();

            Mesh mesh = new();
            _lineRenderer.BakeMesh(mesh, true);
            _meshCollider.sharedMesh = mesh;
            _meshCollider.convex = true;


            //mesh.RecalculateBounds();
            //mesh.RecalculateNormals();

            //MeshFilter meshFilter = line.AddComponent<MeshFilter>();
            //meshFilter.mesh = mesh;

            //line.AddComponent<MeshRenderer>();
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

                for (int sideCount = 0; sideCount < 1; sideCount++)
                {
                    for (int i = 0; i < meshPoints.Length; i++)
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
