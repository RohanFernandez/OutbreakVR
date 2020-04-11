using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class InteractiveCabinetDrawer : AbsInteractiveDoorBase
    {
        /// <summary>
        /// Can reset the interactable value to default state
        /// </summary>
        public override void resetValues()
        {
            base.resetValues();
            closeDoor(true);
        }

        protected override void Awake()
        {
            m_DoorKnobColorController.setColor(GameManager.ColOutlineHighlighterNormal);
        }

        /// <summary>
        /// On interactive pointer entering the door handle
        /// </summary>
        public override void onDoorHandlePointerOver()
        {
            base.onDoorHandlePointerOver();

            if (!m_bIsDoorOpen)
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
            if (!m_bIsDoorOpen)
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
            if (!m_bIsDoorOpen)
            {
                openDoor();

                m_DoorKnobColorController.setColor(GameManager.ColOutlineHighlighterDeactivated);
            }
        }

        /// <summary>
        /// Opens the door depending upon which side of the door the player is at
        /// </summary>
        public override void openDoor()
        {
            base.openDoor();
            m_bIsDoorOpen = true;

            m_animatorDoorControl.SetTrigger(ANIM_TRIGGER_DRAWER_OPEN);
            m_UnpooledAudSrc.play(GameConsts.AUD_CLIP_DRAWER_OPEN, false, 1.0f);
        }

        /// <summary>
        /// Closes door, calls anim trigger
        /// </summary>
        public override void closeDoor(bool a_bIsReset = false)
        {
            base.closeDoor(a_bIsReset);
            m_bIsDoorOpen = false;

            m_animatorDoorControl.SetTrigger(ANIM_TRIGGER_DOOR_OPEN_CLOSE);

            m_DoorKnobColorController.setColor(GameManager.ColOutlineHighlighterNormal);
        }
    }
}