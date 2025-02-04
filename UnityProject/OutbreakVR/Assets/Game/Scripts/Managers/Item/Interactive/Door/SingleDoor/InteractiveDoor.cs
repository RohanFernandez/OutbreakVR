﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class InteractiveDoor : AbsInteractiveDoorBase
    {
        /// <summary>
        /// The handle of the door
        /// </summary>
        [SerializeField]
        private InteractiveDoorHandle m_InteractiveDoorHandle = null;

        /// <summary>
        /// The go on Side 1 of the door
        /// </summary>
        [SerializeField]
        private GameObject m_goDoorSide1 = null;

        /// <summary>
        /// The go on Side 2 of the door
        /// </summary>
        [SerializeField]
        private GameObject m_goDoorSide2 = null;

        /// <summary>
        /// The collider that maintains the proxiity of the door with the player
        /// </summary>
        [SerializeField]
        private Collider m_colDoorProximity = null;

        /// <summary>
        /// The object that holds all the collider triggers
        /// </summary>
        [SerializeField]
        private InteractiveDoorProximityDetector m_DoorProximityDetector = null;

        /// <summary>
        /// The audio clip to play on door opening
        /// </summary>
        [SerializeField]
        private string m_strAudClipDoorOpening = "AudClip_DoorOpen";

        /// <summary>
        /// The audio clip to play on door closing
        /// </summary>
        [SerializeField]
        private string m_strAudClipDoorClosing = "AudClip_DoorClose";

        /// <summary>
        /// Is the door locked, if true the door cannot be opened
        /// </summary>
        [SerializeField]
        protected bool m_bIsDoorLocked = false;
        protected bool IsDoorLocked
        {
            get { return m_bIsDoorLocked; }
            set
            {
                m_bIsDoorLocked = value;
                toggleDoorLock(m_bIsDoorLocked);
            }
        }

        protected virtual void toggleDoorLock(bool a_bIsLocked)
        {
            m_DoorKnobColorController.setColor(a_bIsLocked ? GameManager.ColOutlineHighlighterRestricted : GameManager.ColOutlineHighlighterNormal);
        }

        public bool IsDoorOpen
        {
            get { return m_bIsDoorOpen; }
            set {
                m_bIsDoorOpen = value;

                m_InteractiveDoorHandle.toggleDoorHandleInteraction(!m_bIsDoorOpen);
                m_DoorKnobColorController.setColor(m_bIsDoorOpen ? GameManager.ColOutlineHighlighterDeactivated : GameManager.ColOutlineHighlighterNormal);
            }
        }

        protected override void Awake()
        {
            base.Awake();
            m_DoorKnobColorController.setColor(m_bIsDoorLocked ? GameManager.ColOutlineHighlighterRestricted : m_bIsDoorOpen ? GameManager.ColOutlineHighlighterDeactivated : GameManager.ColOutlineHighlighterNormal);
        }

        /// <summary>
        /// On interactive pointer entering the door handle
        /// </summary>
        public override void onDoorHandlePointerOver()
        {
            base.onDoorHandlePointerOver();

            if (!IsDoorOpen && !IsDoorLocked)
            {
                m_DoorKnobColorController.setColor(GameManager.ColOutlineHighlighterSelected);
            }
        }

        /// <summary>
        /// On interactive pointer exiting the door handle
        /// </summary>
        public override void onDoorHandlePointerExit()
        {
            base.onDoorHandlePointerExit();
            
            if (!IsDoorOpen && !IsDoorLocked)
            {
                m_DoorKnobColorController.setColor(GameManager.ColOutlineHighlighterNormal);
            }
        }

        /// <summary>
        /// On interactive with the door handle of this door
        /// </summary>
        public override void onDoorHandlePointerInteract()
        {
            base.onDoorHandlePointerInteract();

            if (IsDoorOpen || IsDoorLocked)
            {
                return;
            }
            IsDoorOpen = true;
            openDoor();
        }

        /// <summary>
        /// Opens the door depending upon which side of the door the player is at
        /// </summary>
        public override void openDoor()
        {
            base.openDoor();

            Vector3 l_v3PlayerPos = PlayerManager.GetPosition();
            Vector3 l_v3PlayerToDoorDirection = (m_InteractiveDoorHandle.transform.position - l_v3PlayerPos).normalized;

            ///Is player facing the door so the door has to be opened in side 1, else its in side 2
            bool l_bIsPlayerInSide1 = Vector3.Dot(l_v3PlayerToDoorDirection, (m_InteractiveDoorHandle.transform.position - m_goDoorSide1.transform.position).normalized) > 0.05f;
            
            if (l_bIsPlayerInSide1)
            {
                m_animatorDoorControl.SetTrigger(ANIM_TRIGGER_DOOR_OPEN_SIDE_1);
            }
            else
            {
                m_animatorDoorControl.SetTrigger(ANIM_TRIGGER_DOOR_OPEN_SIDE_2);
            }

            m_UnpooledAudSrc.play(m_strAudClipDoorOpening, false, 1.0f);
        }

        /// <summary>
        /// Closes door, calls anim trigger
        /// </summary>
        public override void closeDoor(bool a_bIsReset = false)
        {
            base.closeDoor(a_bIsReset);

            if (IsDoorOpen)
            {
                IsDoorOpen = false;
                m_animatorDoorControl.SetTrigger(ANIM_TRIGGER_DOOR_OPEN_CLOSE);

                if (!a_bIsReset)
                {
                    m_UnpooledAudSrc.play(m_strAudClipDoorClosing, false, 1.0f);
                }
            }
        }

        /// <summary>
        /// Resets door to closed
        /// </summary>
        public override void resetValues()
        {
            base.resetValues();
            //IsDoorLocked = false;
            closeDoor(true);
            m_DoorProximityDetector.resetValues();
        }

        /// <summary>
        /// Locks / Unlocks door
        /// </summary>
        /// <param name="a_bIsDoorLocked"></param>
        public virtual void lockDoor(bool a_bIsDoorLocked)
        {
            IsDoorLocked = a_bIsDoorLocked;
        }

        protected override void Update()
        {

        }
    }
}
