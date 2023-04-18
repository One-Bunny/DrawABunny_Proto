using UnityEngine;

namespace OneBunny
{
    public class LineController : MonoBehaviour
    {
        [SerializeField] private GameObject _paletteCanvas;

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

        private Rigidbody2D rigidbody;
        public Rigidbody2D Rigidbody
        {
            get
            {
                return rigidbody;
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

        private void Awake()
        {
            lineRenderer = gameObject.GetComponent<LineRenderer>();
            edgeCollider = gameObject.GetComponent<EdgeCollider2D>();
            rigidbody = gameObject.GetComponent<Rigidbody2D>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            isCollisionObj = true;
        }
    }
}