using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class HomePauseState : PauseManagedStateBase
    {
        public override void onStateEnter(string a_strState)
        {
            base.onStateEnter(a_strState);
        }

        public override void onStateExit(string a_strState)
        {
            base.onStateExit(a_strState);
        }

        /// <summary>
        /// On trigger/ select button pressed
        /// </summary>
        public override void onSelectPressed()
        {
            base.onSelectPressed();
            GameManager.GoToHome();
        }
    }
}
