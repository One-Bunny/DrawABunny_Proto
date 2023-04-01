using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneBunny
{
    [FSMState((int)Player.States.IDLE)]
    public class PlayerIdleState : FSMState<Player>
    {
        #region Runner
        public PlayerIdleState(IFSMRunner runner) : base(runner)
        {
        }
        #endregion

        private Vector2 moveInput = Vector2.zero;
        
        public override void BeginState()
        {
            runnerEntity.SetAction(Player.ButtonActions.Jump, OnJump);
            runnerEntity.OnMove = (x) => moveInput = x;
            runnerEntity._skeletonAnimation.AnimationState.SetAnimation(0, "P_Default_Animastion", true);
        }

        public override void UpdateState()
        {
            if (moveInput != Vector2.zero)
            {
                runnerEntity.ChangeState(Player.States.MOVE);
            }
        }

        private void OnJump(bool isOn)
        {
            if (isOn)
            {
                runnerEntity.ChangeState(Player.States.JUMP);
            }
        }

        public override void ExitState()
        {
            runnerEntity.OnMove = null;
        }

    }
}