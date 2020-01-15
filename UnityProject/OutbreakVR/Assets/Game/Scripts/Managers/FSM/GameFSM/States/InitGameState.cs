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
            initEntitlementCheck();


            //Get username and userid

            

            //Check internet connection

            //Get Game Constant data

            //Get Achievement data

            //Get player data

            // Add player initilization and platform initializations
            
        }

        /// <summary>
        /// Check for oculus entitlement
        /// On Failure of entitlement check, exit the app.
        /// </summary>
        private void initEntitlementCheck()
        {
            try
            {
#if _MASHMO_OVR_
                Oculus.Platform.Core.AsyncInitialize(SystemConsts.OCULUS_MOBILE_APP_ID);
                Oculus.Platform.Entitlements.IsUserEntitledToApplication().OnComplete(onEntitlementCheckComplete);
#endif
            }
            catch (UnityException a_Exception)
            {
                // Treat any potential initialization exceptions as an entitlement check failure.
                Debug.LogError("ServerConnectionHandler::initializeVRSystem:: Entitlement check failure. Exiting the game."+ a_Exception);
                GameManager.ExitGameOnDisplayNotification(SystemConsts.ERROR_TITLE_ENTITLEMENT_CHECK_FAILURE, SystemConsts.ERROR_MSG_ENTITLEMENT_CHECK_FAILURE);
            }
        }

        /// <summary>
        /// Oculus entitlement check is complete, now checks if it is successfull
        /// </summary>
        /// <param name="a_Msg"></param>
        private void onEntitlementCheckComplete(Oculus.Platform.Message a_Msg)
        {
            if (a_Msg.IsError)
            {
                Debug.LogError("GameState_Init::onEntitlementCheck:: Entitlement check failure. Exiting the game.");
                GameManager.ExitGameOnDisplayNotification(SystemConsts.ERROR_TITLE_ENTITLEMENT_CHECK_FAILURE, SystemConsts.ERROR_MSG_ENTITLEMENT_CHECK_FAILURE);
            }
            else
            {
                try
                {
                    Oculus.Platform.Users.GetLoggedInUser().OnComplete(onRetrievedLoggedInUser);
                    Oculus.Platform.Request.RunCallbacks();
                }
                catch (UnityException a_Exception)
                {
                    Debug.LogError("GameState_Init::onEntitlementCheck:: Failed to retrieve the logged in user with exception : " + a_Exception);
                    GameManager.ExitGameOnDisplayNotification(SystemConsts.ERROR_TITLE_LOGIN_RETRIEVAL_FAILURE, SystemConsts.ERROR_MSG_LOGIN_RETRIEVAL_FAILURE);
                }
            }
        }

        /// <summary>
        /// Callback called on retrived the oculus user ID.
        /// </summary>
        /// <param name="a_Msg"></param>
        private void onRetrievedLoggedInUser(Oculus.Platform.Message<Oculus.Platform.Models.User> a_Msg)
        {
            if (a_Msg.IsError)
            {
                Debug.LogError("GameState_Init::onRetrievedLoggedInUser:: Failed to retrieve the logged in user.");
                GameManager.ExitGameOnDisplayNotification(SystemConsts.ERROR_TITLE_LOGIN_RETRIEVAL_FAILURE, SystemConsts.ERROR_MSG_LOGIN_RETRIEVAL_FAILURE);
            }
            else
            {
                ///Set player username
                Debug.Log("<color=green> System Success </color> Username retrieved");
                PlayerDataManager.InitDataWithUsername(a_Msg.Data.OculusID);
                LevelManager.LoadLevelDataFromPlayerPrefs();

                onInitializationSuccessful();
            }
        }


        /// <summary>
        /// transitions to home state to start the game
        /// </summary>
        private void onInitializationSuccessful()
        {
            GameManager.GoToHome();
        }
    }
}