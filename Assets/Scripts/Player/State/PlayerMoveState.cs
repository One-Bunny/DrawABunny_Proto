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

        public override void BeginState()
        {
            Debug.Log("MOVE");
        }

    }
}
