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
        }

        public override string getSceneName
        {
            get { return GameConsts.STATE_NAME_HOME; }
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