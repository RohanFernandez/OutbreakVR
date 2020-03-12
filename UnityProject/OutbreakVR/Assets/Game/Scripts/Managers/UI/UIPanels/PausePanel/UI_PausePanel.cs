using System.Collections;
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
        /// THe center camera as the main camera
        /// </summary>
        [SerializeField]
        private Transform m_transformCenterCamera = null;

        /// <summary>
        /// The state machine that manages the pause panel
        /// </summary>
        [SerializeField]
        private PauseStateMachine m_PauseStateMachine = null;

        [SerializeField]
        private MainPauseControlPanel m_MainPauseControlPanel = null;

        /// <summary>
        /// The distance from the camera this UI should be rendered
        /// </summary>
        [SerializeField]
        private float m_fDistanceFromCam = 0.7f;

        #region PAUSE STATE

        public const string PAUSE_STATE_OBJECTIVE       = "OBJECTIVES";
        public const string PAUSE_STATE_INSTRUCTIONS    = "INSTRUCTIONS";
        public const string PAUSE_STATE_CONTINUE        = "CONTINUE";
        public const string PAUSE_STATE_LAST_CHECKPOINT = "LAST_CHECKPOINT";
        public const string PAUSE_STATE_GO_HOME         = "GO_HOME";

        #endregion PAUSE STATE

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
            m_PauseStateMachine.initialize();

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
            m_PauseStateMachine.destroy();
            s_Instance = null;
        }

        /// <summary>
        /// Displays panel
        /// </summary>
        public static void Show()
        {
            s_Instance.m_MainPauseControlPanel.resetPanel();
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
            transform.SetPositionAndRotation(m_transformCenterCamera.position + m_transformCenterCamera.forward * m_fDistanceFromCam, m_transformCenterCamera.rotation);

            if (ControllerManager.IsPrimaryTriggerBtnUp()
#if UNITY_EDITOR
                || Input.GetKeyUp(KeyCode.Space)
#endif
                )
            {
                m_MainPauseControlPanel.onSelectPressed();
            }
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
                m_MainPauseControlPanel.onBottomPressed();
            }
            //TOP OPTION
            else if ((l_NewTouchPadBtnPressed == CONTROLLER_TOUCHPAD_BUTTON.BTN_TOP_PRESSED ||
                l_NewTouchPadBtnPressed == CONTROLLER_TOUCHPAD_BUTTON.BTN_LEFT_TOP_PRESSED ||
                l_NewTouchPadBtnPressed == CONTROLLER_TOUCHPAD_BUTTON.BTN_RIGHT_TOP_PRESSED) &&
                l_OldTouchPadBtnPressed != l_NewTouchPadBtnPressed)
            {
                
                m_MainPauseControlPanel.onTopPressed();
            }

            //LEFT OPTION
            if ((l_NewTouchPadBtnPressed == CONTROLLER_TOUCHPAD_BUTTON.BTN_LEFT_PRESSED ||
                l_NewTouchPadBtnPressed == CONTROLLER_TOUCHPAD_BUTTON.BTN_LEFT_BOTTOM_PRESSED ||
                l_NewTouchPadBtnPressed == CONTROLLER_TOUCHPAD_BUTTON.BTN_LEFT_TOP_PRESSED) &&
                l_OldTouchPadBtnPressed != l_NewTouchPadBtnPressed)
            {
                m_MainPauseControlPanel.onLeftPressed();
            }
            //RIGHT OPTION
            else if ((l_NewTouchPadBtnPressed == CONTROLLER_TOUCHPAD_BUTTON.BTN_RIGHT_PRESSED ||
                l_NewTouchPadBtnPressed == CONTROLLER_TOUCHPAD_BUTTON.BTN_RIGHT_BOTTOM_PRESSED ||
                l_NewTouchPadBtnPressed == CONTROLLER_TOUCHPAD_BUTTON.BTN_RIGHT_TOP_PRESSED) &&
                l_OldTouchPadBtnPressed != l_NewTouchPadBtnPressed)
            {
                m_MainPauseControlPanel.onRightPressed();
            }
        }

       
    }
}