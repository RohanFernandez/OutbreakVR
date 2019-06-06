using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class Level1GameState : ManagedState
    {
        public override void onStateEnter(string a_strOldState)
        {
            base.onStateEnter(a_strOldState);

            GameManager.LoadScene(GameConsts.STATE_NAME_LEVEL1, onLevelSceneLoaded);
        }

        public override void onStateExit(string a_strNewState)
        {
            base.onStateExit(a_strNewState);
        }

        /// <summary>
        /// event called on level scene loaded
        /// </summary>
        private void onLevelSceneLoaded()
        {

        }
    }
}