using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class InitGameState : ManagedState
    {
        public override void onStateEnter(string a_strNewState)
        {
            base.onStateEnter(a_strNewState);

            initializeGame();
        }

        public override void onStateExit(string a_strOldState)
        {
            base.onStateExit(a_strOldState);
        }

        /// <summary>
        /// Initializes all aspects of the game 
        /// entitlement check, user data
        /// </summary>
        private void initializeGame()
        {
            //Entitlement check

            //Get username and userid

            //Set player data with username

            //Check internet connection

            //Get Game Constant data

            //Get Achievement data

            //Get player data

            // Add player initilization and platform initializations
            onInitializationSuccessful();
        }

        /// <summary>
        /// transitions to home state to start the game
        /// </summary>
        private void onInitializationSuccessful()
        {
            GameStateMachine.Transition(GameConsts.STATE_NAME_HOME);
        }
    }
}