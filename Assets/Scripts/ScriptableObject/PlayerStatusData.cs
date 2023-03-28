using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneBunny
{
    public class PlayerStatusData : ScriptableObject
    {
        private float moveSpeed;
        public float MoveSpeed
        {
            get
            {
                return moveSpeed;
            }
            set
            {
                moveSpeed = value;
            }
        }
        
        private float jumpPower;
        public float JumpPower
        {
            get
            {
                return jumpPower;
            }
            set
            {
                jumpPower = value;
            }
        }
    }
}
