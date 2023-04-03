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

        private Vector2 moveInput;

        public override void BeginState()
        {
            runnerEntity.OnMove = (x) => moveInput = x;

            var velocity = runnerEntity.rigid.velocity;
            velocity.y = runnerEntity.status.data.jumpPower;

            runnerEntity.rigid.velocity = velocity;
            runnerEntity._skeletonAnimation.AnimationState.SetAnimation(0, "p_Jump_animation", false);
        }

        public override void FixedUpdateState()
        {
            var velocity = new Vector2(moveInput.x * runnerEntity.status.data.moveSpeed, 0);

            velocity.y = runnerEntity.rigid.velocity.y;


            if (runnerEntity.rigid.velocity.y < 0)
            {
                _isGrounded = Physics2D.Raycast(runnerEntity.transform.position, Vector2.down, 1f, _groundMask);
            }
            else
            {
                velocity.y = runnerEntity.rigid.velocity.y * 0.8f;
            }

            runnerEntity.rigid.velocity = velocity;

            if (_isGrounded)
            {
                runnerEntity.ChangeState(Player.States.MOVE);
            }
        }

        public override void ExitState()
        {
            runnerEntity.OnMove = null;
            _isGrounded = false;
        }
    }
}