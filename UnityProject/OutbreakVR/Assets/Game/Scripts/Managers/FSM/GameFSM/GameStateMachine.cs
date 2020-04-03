﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class GameStateMachine : StateMachine
    {
        /// <summary>
        /// Singleton instance
        /// </summary>
        private static GameStateMachine s_Instance = null;

        public override void initialize()
        {
            if (s_Instance != null)
            {
                return;
            }
            s_Instance = this;
            base.initialize();

            Transition(m_strStartState, SystemConsts.SCENE_NAME_INIT_SCENE, string.Empty);
        }

        public override void destroy()
        {
            if (s_Instance != this)
            {
                return;
            }

            base.destroy();
            s_Instance = null;
        }

        public static string GetCurrentState()
        {
            return s_Instance.m_strCurrentState;
        }

        /// <summary>
        /// Transition to new state
        /// returns if the transition was successful
        /// Static function to call from anywhere
        /// </summary>
        /// <param name="a_strNewState"></param>
        public static void Transition(string a_strNewState, string a_strSceneName, string a_strLevelName)
        {
            if (string.IsNullOrEmpty(a_strNewState))
            {
                Debug.LogError("GameStateMachine::Transition:: ID of game state to transition to is empty.");
                return;
            }
            s_Instance.transitionToGameState(a_strNewState, a_strSceneName, a_strLevelName);
        }

        /// <summary>
        /// Internal function to transition, switch to new scene before calling the new state enter
        /// returns true if transition is allowed, else false
        /// </summary>
        /// <param name="a_strNewState"></param>
        /// <param name="a_strSceneName"></param>
        /// <param name="a_strLevelName"></param>
        protected bool transitionToGameState(string a_strNewState, string a_strSceneName, string a_strLevelName)
        {
            if (!transition(a_strNewState, /*a_bIsTransitionToNewState is always false for this func*/ false))
            {
                return false;
            }

            EventHash l_EventHash = EventManager.GetEventHashtable();
            l_EventHash.Add(GameEventTypeConst.ID_OLD_GAME_STATE, s_Instance.m_strLastState);
            l_EventHash.Add(GameEventTypeConst.ID_NEW_GAME_STATE, s_Instance.m_strCurrentState);
            EventManager.Dispatch(GAME_EVENT_TYPE.ON_GAME_STATE_ENDED, l_EventHash);

            ///Changes the level being used by assets, i.e. task and objective
            if (!string.IsNullOrEmpty(a_strLevelName))
            {
                EventHash l_Hashtable = EventManager.GetEventHashtable();
                l_Hashtable.Add(GameEventTypeConst.ID_LEVEL_TYPE, a_strLevelName);
                EventManager.Dispatch(GAME_EVENT_TYPE.ON_LEVEL_SELECTED, l_Hashtable);
            }

            GameManager.LoadScene(a_strSceneName, s_Instance.onLevelSceneLoadComplete);

            return true;
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
    }
}