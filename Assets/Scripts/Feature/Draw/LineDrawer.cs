using System.Collections.Generic;
using UnityEngine;

namespace OneBunny
{
    public sealed class LineDrawer : MonoBehaviour
    {
        [SerializeField] private GameObject _linePrefab;

        private GameObject _gameObject;
        private LineRenderer _lineRenderer;
        private EdgeCollider2D _edgeCollider;


        //선의 시작점 / 끝점 저장
        private List<Vector2> _points = new();

        private LineController _line;

        void Start()
        {
        }
        void Update()
        {

            Vector2 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);


            if (Input.GetMouseButtonDown(0))
            {
                _line = LinePool.GetLine();
                _lineRenderer = _line.GetComponent<LineRenderer>();
                _edgeCollider = _line.GetComponent<EdgeCollider2D>();


                _points.Add(point);
                _lineRenderer.positionCount = 1;
                _lineRenderer.SetPosition(0, _points[0]);
            }
            else if (Input.GetMouseButton(0))
            {

                if (_line.IsTriggerObj)
                {
                    Debug.Log("물체 위에(물체와 겹치게) 그릴 수 없습니다!");

                    LinePool.ReturnObject(_line);
                    _line.SetTriggerObjFalse();
                    _points.Clear();
                    return;
                }

                if (_points.Count == 0 || _points.Count == 1 ||
                    (_points.Count > 1 && Vector2.Distance(_points[_points.Count - 1], point) > 0.1f))
                {
                    _points.Add(point);
                    _lineRenderer.positionCount++;
                    _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, point);
                    _edgeCollider.points = _points.ToArray();
                }

            }
            else if (Input.GetMouseButtonUp(0))
            {
                _points.Clear();
            }
        }

    }

}
