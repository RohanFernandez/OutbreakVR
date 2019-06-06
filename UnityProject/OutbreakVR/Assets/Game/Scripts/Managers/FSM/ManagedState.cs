using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public abstract class ManagedState : MonoBehaviour
    {
        [SerializeField]
        private string m_strStateID = string.Empty;
        public string StateID
        {
            get { return m_strStateID; }
        }

        public virtual void onStateEnter(string a_strOldState)
        {
            Debug.Log("<color=BLUE> ManagedState::onStateEnter :: </color> Entered: " + m_strStateID + "   , Exited : " + a_strOldState);
        }

        public virtual void onStateExit(string a_strNewState)
        {
            Debug.Log("<color=BLUE> ManagedState::onStateExit:: OnStateExit :: </color> Entered: " + a_strNewState + "   , Old State : " + m_strStateID );
        }
    }
}