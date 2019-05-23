using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class HomeGameState : ManagedState
    {
        public override void onStateEnter(string a_strNewState)
        {
            Debug.LogError("StateEntered :"+ a_strNewState);
        }

        public override void onStateExit(string a_strOldState)
        {
            Debug.LogError("StateExited : "+ a_strOldState);
        }
    }
}