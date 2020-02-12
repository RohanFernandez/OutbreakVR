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
        /// Objective panel that holds the level objectives
        /// </summary>
        [SerializeField]
        private UI_LevelObjectivePanel m_ObjectivePanel = null;

        /// <summary>
        /// The list of options in the pause panel
        /// </summary>
        [SerializeField]
        private List<UnityEngine.UI.Button> m_lstBtnOptions = null;

        /// <summary>
        /// The current selected option in the button option panel
        /// </summary>
        private int m_iCurrentSelectedBtnOption = 0;

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

            EventManager.SubscribeTo(GAME_EVENT_TYPE.ON_TOUCHPAD_BTN_CHANGED, onTouchpadBtnChanged);
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

            EventManager.UnsubscribeFrom(GAME_EVENT_TYPE.ON_TOUCHPAD_BTN_CHANGED, onTouchpadBtnChanged);
            s_Instance = null;
        }

        /// <summary>
        /// Displays panel
        /// </summary>
        public static void Show(ObjectiveGroupBase a_CurrentLevelObjectiveGroup)
        {
            if (a_CurrentLevelObjectiveGroup != null)
            {
                s_Instance.m_ObjectivePanel.gameObject.SetActive(true);
                s_Instance.m_ObjectivePanel.refreshObjectives(a_CurrentLevelObjectiveGroup);
            }
            else 
            {
                s_Instance.m_ObjectivePanel.gameObject.SetActive(false);
            }
            s_Instance.showButtonOptionAsHovered(0);
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
            Transform l_HeadsetTransform = ControllerManager.GetHeadsetAnchor().transform;
            Vector3 l_v3HeadsetForward = new Vector3(l_HeadsetTransform.forward.x, 0.0f, l_HeadsetTransform.forward.z).normalized;

            transform.position = l_HeadsetTransform.position + (l_v3HeadsetForward * 0.75f);
            transform.LookAt(l_HeadsetTransform);

            if (ControllerManager.IsPrimaryTriggerBtnDown()
#if UNITY_EDITOR
                || Input.GetKeyUp(KeyCode.Space)
#endif
                )
            {
                selectBtnWithCurrentIndex();
            }
        }

        /// <summary>
        /// On button clicked return to game
        /// </summary>
        public void onBtnClicked_ReturnToGame()
        {
            GameManager.PauseGame(false, false);
        }

        /// <summary>
        /// On button clicked go to home
        /// </summary>
        public void onBtnClicked_GoToHome()
        {
            GameManager.GoToHome();
        }

        /// <summary>
        /// On button clicked go to home
        /// </summary>
        public void onBtnClicked_GoToLastCheckpoint()
        {
            GameManager.RestartLevel();
        }

        /// <summary>
        /// Callback called on event on touchpad changed
        /// </summary>
        /// <param name="a_EventHash"></param>
        private void onTouchpadBtnChanged(EventHash a_EventHash)
        {
            if (!gameObject.activeSelf) { return; }
            CONTROLLER_TOUCHPAD_BUTTON l_NewTouchPadBtnPressed = (CONTROLLER_TOUCHPAD_BUTTON)a_EventHash[GameEventTypeConst.ID_NEW_TOUCHPAD_BTN_PRESSED];
            CONTROLLER_TOUCHPAD_BUTTON l_OldTouchPadBtnPressed = (CONTROLLER_TOUCHPAD_BUTTON)a_EventHash[GameEventTypeConst.ID_OLD_TOUCHPAD_BTN_PRESSED];

            //BOTTOM OPTION
            if ((l_NewTouchPadBtnPressed == CONTROLLER_TOUCHPAD_BUTTON.BTN_BOTTOM_PRESSED ||
                l_NewTouchPadBtnPressed == CONTROLLER_TOUCHPAD_BUTTON.BTN_LEFT_BOTTOM_PRESSED ||
                l_NewTouchPadBtnPressed == CONTROLLER_TOUCHPAD_BUTTON.BTN_RIGHT_BOTTOM_PRESSED) &&
                l_OldTouchPadBtnPressed != l_NewTouchPadBtnPressed)
            {
                showButtonOptionAsHovered((m_iCurrentSelectedBtnOption + 1) % m_lstBtnOptions.Count);
            }
            //TOP OPTION
            else if ((l_NewTouchPadBtnPressed == CONTROLLER_TOUCHPAD_BUTTON.BTN_TOP_PRESSED ||
                l_NewTouchPadBtnPressed == CONTROLLER_TOUCHPAD_BUTTON.BTN_LEFT_TOP_PRESSED ||
                l_NewTouchPadBtnPressed == CONTROLLER_TOUCHPAD_BUTTON.BTN_RIGHT_TOP_PRESSED) &&
                l_OldTouchPadBtnPressed != l_NewTouchPadBtnPressed)
            {
                int l_iNewIndex = m_iCurrentSelectedBtnOption - 1;
                if (l_iNewIndex == -1) { l_iNewIndex = m_lstBtnOptions.Count - 1; }

                showButtonOptionAsHovered(l_iNewIndex);
            }
        }

        /// <summary>
        /// Sets the option as hovered
        /// </summary>
        /// <param name="a_iOptionIndex"></param>
        private void showButtonOptionAsHovered(int a_iOptionIndex)
        {
            m_iCurrentSelectedBtnOption = a_iOptionIndex;

            ///Set all buttons as idle
            int l_iBtnCount = m_lstBtnOptions.Count;
            for (int l_iBtnIndex = 0; l_iBtnIndex < l_iBtnCount; l_iBtnIndex++)
            {
                UnityEngine.UI.Button l_btnCurrentBtn = m_lstBtnOptions[l_iBtnIndex];
                l_btnCurrentBtn.image.sprite = null;
            }

            m_lstBtnOptions[a_iOptionIndex].image.sprite = m_lstBtnOptions[a_iOptionIndex].spriteState.highlightedSprite;
        }

        private void selectBtnWithCurrentIndex()
        {
            m_lstBtnOptions[m_iCurrentSelectedBtnOption].onClick.Invoke(); ;
        }
    }
}