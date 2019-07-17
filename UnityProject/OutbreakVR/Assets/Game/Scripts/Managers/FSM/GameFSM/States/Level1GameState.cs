using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class Level1GameState : ManagedState
    {
        public override string getSceneName
        {
            get { return GameConsts.STATE_NAME_LEVEL1; }
        }

        public override void onStateEnter(string a_strOldState)
        {
            base.onStateEnter(a_strOldState);
        }

        public override void onStateExit(string a_strNewState)
        {
            base.onStateExit(a_strNewState);
        }

        /// <summary>
        /// event called on level scene loaded
        /// </summary>
        protected override void onSceneLoadComplete()
        {
            GameStateMachine.Transition(GameManager.InGameState);
        }
    }
}