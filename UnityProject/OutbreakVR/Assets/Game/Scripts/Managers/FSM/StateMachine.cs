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
        protected string m_strLastState = string.Empty;

        [SerializeField]
        protected string m_strCurrentState = string.Empty;

        [SerializeField]
        private bool m_bAllowTransitionToSelf = false;

        public override void initialize()
        {
            transition(m_strStartState, SystemConsts.SCENE_NAME_INIT_SCENE, string.Empty);
        }

        public override void destroy()
        {

        }

        /// <summary>
        /// Transitions to new state
        /// Fires an event on state changed
        /// </summary>
        /// <param name="a_strNewState"></param>
        public void transition(string a_strNewState, string a_strSceneName, string a_strLevelName)
        {
            string l_strOldStateId = m_strCurrentState;
            string l_strNewStateId = a_strNewState;

            ManagedState l_OldManagedState = getRegisteredManagedState(l_strOldStateId);
            
            if (!m_bAllowTransitionToSelf && l_strOldStateId.Equals(l_strNewStateId, System.StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            m_strCurrentState = l_strNewStateId;
            m_strLastState = l_strOldStateId;

            if (l_OldManagedState != null)
            {
                l_OldManagedState.onStateExit(l_strNewStateId);
            }

            EventHash l_EventHash = EventManager.GetEventHashtable();
            l_EventHash.Add(GameEventTypeConst.ID_OLD_GAME_STATE, m_strLastState);
            l_EventHash.Add(GameEventTypeConst.ID_NEW_GAME_STATE, m_strCurrentState);
            EventManager.Dispatch(GAME_EVENT_TYPE.ON_GAME_STATE_ENDED, l_EventHash);

            ///Changes the level being used by assets, i.e. task and objective
            if (!string.IsNullOrEmpty(a_strLevelName))
            {
                EventHash l_Hashtable = EventManager.GetEventHashtable();
                l_Hashtable.Add(GameEventTypeConst.ID_LEVEL_TYPE, a_strLevelName);
                EventManager.Dispatch(GAME_EVENT_TYPE.ON_LEVEL_SELECTED, l_Hashtable);
            }

            ///Load scene will call the callback directly if already loaded
            GameManager.LoadScene(a_strSceneName, onLevelSceneLoadComplete);
        }

        /// <summary>
        /// Callback on the level load scene complete
        /// </summary>
        private void onLevelSceneLoadComplete()
        {
            ManagedState l_NewManagedState = getRegisteredManagedState(m_strCurrentState);
            if (l_NewManagedState != null)
            {
                l_NewManagedState.onStateEnter(m_strLastState);
            }

            EventHash l_EventHash = EventManager.GetEventHashtable();
            l_EventHash.Add(GameEventTypeConst.ID_NEW_GAME_STATE, m_strCurrentState);
            l_EventHash.Add(GameEventTypeConst.ID_OLD_GAME_STATE, m_strLastState);
            EventManager.Dispatch(GAME_EVENT_TYPE.ON_GAME_STATE_STARTED, l_EventHash);
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