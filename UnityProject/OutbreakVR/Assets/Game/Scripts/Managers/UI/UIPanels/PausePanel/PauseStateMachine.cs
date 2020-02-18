using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class PauseStateMachine : StateMachine
    {
        /// <summary>
        /// Transition to a pause state / page panel
        /// </summary>
        /// <param name="a_strNewState"></param>
        /// <returns></returns>
        public bool transitionToPauseState(string a_strNewState)
        {
            if (!base.transition(a_strNewState))
            {
                return false;
            }

            return true;
        }
    }
}