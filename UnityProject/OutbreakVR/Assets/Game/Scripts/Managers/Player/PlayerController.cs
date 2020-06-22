using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public enum CONTROLLER_TOUCHPAD_SWIPE
    { 
        LEFT_TO_RIGHT   =   0,
        RIGHT_TO_LEFT   =   1,
        BOTTOM_TO_TOP   =   2,
        TOP_TO_BOTTOM   =   3,
        NO_SWIPE        =   4,
    }

    public enum CONTROLLER_TOUCHPAD_BUTTON
    {
        BTN_RIGHT_PRESSED           =   0,
        BTN_LEFT_PRESSED            =   1,
        BTN_TOP_PRESSED             =   2,
        BTN_BOTTOM_PRESSED          =   3,
        BTN_RIGHT_TOP_PRESSED       =   5,
        BTN_LEFT_TOP_PRESSED        =   6,
        BTN_RIGHT_BOTTOM_PRESSED    =   7,
        BTN_LEFT_BOTTOM_PRESSED     =   8,
        BTN_NOT_PRESSED             =   9,
    }

    public class PlayerController : AbsComponentHandler
    {
        /// <summary>
        /// Singleton instance
        /// </summary>
        private static PlayerController s_Instance = null;

        /// <summary>
        /// The character controller components of the player.
        /// </summary>
        [SerializeField]
        private CharacterController m_CharacterController = null;
        public CharacterController CharacterController
        {
            get{ return m_CharacterController;}
        }

        /// <summary>
        /// The pointer to controll the crosshair/ UI pointer
        /// </summary>
        [SerializeField]
        private CustomPointer m_CustomPointer = null;

        /// <summary>
        /// Player camera transform
        /// </summary>
        [SerializeField]
        private Transform m_HeadsetPlayerCamera = null;

        /// <summary>
        /// The controls to manage the player depending on PLAYER STATE
        /// </summary>
        private System.Action<CONTROLLER_TOUCHPAD_SWIPE, CONTROLLER_TOUCHPAD_BUTTON> m_actPlayerStateControl = null;

        /// <summary>
        /// Component of player parent to manage registration to GameObjectManager
        /// </summary>
        [SerializeField]
        private RegisteredGameObject m_RegisteredParentGameObj = null;

        /// <summary>
        /// Component to manage registration to GameObjectManager
        /// </summary>
        [SerializeField]
        private RegisteredGameObject m_RegisteredGameObj = null;

        /// <summary>
        /// The last updated pressed value
        /// </summary>
        private CONTROLLER_TOUCHPAD_BUTTON m_TrackpadPadButton = CONTROLLER_TOUCHPAD_BUTTON.BTN_NOT_PRESSED;

        /// <summary>
        /// The last updated swiped value
        /// </summary>
        private CONTROLLER_TOUCHPAD_SWIPE m_TrackpadPadSwipe = CONTROLLER_TOUCHPAD_SWIPE.NO_SWIPE;

        #region SWIPE

        /// <summary>
        /// Min swipe per dimension to register as swipe direction,
        /// else will register as none
        /// </summary>
        private const float MIN_SWIPE_VALUE = 0.7f;

        #endregion SWIPE

        #region PLAYER MOVEMENT

        /// <summary>
        /// The speed of movement of the character.
        /// </summary>
        [SerializeField]
        private float m_fMovementSpeedMax = 4.0f;

        /// <summary>
        /// The current player movement speed
        /// </summary>
        [SerializeField]
        private float m_fCurrentMovementSpeed = 0.0f;

        /// <summary>
        /// The acceleration of the player
        /// </summary>
        [SerializeField]
        private float m_fMovementAcceleraion = 1.0f;

        /// <summary>
        /// The deceleration of the player
        /// </summary>
        [SerializeField]
        private float m_fMovementDeceleraion = 1.0f;

        /// <summary>
        /// The current player movement direction.
        /// </summary>
        [SerializeField]
        Vector3 m_v3MovementVelocity = Vector3.zero;

        [SerializeField]
        Vector3 m_v3GravityModifier = Vector3.zero;

        [SerializeField]
        private bool m_bIsMoving = false;

        public float MovementSpeedMax
        {
            set { m_fMovementSpeedMax = value; }
            get { return m_fMovementSpeedMax; }
        }

        #endregion PLAYER MOVEMENT

        public override void initialize()
        {
            if (s_Instance != null)
            {
                return;
            }
            s_Instance = this;

            m_RegisteredParentGameObj.registerGameObject();
            m_RegisteredGameObj.registerGameObject();
            
            EventManager.SubscribeTo(GAME_EVENT_TYPE.ON_PLAYER_STATE_CHANGED, onPlayerStateChanged);
        }

        public override void destroy()
        {
            if (s_Instance != this)
            {
                return;
            }
            m_RegisteredGameObj.unregisterGameObject();
            EventManager.UnsubscribeFrom(GAME_EVENT_TYPE.ON_PLAYER_STATE_CHANGED, onPlayerStateChanged);
            s_Instance = null;
        }

        void Update()
        {
            CONTROLLER_TOUCHPAD_BUTTON l_LastTrackpadPadButton = m_TrackpadPadButton;
            CONTROLLER_TOUCHPAD_SWIPE l_LastTrackpadPadSwipe = m_TrackpadPadSwipe;
            m_TrackpadPadButton = getTrackPadPress();
            m_TrackpadPadSwipe = getTrackPadSwipe();

            if (m_TrackpadPadButton != l_LastTrackpadPadButton)
            {
                EventHash l_EventHash = EventManager.GetEventHashtable();
                l_EventHash.Add(GameEventTypeConst.ID_NEW_TOUCHPAD_BTN_PRESSED, m_TrackpadPadButton);
                l_EventHash.Add(GameEventTypeConst.ID_OLD_TOUCHPAD_BTN_PRESSED, l_LastTrackpadPadButton);
                EventManager.Dispatch(GAME_EVENT_TYPE.ON_TOUCHPAD_BTN_CHANGED, l_EventHash);
            }

            if (m_TrackpadPadSwipe != CONTROLLER_TOUCHPAD_SWIPE.NO_SWIPE)
            {
                EventHash l_EventHash = EventManager.GetEventHashtable();
                l_EventHash.Add(GameEventTypeConst.ID_TOUCHPAD_SWIPE, m_TrackpadPadSwipe);
                EventManager.Dispatch(GAME_EVENT_TYPE.ON_TOUCHPAD_SWIPE, l_EventHash);
            }

            if (m_actPlayerStateControl != null)
            {
                m_actPlayerStateControl(m_TrackpadPadSwipe, m_TrackpadPadButton);
            }
        }

        /// <summary>
        /// Dont allow any interaction from the player but only allow the gravity to be implemented on the player
        /// </summary>
        private void managePlayerState_paralysed(CONTROLLER_TOUCHPAD_SWIPE a_TouchPadSwipe, CONTROLLER_TOUCHPAD_BUTTON a_TouchPadButton)
        {
            m_CharacterController.Move(m_v3GravityModifier);
        }

        /// <summary>
        /// Manage input with the movement.
        /// </summary>
        private void managePlayerState_InGameMovement(CONTROLLER_TOUCHPAD_SWIPE a_TouchPadSwipe, CONTROLLER_TOUCHPAD_BUTTON a_TouchPadButton)
        {
            manageMovement(a_TouchPadButton);
            managerSwipeInteraction_InGameMovement(a_TouchPadSwipe);
            manageInGamePause();
        }

        /// <summary>
        /// Manage input with the movement.
        /// </summary>
        private void managePlayerState_MenuSelection(CONTROLLER_TOUCHPAD_SWIPE a_TouchPadSwipe, CONTROLLER_TOUCHPAD_BUTTON a_TouchPadButton)
        {
            managerSwipeInteraction_MenuSelection(a_TouchPadSwipe);
        }

        /// <summary>
        /// Manages movement of the player
        /// </summary>
        private void manageMovement(CONTROLLER_TOUCHPAD_BUTTON a_TouchPadButton)
        {
            bool l_bIsMovingLastFrame = m_bIsMoving;
            m_bIsMoving = false;
            Vector3 l_v3MovementDirection = Vector3.zero;


            //forward Pressed, move player forward
            if (a_TouchPadButton == CONTROLLER_TOUCHPAD_BUTTON.BTN_TOP_PRESSED ||
                a_TouchPadButton == CONTROLLER_TOUCHPAD_BUTTON.BTN_LEFT_TOP_PRESSED ||
                a_TouchPadButton == CONTROLLER_TOUCHPAD_BUTTON.BTN_RIGHT_TOP_PRESSED)
            {
                l_v3MovementDirection += (m_HeadsetPlayerCamera.forward);
                m_bIsMoving = true;
            }
            //back Pressed, move player backwards
            else if (a_TouchPadButton == CONTROLLER_TOUCHPAD_BUTTON.BTN_BOTTOM_PRESSED ||
                a_TouchPadButton == CONTROLLER_TOUCHPAD_BUTTON.BTN_LEFT_BOTTOM_PRESSED ||
                a_TouchPadButton == CONTROLLER_TOUCHPAD_BUTTON.BTN_RIGHT_BOTTOM_PRESSED)
            {
                l_v3MovementDirection += (-m_HeadsetPlayerCamera.forward);
                m_bIsMoving = true;
            }

            //Right Pressed, move player left
            if (a_TouchPadButton == CONTROLLER_TOUCHPAD_BUTTON.BTN_RIGHT_PRESSED ||
                a_TouchPadButton == CONTROLLER_TOUCHPAD_BUTTON.BTN_RIGHT_BOTTOM_PRESSED ||
                a_TouchPadButton == CONTROLLER_TOUCHPAD_BUTTON.BTN_RIGHT_TOP_PRESSED)
            {
                l_v3MovementDirection += (m_HeadsetPlayerCamera.right);
                m_bIsMoving = true;
            }
            //Left Pressed, move player left
            else if (a_TouchPadButton == CONTROLLER_TOUCHPAD_BUTTON.BTN_LEFT_PRESSED ||
                a_TouchPadButton == CONTROLLER_TOUCHPAD_BUTTON.BTN_LEFT_BOTTOM_PRESSED ||
                a_TouchPadButton == CONTROLLER_TOUCHPAD_BUTTON.BTN_LEFT_TOP_PRESSED)
            {
                l_v3MovementDirection += (-m_HeadsetPlayerCamera.right);
                m_bIsMoving = true;
            }

            if (m_bIsMoving)
            {
                if (m_bIsMoving != l_bIsMovingLastFrame)
                {
                    SoundManager.PlayAudio(SoundConst.AUD_SRC_PLAYER_FOOTSTEPS, SoundConst.AUD_CLIP_PLAYER_CONCRETE_FOOTSTEPS, true, 1.0f, AUDIO_SRC_TYPES.AUD_SRC_SFX);
                }

                m_fCurrentMovementSpeed += m_fMovementAcceleraion * Time.deltaTime;
                if (m_fCurrentMovementSpeed > m_fMovementSpeedMax)
                {
                    m_fCurrentMovementSpeed = m_fMovementSpeedMax;
                }
            }
            else
            {
                if (m_fCurrentMovementSpeed != 0.0f)
                {
                    m_fCurrentMovementSpeed -= m_fMovementDeceleraion * Time.deltaTime;
                }

                if (m_fCurrentMovementSpeed < 0.0f)
                {
                    SoundManager.StopAudioSrcWithID(SoundConst.AUD_SRC_PLAYER_FOOTSTEPS);
                    m_fCurrentMovementSpeed = 0.0f;
                }
            }

            m_v3MovementVelocity = Vector3.RotateTowards(m_v3MovementVelocity.normalized, (m_v3MovementVelocity.normalized + l_v3MovementDirection.normalized).normalized, Mathf.PI * Time.deltaTime, Mathf.PI) * m_fCurrentMovementSpeed * Time.deltaTime;
            m_CharacterController.Move(m_v3MovementVelocity + (m_v3GravityModifier * Time.deltaTime));
        }

        /// <summary>
        /// Manages trackpad press
        /// </summary>
        private CONTROLLER_TOUCHPAD_BUTTON getTrackPadPress()
        {
            Vector2 l_v2RemoteTouchPadPosition = ControllerManager.GetPrimaryTouchpadPosition();
            CONTROLLER_TOUCHPAD_BUTTON l_TouchPadBtnPressed = CONTROLLER_TOUCHPAD_BUTTON.BTN_NOT_PRESSED;

#if !UNITY_EDITOR
        if (ControllerManager.IsPrimaryTouchpadBtnDown())
        {
#endif
            //forward Pressed
            if (
#if UNITY_EDITOR
                    Input.GetKey(KeyCode.W) ||
#endif
                    l_v2RemoteTouchPadPosition.y > 0.5f)
            {
                l_TouchPadBtnPressed = CONTROLLER_TOUCHPAD_BUTTON.BTN_TOP_PRESSED;
            }
            //back Pressed
            else if (
#if UNITY_EDITOR
                    Input.GetKey(KeyCode.S) ||
#endif
                    l_v2RemoteTouchPadPosition.y < -0.5f)
            {
                l_TouchPadBtnPressed = CONTROLLER_TOUCHPAD_BUTTON.BTN_BOTTOM_PRESSED;
            }

            //Right Pressed
            if (
#if UNITY_EDITOR
                    Input.GetKey(KeyCode.D) ||
#endif
                    l_v2RemoteTouchPadPosition.x > 0.5f)
            {
                if (l_TouchPadBtnPressed == CONTROLLER_TOUCHPAD_BUTTON.BTN_TOP_PRESSED)
                {
                    l_TouchPadBtnPressed = CONTROLLER_TOUCHPAD_BUTTON.BTN_RIGHT_TOP_PRESSED;
                }
                else if (l_TouchPadBtnPressed == CONTROLLER_TOUCHPAD_BUTTON.BTN_RIGHT_BOTTOM_PRESSED)
                {
                    l_TouchPadBtnPressed = CONTROLLER_TOUCHPAD_BUTTON.BTN_RIGHT_BOTTOM_PRESSED;
                }
                else
                {
                    l_TouchPadBtnPressed = CONTROLLER_TOUCHPAD_BUTTON.BTN_RIGHT_PRESSED;
                }
            }
            //Left Pressed
            else if (
#if UNITY_EDITOR
                    Input.GetKey(KeyCode.A) ||
#endif
                    l_v2RemoteTouchPadPosition.x < -0.5f)
            {
                if (l_TouchPadBtnPressed == CONTROLLER_TOUCHPAD_BUTTON.BTN_TOP_PRESSED)
                {
                    l_TouchPadBtnPressed = CONTROLLER_TOUCHPAD_BUTTON.BTN_LEFT_TOP_PRESSED;
                }
                else if (l_TouchPadBtnPressed == CONTROLLER_TOUCHPAD_BUTTON.BTN_RIGHT_BOTTOM_PRESSED)
                {
                    l_TouchPadBtnPressed = CONTROLLER_TOUCHPAD_BUTTON.BTN_LEFT_BOTTOM_PRESSED;
                }
                else
                {
                    l_TouchPadBtnPressed = CONTROLLER_TOUCHPAD_BUTTON.BTN_LEFT_PRESSED;
                }
            }
#if !UNITY_EDITOR
        }
#endif
            return l_TouchPadBtnPressed;
        }

        /// <summary>
        /// returns the swipe interactions
        /// </summary>
        private CONTROLLER_TOUCHPAD_SWIPE getTrackPadSwipe()
        {
            Vector2 l_v2CurrentSwipe = ControllerManager.GetSwipe();
            CONTROLLER_TOUCHPAD_SWIPE l_TouchPadSwipe = CONTROLLER_TOUCHPAD_SWIPE.NO_SWIPE;

            // Swipe from left to right
            if (l_v2CurrentSwipe.x > MIN_SWIPE_VALUE
#if UNITY_EDITOR
                || Input.GetKeyUp(KeyCode.RightArrow)
#endif
                )
            {
                l_TouchPadSwipe = CONTROLLER_TOUCHPAD_SWIPE.LEFT_TO_RIGHT;
            }
            // Swipe from right to left
            else if (l_v2CurrentSwipe.x < -MIN_SWIPE_VALUE
#if UNITY_EDITOR
                || Input.GetKeyUp(KeyCode.LeftArrow)
#endif
                )
            {
                l_TouchPadSwipe = CONTROLLER_TOUCHPAD_SWIPE.RIGHT_TO_LEFT;
            }
            // Swipe from top to bottom
            else if (l_v2CurrentSwipe.y < -MIN_SWIPE_VALUE
#if UNITY_EDITOR
                || Input.GetKeyUp(KeyCode.DownArrow)
#endif
                )
            {
                l_TouchPadSwipe = CONTROLLER_TOUCHPAD_SWIPE.TOP_TO_BOTTOM;
            }
            // Swipe from bottom to top
            else if (l_v2CurrentSwipe.y > MIN_SWIPE_VALUE
#if UNITY_EDITOR
                || Input.GetKeyUp(KeyCode.UpArrow)
#endif
                )
            {
                l_TouchPadSwipe = CONTROLLER_TOUCHPAD_SWIPE.BOTTOM_TO_TOP;
            }

            return l_TouchPadSwipe;
        }

        /// <summary>
        /// Manages the swipe interactions
        /// </summary>
        private void managerSwipeInteraction_InGameMovement(CONTROLLER_TOUCHPAD_SWIPE a_TouchpadSwipe)
        {
            Vector2 l_v2CurrentSwipe = ControllerManager.GetSwipe();

            // Swipe from left to right
            if (a_TouchpadSwipe == CONTROLLER_TOUCHPAD_SWIPE.LEFT_TO_RIGHT)
            {
                WeaponManager.SetNextCategory();
            }
            // Swipe from right to left
            else if (a_TouchpadSwipe == CONTROLLER_TOUCHPAD_SWIPE.RIGHT_TO_LEFT)
            {
                WeaponManager.SetPreviousCategory();
            }
            // Swipe from top to bottom
            else if (a_TouchpadSwipe == CONTROLLER_TOUCHPAD_SWIPE.TOP_TO_BOTTOM)
            {
                IPointerOver l_IPointerOver = ControllerManager.GetPointerOverObject();
                if (l_IPointerOver != null)
                {
                    l_IPointerOver.onPointerInteract();
                }
                
            }
            // Swipe from bottom to top
            else if (a_TouchpadSwipe == CONTROLLER_TOUCHPAD_SWIPE.BOTTOM_TO_TOP)
            {
                
            }
        }

        /// <summary>
        /// Manages the swipe interactions
        /// </summary>
        private void managerSwipeInteraction_MenuSelection(CONTROLLER_TOUCHPAD_SWIPE a_TouchpadSwipe)
        {
            if (ControllerManager.IsPrimaryTriggerBtnDownThisFrame()
#if UNITY_EDITOR
                ||
                Input.GetKeyDown(KeyCode.Space)
#endif                
                )
            {
                IPointerOver l_IPointerOver = ControllerManager.GetPointerOverObject();
                if (l_IPointerOver != null)
                {
                    l_IPointerOver.onPointerInteract();
                }
            }
        }

        /// <summary>
        /// Manages back button pressed while in game
        /// </summary>
        private void manageInGamePause()
        {
            if (
#if UNITY_EDITOR
                Input.GetKeyUp(KeyCode.P) ||
#endif
                ControllerManager.IsBackBtnUp())
            {
                GameManager.PauseGame(true, false);
            }
        }

        /// <summary>
        /// Event called on player state changed
        /// </summary>
        /// <param name="a_EventHash"></param>
        public void onPlayerStateChanged(EventHash a_EventHash)
        {
            PLAYER_STATE l_NewPlayerState = (PLAYER_STATE)a_EventHash[GameEventTypeConst.ID_NEW_PLAYER_STATE];

            switch (l_NewPlayerState)
            {
                case PLAYER_STATE.IN_GAME_HALTED:
                    {
                        m_actPlayerStateControl = null;
                        ControllerManager.ToggleLaser(true);
                        m_CustomPointer.setPointerAsCrosshair(true);
                        SoundManager.StopAudioSrcWithID(SoundConst.AUD_SRC_PLAYER_FOOTSTEPS);
                        break;
                    }
                case PLAYER_STATE.IN_GAME_MOVEMENT:
                    {
                        m_actPlayerStateControl = managePlayerState_InGameMovement;
                        ControllerManager.ToggleLaser(true);
                        m_CustomPointer.setPointerAsCrosshair(true);
                        break;
                    }
                case PLAYER_STATE.IN_GAME_PARALYSED:
                    {
                        m_actPlayerStateControl = managePlayerState_paralysed;
                        ControllerManager.ToggleLaser(false);
                        m_CustomPointer.setPointerAsCrosshair(false);
                        SoundManager.StopAudioSrcWithID(SoundConst.AUD_SRC_PLAYER_FOOTSTEPS);
                        break;
                    }
                case PLAYER_STATE.MENU_SELECTION:
                    {
                        m_actPlayerStateControl = managePlayerState_MenuSelection;
                        ControllerManager.ToggleLaser(true);
                        m_CustomPointer.setPointerAsCrosshair(false);
                        SoundManager.StopAudioSrcWithID(SoundConst.AUD_SRC_PLAYER_FOOTSTEPS);
                        break;
                    }
                case PLAYER_STATE.NO_INTERACTION:
                    {
                        m_actPlayerStateControl = null;
                        ControllerManager.ToggleLaser(false);
                        SoundManager.StopAudioSrcWithID(SoundConst.AUD_SRC_PLAYER_FOOTSTEPS);
                        break;
                    }
                case PLAYER_STATE.IN_GAME_PAUSED:
                    {
                        m_actPlayerStateControl = null;
                        ControllerManager.ToggleLaser(false);
                        SoundManager.StopAudioSrcWithID(SoundConst.AUD_SRC_PLAYER_FOOTSTEPS);
                        break;
                    }
                default:
                    {
                        m_actPlayerStateControl = null;
                        break;
                    }
            }
        }
    }
}