using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class UI_ScreenDamageIndication : AbsUISingleton
    {
        /// <summary>
        /// Singleton instance
        /// </summary>
        private static UI_ScreenDamageIndication s_Instance = null;

        /// <summary>
        /// initializes, sets singleton to this
        /// </summary>
        public override void initialize()
        {
            if (s_Instance != null)
            {
                return;
            }
            s_Instance = this;

            EventManager.SubscribeTo(GAME_EVENT_TYPE.ON_PLAYER_STATE_CHANGED, onPlayerStateChanged);
        }

        /// <summary>
        /// sets singleton to null
        /// </summary>
        public override void destroy()
        {
            if (s_Instance != this)
            {
                return;
            }
            EventManager.UnsubscribeFrom(GAME_EVENT_TYPE.ON_PLAYER_STATE_CHANGED, onPlayerStateChanged);

            s_Instance = null;
        }

        /// <summary>
        /// Callback on player state changed
        /// </summary>
        /// <param name="a_EventHash"></param>
        private void onPlayerStateChanged(EventHash a_EventHash)
        {
            managePanel();
        }

        /// <summary>
        /// Manages shows/hides the panel if the player is in the game
        /// Displays the blood spatter on the helmet
        /// </summary>
        private void managePanel()
        {
            PLAYER_STATE l_CurrentPlayerState = PlayerManager.PlayerState;

            // check if the player is in the game and the player is wearing a helmet
            if (l_CurrentPlayerState == PLAYER_STATE.MENU_SELECTION ||
                l_CurrentPlayerState == PLAYER_STATE.NO_INTERACTION)
            {
                hide();
            }
            else
            { 
            
            }
        }
    }
}