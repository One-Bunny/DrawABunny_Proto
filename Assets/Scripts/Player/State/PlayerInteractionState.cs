using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneBunny
{
    [FSMState((int)Player.States.INTERACTION)]
    public class PlayerInteractionState : FSMState<Player>
    {
        #region Runner
        public PlayerInteractionState(IFSMRunner runner) : base(runner)
        {
            
        }
        #endregion

        public override void BeginState()
        {
            runnerEntity.board.SetActive(true);
        }

        public override void UpdateState()
        {
            if (!runnerEntity.board.activeSelf)
            {
                runnerEntity.ChangeState(Player.States.IDLE);
            }
        }
        
        public override void ExitState()
        {
        } 
        
    }
}