﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    [System.Serializable]
    public class KeypadEntry
    {
        [Range(1, 9)]
        [SerializeField]
        private int m_iPreSetCode = 0;
        public int PreSetCode
        {
            get { return m_iPreSetCode; }
        }

        [SerializeField]
        private int m_iEnteredCode = 0;
        public int EnteredCode
        {
            get { return m_iEnteredCode; }
            set { m_iEnteredCode = value; }
        }

        public bool isCodeMatch()
        {
            return m_iPreSetCode == EnteredCode;
        }
    }

    public class InteractiveLockedDoor : InteractiveDoor
    {
        /// <summary>
        /// List of all keypad number entry points
        /// </summary>
        [SerializeField]
        private List<KeypadEntity> m_lstKeypadEntities = null;

        /// <summary>
        /// The pre set and entered code
        /// </summary>
        [SerializeField]
        private List<KeypadEntry> m_lstKeypadEntries = null;

        /// <summary>
        /// The max entries the user can put
        /// </summary>
        private int m_iMaxEntries = 0;

        /// <summary>
        /// The entries entered by the user
        /// </summary>
        private int m_iEnteredEntryCount = 0;

        /// <summary>
        /// Is the door locked, if true the door cannot be opened
        /// </summary>
        [SerializeField]
        private bool m_bIsDoorLocked = false;
        private bool IsDoorLocked
        {
            get { return m_bIsDoorLocked; }
            set {
                m_bIsDoorLocked = value;
                m_OutlineGroupHighlighterBase.toggleHighlighter(true, m_bIsDoorLocked ? GameManager.ColOutlineHighlighterRestricted : GameManager.ColOutlineHighlighterNormal);
            }
        }

        /// <summary>
        /// On interactive with the door handle of this door
        /// </summary>
        public override void onDoorHandlePointerInteract()
        {
            //if door is unlocked then open door else remain locked
            if (!IsDoorLocked)
            {
                base.onDoorHandlePointerInteract();
            }
        }

        /// <summary>
        /// On interactive pointer entering the door handle
        /// </summary>
        public override void onDoorHandlePointerOver()
        {
            if (!IsDoorLocked)
            {
                base.onDoorHandlePointerOver();
            }
        }

        /// <summary>
        /// On interactive pointer exiting the door handle
        /// </summary>
        public override void onDoorHandlePointerExit()
        {
            if (!IsDoorLocked)
            {
                base.onDoorHandlePointerExit();
            }
        }

        /// <summary>
        /// Resets door to closed
        /// </summary>
        public override void resetValues()
        {
            base.resetValues();
            IsDoorLocked = true;

            resetKeypad();
        }

        /// <summary>
        /// Resets the keypad entries
        /// </summary>
        private void resetKeypad()
        {
            m_iMaxEntries = m_lstKeypadEntries.Count;
            m_iEnteredEntryCount = 0;

            int l_iKeypadEntityCount = m_lstKeypadEntities.Count;
            for (int l_iKeypadEntityIndex = 0; l_iKeypadEntityIndex < l_iKeypadEntityCount; l_iKeypadEntityIndex++)
            {
                m_lstKeypadEntities[l_iKeypadEntityIndex].resetKey();
            }
        }

        /// <summary>
        /// On the keypad entity clicked enter into current passcode to check
        /// </summary>
        /// <param name="a_KeypadEntity"></param>
        public void onKeypadEntityClicked(KeypadEntity a_KeypadEntity)
        {
            ///if the key entity is not pressed i.e. keypad is enabled, dont do anything
            if (!a_KeypadEntity.IsKeyEntityPressed())
            {
                return;
            }

            m_UnpooledAudSrc.play(GameConsts.AUD_CLIP_KEYPAD_CLICK, false, 1.0f);

            m_lstKeypadEntries[m_iEnteredEntryCount].EnteredCode = a_KeypadEntity.KeyPadIndex;
            a_KeypadEntity.disableKeyInteractability();

            m_iEnteredEntryCount++;

            if (m_iMaxEntries == m_iEnteredEntryCount)
            {
                if (IsKeycodeMatch())
                {
                    m_UnpooledAudSrc.play(GameConsts.AUD_CLIP_KEYPAD_CORRECT_CODE, false, 1.0f);

                    //Unlock door
                    IsDoorLocked = false;

                    int l_iKeypadEntityCount = m_lstKeypadEntities.Count;
                    for (int l_iKeypadEntityIndex = 0; l_iKeypadEntityIndex < l_iKeypadEntityCount; l_iKeypadEntityIndex++)
                    {
                        m_lstKeypadEntities[l_iKeypadEntityIndex].disableKeyInteractability();
                    }
                }
                else
                {
                    m_UnpooledAudSrc.play(GameConsts.AUD_CLIP_KEYPAD_WRONG_CODE, false, 1.0f);
                    resetKeypad();
                }
            }
        }

        /// <summary>
        /// Checks if the preset code matches with the user set keycode
        /// </summary>
        /// <returns></returns>
        private bool IsKeycodeMatch()
        {
            for (int l_iKeycodeIndex = 0; l_iKeycodeIndex < m_iMaxEntries; l_iKeycodeIndex++)
            {
                if (!m_lstKeypadEntries[l_iKeycodeIndex].isCodeMatch())
                {
                    return false;
                }
            }
            return true;
        }
    }
}