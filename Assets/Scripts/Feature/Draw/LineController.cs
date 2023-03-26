using UnityEngine;

namespace OneBunny
{
    public class LineController : MonoBehaviour
    {
        private bool isTriggerObj;
        public bool IsTriggerObj
        {
            get
            {
                return isTriggerObj;
            }
            private set
            {
                isTriggerObj = value;
            }
        }

        private bool isTriggerLine;
        public bool IsTriggerLine
        {
            get
            {
                return isTriggerLine;
            }
            private set
            {
                isTriggerLine = value;
            }
        
        }


        public void SetTriggerObjFalse()
        {
            this.IsTriggerObj = false;
        }

        //private void OnTriggerEnter2D(Collider2D collision)
        //{
        //    if (collision.CompareTag("OBJ"))
        //    {
        //        isTriggerObj = true;
        //    }
        //    else if (collision.CompareTag("LINE"))
        //    {
        //        isTriggerLine = true;
        //    }
        //}

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("LINE"))
            {

            }
                isTriggerObj = true;
        }
    }
}