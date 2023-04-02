using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace OneBunny
{
    [FSMState((int)Player.States.MOVE)]
    public class PlayerMoveState : FSMState<Player>
    {
        #region Runner
        public PlayerMoveState(IFSMRunner runner) : base(runner)
        {
            
        }
        #endregion

        private Vector2 moveInput;
        
        public override void BeginState()
        {
            runnerEntity.OnMove = (x) => moveInput = x;

            runnerEntity.SetAction(Player.ButtonActions.Jump, OnJump);
            runnerEntity._skeletonAnimation.AnimationState.SetAnimation(0, "P_Move_Animation", true);

        }

        public override void UpdateState()
        {
            if (moveInput == Vector2.zero)
            {
                runnerEntity.ChangeState(Player.States.IDLE);
            }
        }

        public override void FixedUpdateState()
        {
            var velocity = new Vector2(moveInput.x * runnerEntity.status.data.moveSpeed,0);

            runnerEntity._skeletonAnimation.skeleton.ScaleX = moveInput.x < 0 ? -1f : 1f;
            
            velocity.y = runnerEntity.rigid.velocity.y;
            runnerEntity.rigid.velocity = velocity;
        }

        public override void ExitState()
        {
            runnerEntity.OnMove = null;
            runnerEntity.rigid.velocity = Vector2.zero;

            runnerEntity.ClearAction(Player.ButtonActions.Jump);
            runnerEntity.ClearAction(Player.ButtonActions.Interaction);
        }

        private void OnJump(bool isOn)
        {
            if(isOn)
            {
                runnerEntity.ChangeState(Player.States.JUMP);
            }
        }
    }
}
