using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class StateMachine : AbsComponentHandler
    {
        [SerializeField]
        private List<ManagedState> m_lstRegisteredManagedStates = null;

        [SerializeField]
        private string m_strStartState = string.Empty;

        [SerializeField]
        protected string m_strCurrentState = string.Empty;

        [SerializeField]
        private bool m_bAllowTransitionToSelf = false;

        public override void initialize()
        {
            transition(m_strStartState);
        }

        public override void destroy()
        {

        }

        /// <summary>
        /// Transitions to new state
        /// Fires an event on state changed
        /// </summary>
        /// <param name="a_strNewState"></param>
        public bool transition(string a_strNewState, bool a_bIsStateLoad = false)
        {
            string l_strOldStateId = m_strCurrentState;
            string l_strNewStateId = a_strNewState;

            ManagedState l_OldManagedState = getRegisteredManagedState(l_strOldStateId);
            ManagedState l_NewManagedState = getRegisteredManagedState(l_strNewStateId);

            if (!m_bAllowTransitionToSelf && l_strOldStateId.Equals(l_strNewStateId, System.StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (l_OldManagedState != null)
            {
                l_OldManagedState.onStateExit(l_strNewStateId);
            }
            m_strCurrentState = l_strNewStateId;

            if (l_NewManagedState != null)
            {
                l_NewManagedState.onStateEnter(l_strOldStateId);
            }

            onStateChanged(l_strOldStateId, l_strNewStateId, a_bIsStateLoad);
            return true;
        }

        /// <summary>
        /// Called on state change event is called
        /// </summary>
        /// <param name="a_strOldStateID"></param>
        /// <param name="a_strNewStateID"></param>
        protected virtual void onStateChanged(string a_strOldStateID, string a_strNewStateID, bool a_bIsStateLoad)
        {

        }

        /// <summary>
        /// Returns registered state in list with ID else returns null
        /// </summary>
        /// <param name="a_strStateName"></param>
        /// <returns></returns>
        private ManagedState getRegisteredManagedState(string a_strStateName)
        {
            int l_iStateCount = m_lstRegisteredManagedStates.Count;
            for (int l_iStateIndex = 0; l_iStateIndex < l_iStateCount; l_iStateIndex++)
            {
                if (a_strStateName.Equals(m_lstRegisteredManagedStates[l_iStateIndex].StateID, System.StringComparison.OrdinalIgnoreCase))
                {
                    return m_lstRegisteredManagedStates[l_iStateIndex];
                }
            }
            return null;
        }
    }
}