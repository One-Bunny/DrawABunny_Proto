using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace OneBunny
{
    [FSMState((int)Player.States.JUMP)]
    public class PlayerJumpState : FSMState<Player>
    {
        #region Runner

        public PlayerJumpState(IFSMRunner runner) : base(runner)
        {
        }

        #endregion

        private bool _isGrounded = false;
        readonly private LayerMask _groundMask = LayerMask.GetMask("Ground");

        public override void BeginState()
        {
            var velocity = runnerEntity.rigid.velocity;
            velocity.y = 10f;

            runnerEntity.rigid.velocity = velocity;
        }

        public override void FixedUpdateState()
        {
            if (runnerEntity.rigid.velocity.y < 0)
            {
                Debug.Log($"PLAYER VELOCITY : {runnerEntity.rigid.velocity.y}");
                _isGrounded = Physics2D.Raycast(runnerEntity.transform.position, Vector2.down, 0.1f, _groundMask);
            }

            if (_isGrounded)
            {
                runnerEntity.ChangeState(Player.States.MOVE);
            }
        }

        public override void ExitState()
        {
            _isGrounded = false;
        }
    }
}