using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class KeypadEntity : MonoBehaviour
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
        /// On press it selects this button for the keypad entry
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Toggle m_togKeyPadEntry = null;

        /// <summary>
        /// Disables the toggle so that it cannot be clicked anymore
        /// </summary>
        public void disableKeyInteractability()
        {
            m_togKeyPadEntry.interactable = false;
        }

        /// <summary>
        /// Enables the toggle so that it cannot be clicked anymore
        /// </summary>
        public void enableKeyInteractability()
        {
            m_togKeyPadEntry.interactable = true;
        }

        /// <summary>
        /// Resets to start
        /// </summary>
        public void resetKey()
        {
            enableKeyInteractability();
            m_togKeyPadEntry.isOn = false;
        }

        /// <summary>
        /// Is the toggle of this entity on
        /// </summary>
        /// <returns></returns>
        public bool IsKeyEntityPressed()
        {
            return m_togKeyPadEntry.isOn;
        }
    }
}