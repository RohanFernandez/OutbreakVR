using System.Collections;
using System.Collections.Generic;
using TMPro;
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

        ///// <summary>
        ///// The outline highlighter of this button
        ///// </summary>
        //[SerializeField]
        //private OutlineHighlighterBase m_OutlineHighlighter = null;

        /// <summary>
        /// The main door component that manages all workings of this door
        /// </summary>
        [SerializeField]
        private InteractiveLockedDoor m_InteractiveLockedDoor = null;

        [SerializeField]
        private Sprite m_sprSelected = null;

        [SerializeField]
        private Sprite m_sprHovered = null;

        [SerializeField]
        private Sprite m_sprNormal = null;

        [SerializeField]
        private SpriteRenderer m_KeyBackground = null; 

        [SerializeField]
        private TextMeshPro m_txtKeyCode = null;

        private const string SELECTED_INDICATED = "*";

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
            m_KeyBackground.sprite = m_sprSelected;
        }

        /// <summary>
        /// Enables the toggle so that it cannot be clicked anymore
        /// </summary>
        public void enableKeyInteractability()
        {
            IsSelected = false;
            m_colDoorButton.enabled = true;
            m_KeyBackground.sprite = m_sprNormal;
            m_txtKeyCode.text = string.Empty;
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
            if (!IsSelected)
            {
                m_KeyBackground.sprite = m_sprHovered;
                m_txtKeyCode.text = string.Empty;
            }
        }

        public void onPointerExit()
        {
            if (!IsSelected)
            {
                m_KeyBackground.sprite = m_sprNormal;
                m_txtKeyCode.text = string.Empty;
            }
        }

        public void onPointerInteract()
        {
            IsSelected = true;
            m_InteractiveLockedDoor.onKeypadEntityClicked(this);
            if (IsSelected)
            {
                m_KeyBackground.sprite = m_sprSelected;
                m_txtKeyCode.text = SELECTED_INDICATED;
            }
        }

        #endregion IPointerOver
    }
}