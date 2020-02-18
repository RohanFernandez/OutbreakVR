using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class StateMachine : AbsComponentHandler
    {
        [SerializeField]
        protected List<ManagedState> m_lstRegisteredManagedStates = null;

        [SerializeField]
        protected string m_strStartState = string.Empty;

        [SerializeField]
        protected string m_strLastState = string.Empty;

        [SerializeField]
        protected string m_strCurrentState = string.Empty;

        [SerializeField]
        protected bool m_bAllowTransitionToSelf = false;

        public override void initialize()
        {
            
        }

        public override void destroy()
        {

        }

        /// <summary>
        /// Transitions to new state
        /// Fires an event on state changed
        /// returns true if transition is allowed, else false
        /// </summary>
        /// <param name="a_strNewState"></param>
        protected virtual bool transition(string a_strNewState, bool a_bIsTransitionToNewState = true)
        {
            string l_strOldStateId = m_strCurrentState;
            string l_strNewStateId = a_strNewState;

            ManagedState l_OldManagedState = getRegisteredManagedState(l_strOldStateId);
            
            if (!m_bAllowTransitionToSelf && l_strOldStateId.Equals(l_strNewStateId, System.StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            m_strCurrentState = l_strNewStateId;
            m_strLastState = l_strOldStateId;

            if (l_OldManagedState != null)
            {
                l_OldManagedState.onStateExit(l_strNewStateId);
            }

            if (a_bIsTransitionToNewState)
            {
                ManagedState l_NewManagedState = getRegisteredManagedState(m_strCurrentState);
                if (l_NewManagedState != null)
                {
                    l_NewManagedState.onStateEnter(m_strLastState);
                }
            }

            return true;
        }

        /// <summary>
        /// Returns registered state in list with ID else returns null
        /// </summary>
        /// <param name="a_strStateName"></param>
        /// <returns></returns>
        public ManagedState getRegisteredManagedState(string a_strStateName)
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