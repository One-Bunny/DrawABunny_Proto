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
        }

        public override void FixedUpdateState()
        {
            var velocity = new Vector2(moveInput.x * runnerEntity.status.data.moveSpeed,0);

            velocity.y = runnerEntity.rigid.velocity.y;
            runnerEntity.rigid.velocity = velocity;
        }

        public override void ExitState()
        {
            runnerEntity.OnMove = null;

            runnerEntity.ClearAction(Player.ButtonActions.Jump);
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
