using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class HomeMonitorScreenFSM : StateMachine
    {
        #region STATE NAMES
        public const string MONITOR_STATE_CONTROLS                  = "Controls";
        public const string NEW_GAME_CONFIRMATION_STATE_CONTROLS    = "NewGameConfirmation";
        public const string CONTINUE_CONFIRMATION_STATE_CONTROLS    = "ContinueConfirmation";
        #endregion STATE NAMES

        void Awake()
        {
            transitionMonitorScreen(m_strStartState);
        }

        /// <summary>
        /// Transitions to the given state of monitor screen
        /// </summary>
        /// <param name="a_strNewState"></param>
        public void transitionMonitorScreen(string a_strNewState )
        {
            transition(a_strNewState, true);
        }

        public void transitionToControlsState()
        {
            transitionMonitorScreen(MONITOR_STATE_CONTROLS);
        }
    }
}