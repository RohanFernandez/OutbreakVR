using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class KeypadEntity : MonoBehaviour, IPointerOver
    {
        /// <summary>
        /// The index of this keypad
        /// </summary>
        [SerializeField]
        private int m_iKeyPadIndex = 0;
        public int KeyPadIndex
        {
            get { return m_iKeyPadIndex; }
        }

        /// <summary>
        /// the collider of this button
        /// </summary>
        [SerializeField]
        private Collider m_colDoorButton = null;

        /// <summary>
        /// The outline highlighter of this button
        /// </summary>
        [SerializeField]
        private OutlineHighlighterBase m_OutlineHighlighter = null;

        /// <summary>
        /// The main door component that manages all workings of this door
        /// </summary>
        [SerializeField]
        private InteractiveLockedDoor m_InteractiveLockedDoor = null;

        /// <summary>
        /// Is the button selected
        /// </summary>
        [SerializeField]
        private bool m_bIsSelected = false;
        public bool IsSelected
        {
            get { return m_bIsSelected; }
            set { m_bIsSelected = value; }
        }

        /// <summary>
        /// Disables the toggle so that it cannot be clicked anymore
        /// </summary>
        public void disableKeyInteractability()
        {
            IsSelected = true;
            m_colDoorButton.enabled = false;
            if (m_OutlineHighlighter != null)
            {
                m_OutlineHighlighter.toggleHighlighter(true, GameManager.ColOutlineHighlighterDeactivated);
            }
        }

        /// <summary>
        /// Enables the toggle so that it cannot be clicked anymore
        /// </summary>
        public void enableKeyInteractability()
        {
            IsSelected = false;
            m_colDoorButton.enabled = true;
            if (m_OutlineHighlighter != null)
            {
                m_OutlineHighlighter.toggleHighlighter(true, GameManager.ColOutlineHighlighterNormal);
            }
        }

        /// <summary>
        /// Resets to start
        /// </summary>
        public void resetKey()
        {
            enableKeyInteractability();
        }

        #region IPointerOver
        public void onPointerEnter()
        {
            if (m_OutlineHighlighter != null && !IsSelected)
            {
                m_OutlineHighlighter.toggleHighlighter(true, GameManager.ColOutlineHighlighterSelected);
            }
        }

        public void onPointerExit()
        {
            if (m_OutlineHighlighter != null && !IsSelected)
            {
                m_OutlineHighlighter.toggleHighlighter(true, GameManager.ColOutlineHighlighterNormal);
            }
        }

        public void onPointerInteract()
        {
            IsSelected = true;
            m_InteractiveLockedDoor.onKeypadEntityClicked(this);
        }

        #endregion IPointerOver
    }
}