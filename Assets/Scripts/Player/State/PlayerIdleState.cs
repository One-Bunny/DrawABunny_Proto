using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneBunny
{
    public abstract class PlayerIdleState : FSMState<Player>
    {
        #region Runner
        public PlayerIdleState(IFSMRunner runner) : base(runner)
        {
        }
        #endregion

        

    }
}