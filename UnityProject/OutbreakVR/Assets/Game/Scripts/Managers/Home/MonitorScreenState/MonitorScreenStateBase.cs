using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class MonitorScreenStateBase : ManagedState
    {
        [SerializeField]
        private UnityEngine.UI.Toggle m_Toggle = null;

        public override void onStateEnter(string a_strOldState)
        {
            base.onStateEnter(a_strOldState);
            gameObject.SetActive(true);
            m_Toggle.isOn = true;
        }

        public override void onStateExit(string a_strNewState)
        {
            base.onStateExit(a_strNewState);
            gameObject.SetActive(false);
        }
    }
}