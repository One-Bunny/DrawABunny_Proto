using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneBunny
{
    public class Player : FSMRunner<Player>, IFSMRunner
    {
        public enum States : int
        {
            Start,

            IDLE,
            MOVE,
            JUMP,

            End
        }


        private void Awake()
        {
            SetUp(States.IDLE);
        }

        protected override void Update()
        {

        }



    }
}
