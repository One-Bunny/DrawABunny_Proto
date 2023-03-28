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
            runnerEntity.rigid.AddForce(new Vector2(0f, 500f));
        }

        public override void UpdateState()
        {
            Debug.Log("TEST");
        }

        public override void ExitState()
        {

        }

    }

}