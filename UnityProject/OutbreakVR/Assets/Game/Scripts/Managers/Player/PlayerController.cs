using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
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
        private System.Action m_actPlayerStateControl = null;

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
        /// The current time taken to reload the weapon
        /// </summary>
        [SerializeField]
        private float m_fCurrentReloadWaitTime = 0.0f;
        private float CurrentReloadWaitTime
        {
            get { return m_fCurrentReloadWaitTime; }
            set {
                if (m_fCurrentReloadWaitTime == 0.0f &&
                    value > 0.0f)
                {
                    m_bIsReloadInProgress = true;
                    UI_PlayerHelmet.ToggleReloadProgressBar(m_bIsReloadInProgress);
                }
                else if (m_fCurrentReloadWaitTime > 0.0f &&
                    value == 0.0f)
                {
                    m_bIsReloadInProgress = false;
                    UI_PlayerHelmet.ToggleReloadProgressBar(m_bIsReloadInProgress);
                }
                m_fCurrentReloadWaitTime = value;
            }
        }

        /// <summary>
        /// Is the reload of the current weapon in progress
        /// </summary>
        private bool m_bIsReloadInProgress = false;

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
            EventManager.SubscribeTo(GAME_EVENT_TYPE.ON_CURRENT_WEAPON_OR_CATEGORY_CHANGED, onWeaponChanged);
        }

        public override void destroy()
        {
            if (s_Instance != this)
            {
                return;
            }
            m_RegisteredGameObj.unregisterGameObject();
            EventManager.UnsubscribeFrom(GAME_EVENT_TYPE.ON_PLAYER_STATE_CHANGED, onPlayerStateChanged);
            EventManager.UnsubscribeFrom(GAME_EVENT_TYPE.ON_CURRENT_WEAPON_OR_CATEGORY_CHANGED, onWeaponChanged);
            s_Instance = null;
        }

        void Update()
        {
            if (m_actPlayerStateControl != null)
            {
                m_actPlayerStateControl();
            }
        }

        /// <summary>
        /// Manage input with the movement.
        /// </summary>
        private void managePlayerState_InGameMovement()
        {
            manageMovement();
            managerSwipeInteraction();
            manageWeaponAttack();
            manageInGamePause();
        }

        /// <summary>
        /// Manages movement of the player
        /// </summary>
        private void manageMovement()
        {
            Vector2 l_v2RemoteTouchPadPosition = ControllerManager.GetPrimaryTouchpadPosition();
            bool l_bIsMovingLastFrame = m_bIsMoving;
            m_bIsMoving = false;
            Vector3 l_v3MovementDirection = Vector3.zero;

#if !UNITY_EDITOR
        if (ControllerManager.IsPrimaryTouchpadBtnDown())
        {
#endif
            //forward Pressed, move player forward
            if (
#if UNITY_EDITOR
                    Input.GetKey(KeyCode.W) ||
#endif
                    l_v2RemoteTouchPadPosition.y > 0.5f)
            {
                l_v3MovementDirection += (m_HeadsetPlayerCamera.forward);
                m_bIsMoving = true;
            }
            //back Pressed, move player backwards
            else if (
#if UNITY_EDITOR
                    Input.GetKey(KeyCode.S) ||
#endif
                    l_v2RemoteTouchPadPosition.y < -0.5f)
            {
                l_v3MovementDirection += (-m_HeadsetPlayerCamera.forward);
                m_bIsMoving = true;
            }

            //Right Pressed, move player left
            if (
#if UNITY_EDITOR
                    Input.GetKey(KeyCode.D) ||
#endif
                    l_v2RemoteTouchPadPosition.x > 0.5f)
            {
                l_v3MovementDirection += (m_HeadsetPlayerCamera.right);
                m_bIsMoving = true;
            }
            //Left Pressed, move player left
            else if (
#if UNITY_EDITOR
                    Input.GetKey(KeyCode.A) ||
#endif
                    l_v2RemoteTouchPadPosition.x < -0.5f)
            {
                l_v3MovementDirection += (-m_HeadsetPlayerCamera.right);
                m_bIsMoving = true;
            }
#if !UNITY_EDITOR
        }
#endif
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
        /// Manages the swipe interactions
        /// </summary>
        private void managerSwipeInteraction()
        {
            Vector2 l_v2CurrentSwipe = ControllerManager.GetSwipe();

            // Swipe from left to right
            if (l_v2CurrentSwipe.x > MIN_SWIPE_VALUE
#if UNITY_EDITOR
                || Input.GetKeyUp(KeyCode.RightArrow)
#endif
                )
            {
                WeaponManager.SetNextCategory();
            }
            // Swipe from right to left
            else if (l_v2CurrentSwipe.x < -MIN_SWIPE_VALUE
#if UNITY_EDITOR
                || Input.GetKeyUp(KeyCode.LeftArrow)
#endif
                )
            {
                WeaponManager.SetPreviousCategory();
            }
            // Swipe from top to bottom
            else if (l_v2CurrentSwipe.y < -MIN_SWIPE_VALUE
#if UNITY_EDITOR
                || Input.GetKeyUp(KeyCode.DownArrow)
#endif
                )
            {
                IPointerOver l_IPointerOver = ControllerManager.GetPointerOverObject();
                if (l_IPointerOver != null)
                {
                    l_IPointerOver.onPointerInteract();
                }
                
            }
            // Swipe from bottom to top
            else if (l_v2CurrentSwipe.y > MIN_SWIPE_VALUE
#if UNITY_EDITOR
                || Input.GetKeyUp(KeyCode.UpArrow)
#endif
                )
            {
                
            }
        }

        /// <summary>
        /// Fires/Reloads the weapon
        /// </summary>
        public void manageWeaponAttack()
        {
            ///Fire weapon
            if (ControllerManager.IsPrimaryTriggerBtnDown()
#if UNITY_EDITOR
                || Input.GetKey(KeyCode.Mouse0)
#endif
                )
            {
                if (WeaponManager.CanCurrentWeaponBeFired() &&
                    !m_bIsReloadInProgress)
                {
                    WeaponManager.FireWeapon();
                }
                else
                {
                    /// Indicate weapon cannot be fired
                }
            }

            ///Reload weapon
            float l_fDotFacingDown = Vector3.Dot(ControllerManager.GetPrimaryControllerDirection(), Vector3.down);
            if (WeaponManager.CanCurrentWeaponBeReloaded())
            {
                if (
                    (l_fDotFacingDown > 0.85f)
                )
                {
                    CurrentReloadWaitTime += Time.deltaTime;
                    float l_fCurrentWeaponReloadTime = WeaponManager.getCurrentWeaponReloadTime();
                    UI_PlayerHelmet.UpdateReloadProgressBar(CurrentReloadWaitTime, l_fCurrentWeaponReloadTime);

                    if (CurrentReloadWaitTime > l_fCurrentWeaponReloadTime)
                    {
                        CurrentReloadWaitTime = 0.0f;
                        WeaponManager.ReloadWeapon();
                    }
                }
                else
                {
                    CurrentReloadWaitTime = 0.0f;
                }
            }
            else
            {
                CurrentReloadWaitTime = 0.0f;
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
                GameManager.PauseGame(true);
            }
        }

        /// <summary>
        /// Event called on player state changed
        /// </summary>
        /// <param name="a_EventHash"></param>
        public void onPlayerStateChanged(EventHash a_EventHash)
        {
            CurrentReloadWaitTime = 0.0f;

            PLAYER_STATE l_NewPlayerState = (PLAYER_STATE)a_EventHash[GameEventTypeConst.ID_NEW_PLAYER_STATE];

            switch (l_NewPlayerState)
            {
                case PLAYER_STATE.IN_GAME_HALTED:
                    {
                        m_actPlayerStateControl = null;
                        ControllerManager.ToggleLaser(true);
                        WeaponManager.IsWeaponActive = true;
                        UI_PlayerHelmet.Show();
                        m_CustomPointer.setPointerAsCrosshair(true);
                        break;
                    }
                case PLAYER_STATE.IN_GAME_MOVEMENT:
                    {
                        m_actPlayerStateControl = managePlayerState_InGameMovement;
                        ControllerManager.ToggleLaser(true);
                        WeaponManager.IsWeaponActive = true;
                        UI_PlayerHelmet.Show();
                        m_CustomPointer.setPointerAsCrosshair(true);
                        break;
                    }
                case PLAYER_STATE.MENU_SELECTION:
                    {
                        m_actPlayerStateControl = null;
                        ControllerManager.ToggleLaser(true);
                        WeaponManager.IsWeaponActive = false;
                        UI_PlayerHelmet.Hide();
                        m_CustomPointer.setPointerAsCrosshair(false);
                        break;
                    }
                case PLAYER_STATE.NO_INTERACTION:
                    {
                        m_actPlayerStateControl = null;
                        ControllerManager.ToggleLaser(false);
                        WeaponManager.IsWeaponActive = false;
                        UI_PlayerHelmet.Hide();
                        break;
                    }
                default:
                    {
                        m_actPlayerStateControl = null;
                        break;
                    }
            }
        }

        /// <summary>
        /// Event called on weapon changed
        /// </summary>
        /// <param name="a_EventHash"></param>
        public void onWeaponChanged(EventHash a_EventHash)
        {
            CurrentReloadWaitTime = 0.0f;
        }
    }
}