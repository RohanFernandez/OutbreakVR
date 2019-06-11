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
        /// Player camera transform
        /// </summary>
        [SerializeField]
        private Transform m_HeadsetPlayerCamera = null;

        #region PLAYER MOVEMENT

        /// <summary>
        /// Is player movement allowed.
        /// </summary>
        private bool m_bIsMovementAllowed = true;
        public bool IsMovementAllowed
        {
            set { m_bIsMovementAllowed = value; }
            get { return m_bIsMovementAllowed; }
        }

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
            EventManager.SubscribeTo(GAME_EVENT_TYPE.ON_CONTROLLER_CHANGED, onControllerChanged);
        }

        public override void destroy()
        {
            if (s_Instance != this)
            {
                return;
            }
            EventManager.UnsubscribeFrom(GAME_EVENT_TYPE.ON_CONTROLLER_CHANGED, onControllerChanged);
            s_Instance = null;
        }

        /// <summary>
        /// Event callback on controller changed
        /// </summary>
        /// <param name="a_ControllerType"></param>
        /// <param name="a_NewControllerAnchor"></param>
        void onControllerChanged(EventHash a_Hashtable)
        {
            CONTROLLER_TYPE l_NewControllerType = (CONTROLLER_TYPE)a_Hashtable[GameEventTypeConst.ID_NEW_CONTROLLER_TYPE];
            GameObject l_goControllerAnchor = (GameObject)a_Hashtable[GameEventTypeConst.ID_NEW_CONTROLLER_ANCHOR];
        }

        void Update()
        {
            manageMovement();

            if (
#if UNITY_EDITOR
                    Input.GetKeyUp(KeyCode.P) ||
#endif
                    OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.Active))
            {
                //m_PausePanel.SetActive(!m_PausePanel.activeSelf);
            }
        }

        /// <summary>
        /// Manage input with the movement.
        /// </summary>
        void manageMovement()
        {
            if (IsMovementAllowed)
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
        }
    }
}