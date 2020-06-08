using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ns_Mashmo
{
    public class UI_ScreenDamageIndication : AbsUISingleton
    {
        /// <summary>
        /// Singleton instance
        /// </summary>
        private static UI_ScreenDamageIndication s_Instance = null;

        /// <summary>
        /// the image that holds the blood splatter
        /// </summary>
        [SerializeField]
        private Image m_imgBloodSplatter = null;

        [SerializeField]
        private Color m_colMaxHealthHigh;

        [SerializeField]
        private float m_fHealthAtHigh = 60.0f;

        [SerializeField]
        private Color m_colMaxHealthMid;

        [SerializeField]
        private float m_fHealthAtMid = 40.0f;

        [SerializeField]
        private Color m_colMaxHealthLow;

        [SerializeField]
        private float m_fHealthAtLow = 10.0f;

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
                show();
            }
        }

        void Update()
        {
            int l_iHealth = PlayerManager.HealthMeter;

            //Low alpha
            if ((l_iHealth < m_fHealthAtHigh) && (l_iHealth > m_fHealthAtMid))
            {
                m_imgBloodSplatter.color = Color.Lerp(m_colMaxHealthMid, m_colMaxHealthHigh, Mathf.Sin(Time.time));
            }
            //mid alpha
            else if ((l_iHealth < m_fHealthAtMid) && (l_iHealth > m_fHealthAtLow))
            {
                m_imgBloodSplatter.color = Color.Lerp(m_colMaxHealthLow, m_colMaxHealthMid, Mathf.Sin(Time.time));
            }
            //high alpha
            else if ((l_iHealth < m_fHealthAtLow) && (l_iHealth > 0))
            {
                m_imgBloodSplatter.color = Color.Lerp(Color.red, m_colMaxHealthLow, Mathf.Sin(Time.time));
            }
            else if (l_iHealth == 0)
            {
                m_imgBloodSplatter.color = Color.red;
            }
            else
            {
                m_imgBloodSplatter.color = Color.clear;
            }
        }
    }
}