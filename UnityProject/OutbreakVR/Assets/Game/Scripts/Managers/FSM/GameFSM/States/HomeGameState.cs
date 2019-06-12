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
            GameManager.LoadScene(SystemConsts.SCENE_NAME_HOME_SCENE, onLoadHomeSceneComplete);
        }

        public override void onStateExit(string a_strOldState)
        {
            base.onStateExit(a_strOldState);
        }

        /// <summary>
        /// On loading on home scene complete do what you want to initialize
        /// </summary>
        private void onLoadHomeSceneComplete()
        {

        }
    }
}