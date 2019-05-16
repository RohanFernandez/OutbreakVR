﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ns_Mashmo
{
    /// <summary>
    /// The type of the controller 
    /// </summary>
    public enum CONTROLLER_TYPE
    {
        CONTROLLER_NONE,
        CONTROLLER_HEADSET,
        CONTROLLER_LEFT_REMOTE,
        CONTROLLER_RIGHT_REMOTE
    }

    public class ControllerManager : AbsComponentHandler
    {
        /// <summary>
        /// singleton instance
        /// </summary>
        private static ControllerManager s_Instance = null;

#if _MASHMO_OVR_
        [SerializeField]
        private UnityEngine.EventSystems.OVRInputModule m_OVRInputModule = null;
#endif

        /// <summary>
        /// Initialize singleton from Player
        /// </summary>
        public override void initialize()
        {
            if (s_Instance != null)
            {
                return;
            }
            s_Instance = this;
            OVRManager.HMDMounted += onHMDFound;
            OVRManager.HMDUnmounted += onHMDLost;

            // Setup controller to controller-anchor-gameobject.
            m_DictControllerSets = new Dictionary<CONTROLLER_TYPE, GameObject>
            {
                { CONTROLLER_TYPE.CONTROLLER_NONE, m_goHeadsetControllerAnchor},
                { CONTROLLER_TYPE.CONTROLLER_HEADSET, m_goHeadsetControllerAnchor},
                { CONTROLLER_TYPE.CONTROLLER_LEFT_REMOTE, m_goLeftControllerAnchor},
                { CONTROLLER_TYPE.CONTROLLER_RIGHT_REMOTE, m_goRightControllerAnchor}
            };

            m_CurrentControllerAnchor = m_goHeadsetControllerAnchor;
            m_CurrentControllerType = CONTROLLER_TYPE.CONTROLLER_NONE;
            dispatchControllerChanged(m_CurrentControllerType, m_CurrentControllerAnchor, m_CurrentControllerType, m_CurrentControllerAnchor);
        }

        /// <summary>
        /// Set singleton to null when Player is destroyed
        /// </summary>
        public override void destroy()
        {
            if (s_Instance != this)
            {
                return;
            }

            OVRManager.HMDMounted -= onHMDFound;
            OVRManager.HMDUnmounted -= onHMDLost;
            s_Instance = null;
        }


        #region Controller Events

        /// <summary>
        /// Is the headset on the player and the system checking the input.
        /// </summary>
        private bool m_bIsInputActive = false;

        /// <summary>
        /// On Controller changed set the new Controller enum and the new controller anchor.
        /// </summary>
        /// <param name="a_Hashtable"></param>
        private void dispatchControllerChanged(CONTROLLER_TYPE a_OldControllerType, GameObject a_OldControllerAnchor, CONTROLLER_TYPE a_NewControllerType, GameObject a_NewControllerAnchor)
        {
            m_CurrentControllerType = a_NewControllerType;
            m_CurrentControllerAnchor = a_NewControllerAnchor;

            m_OVRInputModule.rayTransform = m_CurrentControllerAnchor.transform;
            m_Pointer.transform.SetParent(m_CurrentControllerAnchor.transform);
            m_Pointer.transform.localPosition = Vector3.zero;
            m_bIsRemoteAttached = m_CurrentControllerType == CONTROLLER_TYPE.CONTROLLER_LEFT_REMOTE || m_CurrentControllerType == CONTROLLER_TYPE.CONTROLLER_RIGHT_REMOTE;

            Hashtable l_hash = new Hashtable(4);
            l_hash.Add(GameEventTypeConst.ID_OLD_CONTROLLER_TYPE, a_OldControllerType);
            l_hash.Add(GameEventTypeConst.ID_OLD_CONTROLLER_ANCHOR, a_OldControllerAnchor);
            l_hash.Add(GameEventTypeConst.ID_NEW_CONTROLLER_TYPE, a_NewControllerType);
            l_hash.Add(GameEventTypeConst.ID_NEW_CONTROLLER_ANCHOR, a_NewControllerAnchor);
            EventManager.Dispatch(GAME_EVENT_TYPE.ON_CONTROLLER_CHANGED, l_hash);
        }

        /// <summary>
        /// Event called on HMD connected..
        /// </summary>
        private void onHMDFound()
        {
            m_bIsInputActive = true;
        }

        /// <summary>
        /// Event called on HMD disconnected..
        /// </summary>
        private void onHMDLost()
        {
            m_bIsInputActive = false;
        }

#endregion Controller Events

#region Controller Components

        /// <summary>
        /// The game object collider the pointer is currently over
        /// </summary>
        private IPointerOver m_IPointerOver = null;

        /// <summary>
        /// The gameobject of the tracked right controller.
        /// </summary>
        [SerializeField]
        private GameObject m_goRightControllerAnchor = null;
        public GameObject RightControllerAnchor
        {
            get { return m_goRightControllerAnchor; }
        }

        /// <summary>
        /// The gameobject of the tracked left controller.
        /// </summary>
        [SerializeField]
        private GameObject m_goLeftControllerAnchor = null;
        public GameObject LeftControllerAnchor
        {
            get { return m_goLeftControllerAnchor; }
        }

        /// <summary>
        /// The gameobject of the tracked headset controller.
        /// </summary>
        [SerializeField]
        private GameObject m_goHeadsetControllerAnchor = null;
        public GameObject HeadsetControllerAnchor
        {
            get { return m_goHeadsetControllerAnchor; }
        }

        /// <summary>
        /// Gets the rotation of the headset anchor
        /// </summary>
        /// <returns></returns>
        public static Quaternion GetHeadsetRotation()
        {
            return s_Instance.m_goHeadsetControllerAnchor.transform.rotation;
        }

        /// <summary>
        /// Dictionary to holder the current controller to the the GameObject that holds the controller source.
        /// </summary>
        private Dictionary<CONTROLLER_TYPE, GameObject> m_DictControllerSets = null;

        /// <summary>
        /// The current controller connected  to the player.
        /// </summary>
        private CONTROLLER_TYPE m_CurrentControllerType = CONTROLLER_TYPE.CONTROLLER_NONE;
        public static CONTROLLER_TYPE CurrentControllerType
        {
            get { return s_Instance.m_CurrentControllerType; }
        }

        /// <summary>
        /// Cureent ray - controller source.
        /// </summary>
        private GameObject m_CurrentControllerAnchor = null;
        public static GameObject CurrentControllerAnchor
        {
            get { return s_Instance.m_CurrentControllerAnchor; }
        }

        /// <summary>
        /// Get the current connected controller.
        /// </summary>
        /// <returns></returns>
        public static CONTROLLER_TYPE GetActiveControllerType()
        {
            if (OVRInput.IsControllerConnected(OVRInput.Controller.LTrackedRemote))
            {
                return CONTROLLER_TYPE.CONTROLLER_LEFT_REMOTE;
            }
            else if (OVRInput.IsControllerConnected(OVRInput.Controller.RTrackedRemote))
            {
                return CONTROLLER_TYPE.CONTROLLER_RIGHT_REMOTE;
            }
            else if (OVRInput.IsControllerConnected(OVRInput.Controller.Touchpad))
            {
                return CONTROLLER_TYPE.CONTROLLER_HEADSET;
            }
            else
            {
                return CONTROLLER_TYPE.CONTROLLER_NONE;
            }
        }

        /// <summary>
        /// Returns the Oculus defined controller type
        /// </summary>
        /// <param name="a_ControllerType"></param>
        /// <returns></returns>
        public static OVRInput.Controller GetPlatformControllerType(CONTROLLER_TYPE a_ControllerType)
        {
            switch (a_ControllerType)
            {
                case CONTROLLER_TYPE.CONTROLLER_LEFT_REMOTE:
                    return OVRInput.Controller.LTrackedRemote;

                case CONTROLLER_TYPE.CONTROLLER_RIGHT_REMOTE:
                    return OVRInput.Controller.RTrackedRemote;
            }

            /// if headset or none return default
            return OVRInput.Controller.Touchpad;
        }

        /// <summary>
        /// Checks the current active controller status.
        /// Sets the current controller and the the controller anchor.
        /// </summary>
        private void updateControllerSource(CONTROLLER_TYPE a_CurrentControllerType)
        {
            CONTROLLER_TYPE l_CurrentControllerType = GetActiveControllerType();
            
            if (l_CurrentControllerType != a_CurrentControllerType)
            {
                GameObject l_goNewAnchor = null;
                GameObject l_goOldAnchor = null;
                m_DictControllerSets.TryGetValue(l_CurrentControllerType, out l_goNewAnchor);
                m_DictControllerSets.TryGetValue(a_CurrentControllerType, out l_goOldAnchor);

                dispatchControllerChanged(a_CurrentControllerType, l_goOldAnchor, l_CurrentControllerType, l_goNewAnchor);
            }
        }

        /// <summary>
        /// Returns the controller rotation
        /// </summary>
        /// <returns></returns>
        public static Quaternion GetControllerRotation()
        {
            return s_Instance.getCurrentControllerRotation();
        }

        /// <summary>
        /// Returns the controller angular velocity
        /// </summary>
        /// <returns></returns>
        public static Vector3 GetControllerAngularVelocity()
        {
            return s_Instance.getCurrentControllerAngularVelocity();
        }

        /// <summary>
        /// Returns the controller angular acceleration
        /// </summary>
        /// <returns></returns>
        public static Vector3 GetControllerAngularAcceleration()
        {
            return s_Instance.getCurrentControllerAngularAcceleration();
        }

        /// <summary>
        /// Returns the current controller rotation if either of the remotes are connected
        /// </summary>
        /// <returns></returns>
        public Quaternion getCurrentControllerRotation()
        {
            if (m_CurrentControllerType == CONTROLLER_TYPE.CONTROLLER_RIGHT_REMOTE)
            {
                return OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTrackedRemote);
            }
            else if (m_CurrentControllerType == CONTROLLER_TYPE.CONTROLLER_LEFT_REMOTE)
            {
                return OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTrackedRemote);
            }
            else
            {
                return Quaternion.identity;
            }
        }

        /// <summary>
        /// Returns the current controller angular velocity if either of the remotes are connected
        /// </summary>
        /// <returns></returns>
        public Vector3 getCurrentControllerAngularVelocity()
        {
            if (m_CurrentControllerType == CONTROLLER_TYPE.CONTROLLER_RIGHT_REMOTE)
            {
                return OVRInput.GetLocalControllerAngularVelocity(OVRInput.Controller.RTrackedRemote);
            }
            else if (m_CurrentControllerType == CONTROLLER_TYPE.CONTROLLER_LEFT_REMOTE)
            {
                return OVRInput.GetLocalControllerAngularVelocity(OVRInput.Controller.LTrackedRemote);
            }
            else
            {
                return Vector3.zero;
            }
        }

        /// <summary>
        /// Returns the current controller angular acceleration if either of the remotes are connected
        /// </summary>
        /// <returns></returns>
        public Vector3 getCurrentControllerAngularAcceleration()
        {
            if (m_CurrentControllerType == CONTROLLER_TYPE.CONTROLLER_RIGHT_REMOTE)
            {
                return OVRInput.GetLocalControllerAngularAcceleration(OVRInput.Controller.RTrackedRemote);
            }
            else if (m_CurrentControllerType == CONTROLLER_TYPE.CONTROLLER_LEFT_REMOTE)
            {
                return OVRInput.GetLocalControllerAngularAcceleration(OVRInput.Controller.LTrackedRemote);
            }
            else
            {
                return Vector3.zero;
            }
        }

        /// <summary>
        /// Returns the current controllers touchpad position
        /// </summary>
        /// <returns></returns>
        public static Vector2 GetPrimaryTouchpadPosition()
        {
            return OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad, ControllerManager.GetPlatformControllerType(ControllerManager.CurrentControllerType));
        }

        /// <summary>
        /// Is either the left or right remote attached.
        /// </summary>
        private bool m_bIsRemoteAttached = false;
        public static bool IsRemoteAttached
        {
            get { return s_Instance.m_bIsRemoteAttached; }
        }

        /// <summary>
        /// is the consoles back button released
        /// </summary>
        /// <returns></returns>
        public static bool IsBackBtnUp()
        {
#if _MASHMO_OVR_
            return OVRInput.GetUp(OVRInput.Button.Back);
#endif

            return false;
        }

        public static bool IsPrimaryTouchpadBtnDown()
        {
#if _MASHMO_OVR_
            return OVRInput.Get(OVRInput.Button.PrimaryTouchpad, GetPlatformControllerType(s_Instance.m_CurrentControllerType));
#endif
            return false;
        }

        #endregion Controller Components

        #region Controller laser pointer

        /// <summary>
        /// Interaction layer mask on raycast allowed to be hit.
        /// </summary>
        [SerializeField]
        private LayerMask m_InteractionLayer;

        /// <summary>
        /// The Gameobject that acts as the cursor.
        /// </summary>
        [SerializeField]
        private GameObject m_Pointer = null;

        /// <summary>
        /// Max laser ray hit distance.
        /// </summary>
        public const float MAX_CURSOR_DISTANCE = 10.0f;

        /// <summary>
        /// The line renderer to render the controller-pointer.
        /// </summary>
        [SerializeField]
        private LineRenderer m_LineRenderer = null;

        /// <summary>
        /// Is the controller laser pointer active.
        /// </summary>
        [SerializeField]
        private bool m_bIsLaserActive = true;
        public static bool IsLaserActive
        {
            get { return s_Instance.m_bIsLaserActive; }
        }

        /// <summary>
        /// Toggle On/Off the controller laser pointer.
        /// </summary>
        /// <param name="a_bSetLaserOn"></param>
        public static void ToggleLaser(bool a_bSetLaserOn)
        {
            s_Instance.m_bIsLaserActive = a_bSetLaserOn;
        }

        /// <summary>
        /// Update the controller laser pointer.
        /// If the current controller is not the headset then use a gaze-pointer.
        /// If using a remote controller use a laser, depending if the laser is active.
        /// </summary>
        private void updateControllerPointer()
        {
            // If laser is disabled, do not raycast the line
            if (!m_bIsLaserActive)
            {
                return;
            }

            RaycastHit l_hit = createRaycast(m_CurrentControllerAnchor);

            if (l_hit.collider != null)
            {
                IPointerOver l_PointerOver = l_hit.collider.gameObject.GetComponent<IPointerOver>();

                /// Hover over : Pointer enter / Pointer exit detection
                if (l_PointerOver != null)
                {
                    if (l_PointerOver != m_IPointerOver)
                    {
                        if (m_IPointerOver != null)
                        {
                            m_IPointerOver.onPointerExit();
                        }
                        m_IPointerOver = l_PointerOver;
                        m_IPointerOver.onPointerEnter();
                    }
                }
                else if (m_IPointerOver != null)
                {
                    m_IPointerOver.onPointerExit();
                    m_IPointerOver = null;
                }
            }
            else
            {
                if (m_IPointerOver != null)
                {
                    m_IPointerOver.onPointerExit();
                    m_IPointerOver = null;
                }
            }
        }

        /// <summary>
        /// Create a raycast from the remote controller anchor to its forward.
        /// </summary>
        /// <param name="a_goController"></param>
        /// <returns></returns>
        private RaycastHit createRaycast(GameObject a_goController)
        {
            RaycastHit l_RaycastHit;
            Ray l_ray = new Ray(a_goController.transform.position, a_goController.transform.forward);
        
            Physics.Raycast(l_ray, out l_RaycastHit, MAX_CURSOR_DISTANCE, m_InteractionLayer);
            
            return l_RaycastHit;
        }

        ///// <summary>
        ///// Enables the line renderer depending on if the m_bIsLaserActive == true or the current controller is not the Touchpad.
        ///// Enables the pointer if the current controller is the touchpad and the m_bIsLaserActive == true.
        ///// </summary>
        //private void setPointerVisibility()
        //{
        //    // Disable line renderer if headset is the current controller or laser pointer is explicity turned off.
        //    m_LineRenderer.enabled = m_bIsLaserActive && (m_CurrentControllerType != CONTROLLER_TYPE.CONTROLLER_HEADSET && m_CurrentControllerType != CONTROLLER_TYPE.CONTROLLER_NONE);
        //    m_Pointer.SetActive(m_bIsLaserActive && (m_CurrentControllerType == CONTROLLER_TYPE.CONTROLLER_HEADSET || m_CurrentControllerType == CONTROLLER_TYPE.CONTROLLER_NONE));
        //}

#endregion Controller laser pointer

        private void Update()
        {
            // If input is inactive, do not check for controller support.
#if !UNITY_EDITOR
            if (!m_bIsInputActive)
            {
                return;
            }
#endif
            updateControllerSource(m_CurrentControllerType);
            updateControllerPointer();
        }
    }
}