using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class PauseConfirmationState : PauseManagedStateBase
    {
        /// <summary>
        /// Yes btn 
        /// </summary>
        [SerializeField]
        protected UnityEngine.UI.Button m_btnYes = null;

        /// <summary>
        /// No btn 
        /// </summary>
        [SerializeField]
        protected UnityEngine.UI.Button m_btnNo = null;

        /// <summary>
        /// The deactivated image that acts as an overlay
        /// </summary>
        [SerializeField]
        protected UnityEngine.UI.Image m_imgDeactivated = null;

        /// <summary>
        /// Is the yes option selected
        /// </summary>
        protected bool m_bIsYesSelected = false;

        /// <summary>
        /// The action on selecting no to return the control to the main panel
        /// </summary>
        protected System.Action m_actOnReturnControlToMainPanel = null;

        /// <summary>
        /// On right button pressed
        /// </summary>
        public override void onRightPressed()
        {
            //Select no, disable yes
            m_btnYes.image.sprite = m_btnYes.spriteState.highlightedSprite;
            m_btnNo.image.sprite = m_btnNo.spriteState.selectedSprite;
            m_bIsYesSelected = false;
        }

        /// <summary>
        /// On left button pressed
        /// </summary>
        public override void onLeftPressed()
        {
            //Select yes, disable no
            m_btnYes.image.sprite = m_btnYes.spriteState.selectedSprite;
            m_btnNo.image.sprite = m_btnNo.spriteState.highlightedSprite;
            m_bIsYesSelected = true;
        }

        public override void onStateEnter(string a_strState)
        {
            base.onStateEnter(a_strState);
            onRightPressed();
            m_imgDeactivated.gameObject.SetActive(true);
        }

        /// <summary>
        /// On trigger/ select button pressed
        /// </summary>
        public override void onSelectPressed(System.Action a_onReturnControlToMainPanel)
        {
            base.onSelectPressed(a_onReturnControlToMainPanel);
            m_actOnReturnControlToMainPanel = a_onReturnControlToMainPanel;

            if (m_bIsYesSelected)
            {
                m_btnYes.onClick.Invoke();
            }
            else
            {
                m_btnNo.onClick.Invoke();
            }
        }

        /// <summary>
        /// On the state is selected
        /// </summary>
        public override void onStateSelected()
        {
            base.onStateSelected();
            onRightPressed();
            m_imgDeactivated.gameObject.SetActive(false);
        }

        /// <summary>
        /// event on yess button is selected
        /// </summary>
        public virtual void OnButtonSelected_onYesPressed()
        {
            
        }

        /// <summary>
        /// event on no button is selected
        /// </summary>
        public virtual void OnButtonSelected_onNoPressed()
        {
            if (m_actOnReturnControlToMainPanel != null)
            {
                m_actOnReturnControlToMainPanel();
            }
            m_imgDeactivated.gameObject.SetActive(true);
        }
    }
}