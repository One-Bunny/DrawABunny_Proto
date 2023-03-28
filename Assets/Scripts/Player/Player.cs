using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneBunny
{
    public partial class Player : FSMRunner<Player>, IFSMRunner
    {
        public enum States : int
        {
            Start,

            IDLE,
            MOVE,
            JUMP,

            End
        }

        [field: SerializeField] public Rigidbody2D rigid { get; private set; }


        private void Awake()
        {
            InitInputs();
            SetUp(States.MOVE);
        }

        protected override void Update()
        {
            base.Update();

            UpdateInputs();

            Debug.DrawRay(transform.position, Vector2.down, Color.red);
        }

    }
}
