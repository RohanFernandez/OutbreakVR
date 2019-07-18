﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class UI_PausePanel : AbsUISingleton
    {
        /// <summary>
        /// singleton instance
        /// </summary>
        private static UI_PausePanel s_Instance = null;

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
            s_Instance = null;
        }

        /// <summary>
        /// Displays panel
        /// </summary>
        public static void Show()
        {
            s_Instance.show();
        }

        /// <summary>
        /// Hides panel
        /// </summary>
        public static void Hide()
        {
            s_Instance.hide();
        }

        private void Update()
        {
            Transform m_transHeadsetAnchor = ControllerManager.GetHeadsetAnchor().transform;
            Vector3 l_v3Forward = m_transHeadsetAnchor.forward;

            if (Vector3.Dot(l_v3Forward, Vector3.down) > 0.75f)
            {
                return;
            }

            l_v3Forward.y = 0.0f;
            l_v3Forward.Normalize();

            transform.position = PlayerManager.GetPosition() + (l_v3Forward * 0.5f); ;
            transform.LookAt(m_transHeadsetAnchor);
        }

        /// <summary>
        /// On button clicked return to game
        /// </summary>
        public void onBtnClicked_ReturnToGame()
        {
            GameManager.PauseGame(false);
        }
    }
}