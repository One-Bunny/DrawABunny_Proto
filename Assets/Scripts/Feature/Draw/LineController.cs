using UnityEngine;

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

        private bool isLoop;
        public bool IsLoop
        {
            get
            {
                return isLoop;
            }
            private set
            {
                isLoop = value;
            }
        }

        public void SetTriggerObjFalse()
        {
            this.IsCollisionObj = false;
        }

        public void SetLoopTrue()
        {
            this.isLoop = true;
        }

        public void SetLoopFalse()
        {
            this.isLoop = false;
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