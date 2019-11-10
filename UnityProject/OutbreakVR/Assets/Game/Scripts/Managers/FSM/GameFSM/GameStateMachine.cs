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
        public static void Transition(string a_strNewState, string a_strSceneName, string a_strLevelName)
        {
            if (string.IsNullOrEmpty(a_strNewState))
            {
                Debug.LogError("GameStateMachine::Transition:: ID of game state to transition to is empty.");
                return;
            }

            s_Instance.transition(a_strNewState, a_strSceneName, a_strLevelName);
        }
    }
}