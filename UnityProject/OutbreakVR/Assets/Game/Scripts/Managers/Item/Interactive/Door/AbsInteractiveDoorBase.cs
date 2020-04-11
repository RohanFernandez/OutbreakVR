using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public abstract class AbsInteractiveDoorBase : AbsEnvironmentInteractableObject
    {
        /// <summary>
        /// Anim trigger to open the door on side 1
        /// </summary>
        public const string ANIM_TRIGGER_DOOR_OPEN_SIDE_1 = "DoorOpeningSide1";

        /// <summary>
        /// Anim trigger to open the door on side 2
        /// </summary>
        public const string ANIM_TRIGGER_DOOR_OPEN_SIDE_2 = "DoorOpeningSide2";

        /// <summary>
        /// Anim trigger to open the drawer
        /// </summary>
        public const string ANIM_TRIGGER_DRAWER_OPEN = "DrawerOpening";

        /// <summary>
        /// Anim trigger to close the door on any side
        /// </summary>
        public const string ANIM_TRIGGER_DOOR_OPEN_CLOSE = "DoorClose";

        /// <summary>
        /// The audio src that plays the door open and close
        /// </summary>
        [SerializeField]
        protected UnpooledAudioSource m_UnpooledAudSrc = null;

        /// <summary>
        /// The animator controller to open, close the door side
        /// </summary>
        [SerializeField]
        public Animator m_animatorDoorControl = null;

        /// <summary>
        /// Is the door currently open
        /// </summary>
        protected bool m_bIsDoorOpen = false;

        /// <summary>
        /// Sets the color of the door knob
        /// </summary>
        [SerializeField]
        protected DoorKnobColorController m_DoorKnobColorController = null;

        protected virtual void Awake()
        {
            
        }

        /// <summary>
        /// On interactive pointer entering the door handle
        /// </summary>
        public virtual void onDoorHandlePointerOver()
        {
        }

        /// <summary>
        /// On interactive pointer exiting the door handle
        /// </summary>
        public virtual void onDoorHandlePointerExit()
        {
        }

        /// <summary>
        /// On interactive with the door handle of this door
        /// </summary>
        public virtual void onDoorHandlePointerInteract()
        {
        }

        /// <summary>
        /// Opens the door depending upon which side of the door the player is at
        /// </summary>
        public virtual void openDoor()
        {
        }

        /// <summary>
        /// Closes door, calls anim trigger
        /// </summary>
        public virtual void closeDoor(bool a_bIsReset = false)
        {
        }

        protected virtual void Update()
        { 
        
        }
    }
}