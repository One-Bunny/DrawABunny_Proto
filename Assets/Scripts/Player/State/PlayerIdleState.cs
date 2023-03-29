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

        public override void BeginState()
        {

        }

        public override void ExitState()
        {

        }

    }
}