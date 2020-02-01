using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class InteractiveCabinetDoor : AbsInteractiveDoorBase
    {
        public enum PIVOT_SIDE
        { 
            PIVOT_ON_LEFT   =   0,
            PIVOT_ON_RIGHT  =   1,
            PIVOT_ON_TOP    =   2,
        }

        /// <summary>
        /// Which side is the door pivot on when facing the door
        /// </summary>
        [SerializeField]
        private PIVOT_SIDE m_PivotSide;

        public bool IsDoorOpen
        {
            get { return m_bIsDoorOpen; }
            set { m_bIsDoorOpen = value; }
        }

        /// <summary>
        /// Resets door to closed
        /// </summary>
        public override void resetValues()
        {
            base.resetValues();
            closeDoor(true);
        }

        /// <summary>
        /// On interactive pointer entering the door handle
        /// </summary>
        public override void onDoorHandlePointerOver()
        {
            base.onDoorHandlePointerOver();

            if (m_OutlineGroupHighlighterBase != null && !IsDoorOpen)
            {
                m_OutlineGroupHighlighterBase.toggleHighlighter(true, GameManager.ColOutlineHighlighterSelected);
            }
        }

        /// <summary>
        /// On interactive pointer exiting the door handle
        /// </summary>
        public override void onDoorHandlePointerExit()
        {
            base.onDoorHandlePointerExit();

            if (m_OutlineGroupHighlighterBase != null && !IsDoorOpen)
            {
                m_OutlineGroupHighlighterBase.toggleHighlighter(true, GameManager.ColOutlineHighlighterNormal);
            }
        }

        /// <summary>
        /// On interactive with the door handle of this door
        /// </summary>
        public override void onDoorHandlePointerInteract()
        {
            base.onDoorHandlePointerInteract();

            if (IsDoorOpen)
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

            if (m_PivotSide == PIVOT_SIDE.PIVOT_ON_RIGHT)
            {
                m_animatorDoorControl.SetTrigger(ANIM_TRIGGER_DOOR_OPEN_SIDE_1);
            }
            else if(m_PivotSide == PIVOT_SIDE.PIVOT_ON_LEFT)
            {
                m_animatorDoorControl.SetTrigger(ANIM_TRIGGER_DOOR_OPEN_SIDE_2);
            }

            if (m_OutlineGroupHighlighterBase != null)
            {
                m_OutlineGroupHighlighterBase.toggleHighlighter(true, GameManager.ColOutlineHighlighterDeactivated) ;
            }

            m_UnpooledAudSrc.play(GameConsts.AUD_CLIP_DOOR_OPEN, false, 1.0f);
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

                if (m_OutlineGroupHighlighterBase != null)
                {
                    m_OutlineGroupHighlighterBase.toggleHighlighter(true, GameManager.ColOutlineHighlighterNormal);
                }

                if (!a_bIsReset)
                {
                    m_UnpooledAudSrc.play(GameConsts.AUD_CLIP_DOOR_CLOSE, false, 1.0f);
                }
            }
        }
    }
}