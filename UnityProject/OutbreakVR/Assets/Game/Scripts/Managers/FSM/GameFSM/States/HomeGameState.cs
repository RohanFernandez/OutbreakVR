using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class HomeGameState : ManagedState
    {
        public override void onStateEnter(string a_strNewState)
        {
            base.onStateEnter(a_strNewState);
            PlayerManager.SetPlayerState(PLAYER_STATE.MENU_SELECTION);
        }

        public override void onStateExit(string a_strOldState)
        {
            base.onStateExit(a_strOldState);
        }

        /// <summary>
        /// On loading on home scene complete do what you want to initialize
        /// </summary>
        protected override void onSceneLoadComplete()
        {
            base.onSceneLoadComplete();
        }
    }
}