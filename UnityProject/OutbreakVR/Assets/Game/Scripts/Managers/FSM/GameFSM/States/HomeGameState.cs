using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class HomeGameState : ManagedState
    {
        public override void onStateEnter(string a_strOldState)
        {
            base.onStateEnter(a_strOldState);
        }

        public override void onStateExit(string a_strNextState)
        {
            base.onStateExit(a_strNextState);
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