using UnityEngine;
using System.Collections.Generic;

namespace OneBunny
{
    public class LineController : MonoBehaviour
    {

        private LineRenderer lineRenderer;
        public LineRenderer LineRenderer
        {
            get
            {
                return lineRenderer;
            }
        }

        private EdgeCollider2D edgeCollider;
        public EdgeCollider2D EdgeCollider
        {
            get
            {
                return edgeCollider;
            }
        }

        private bool isCollisionObj;
        public bool IsCollisionObj
        {
            get
            {
                return isCollisionObj;
            }
            private set
            {
                isCollisionObj = value;
            }
        }

        public void SetTriggerObjFalse()
        {
            this.IsCollisionObj = false;
        }

        private void Start()
        {
            lineRenderer = gameObject.GetComponent<LineRenderer>();
            edgeCollider = gameObject.GetComponent<EdgeCollider2D>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            isCollisionObj = true;
        }
    }
}