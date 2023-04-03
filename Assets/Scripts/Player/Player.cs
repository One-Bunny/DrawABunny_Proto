using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
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
            INTERACTION,

            End
        }

        [field: SerializeField] public Rigidbody2D rigid { get; private set; }
        public PlayerStatus status;

        public GameObject board;

        public SkeletonAnimation _skeletonAnimation;

        private void Awake()
        {
            InitInputs();
            SetUp(States.IDLE);
        }

        protected override void Update()
        {
            base.Update();

            UpdateInputs();

            Debug.DrawRay(transform.position, Vector2.down, Color.red);
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }

    }
}
