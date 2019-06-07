using System.Collections;
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
        /// </summary>
        /// <param name="a_strNewState"></param>
        public static bool Transition(string a_strNewState)
        {
            if (string.IsNullOrEmpty(a_strNewState))
            {
                Debug.LogError("GameStateMachine::Transition:: ID of game state to transition to is empty.");
                return false;
            }

            return s_Instance.transition(a_strNewState);
        }

        protected override void onStateChanged(string a_strOldStateID, string a_strNewStateID)
        {
            base.onStateChanged(a_strOldStateID, a_strNewStateID);

            Hashtable l_hash = EventManager.GetHashtable();
            l_hash.Add(GameEventTypeConst.ID_OLD_GAME_STATE, a_strOldStateID);
            l_hash.Add(GameEventTypeConst.ID_NEW_GAME_STATE, a_strNewStateID);
            EventManager.Dispatch(GAME_EVENT_TYPE.ON_GAME_STATE_CHANGED, l_hash);
            EventManager.ReturnHashtableToPool(l_hash);
        }
    }
}