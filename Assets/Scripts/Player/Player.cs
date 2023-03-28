using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneBunny
{
    public partial class Player : FSMRunner<Player>, IFSMRunner
    {
        [field: SerializeField] public Rigidbody2D rigid { get; private set; }
         
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
            InitInputs();
            
            SetUp(States.MOVE);
        }

        protected override void Update()
        {
            UpdateInputs();
        }



    }
}
