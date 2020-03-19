using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public interface IPauseState
    {
        /// <summary>
        /// On right button pressed
        /// </summary>
        void onRightPressed();

        /// <summary>
        /// On left button pressed
        /// </summary>
        void onLeftPressed();

        /// <summary>
        /// On top button pressed
        /// </summary>
        void onTopPressed();

        /// <summary>
        /// On bottom button pressed
        /// </summary>
        void onBottomPressed();

        /// <summary>
        /// On trigger/ select button pressed
        /// </summary>
        void onSelectPressed(System.Action a_onReturnControlToMainPanel);

        /// <summary>
        /// On state selected
        /// </summary>
        void onStateSelected();
    }

    public class PauseManagedStateBase : ManagedState, IPauseState
    {
        public enum PANEL_CONTROL_TYPE
        {
            NONE = 0,
            CONFIRMATION = 1,
            NO_CONFIRMATION = 2,
        }

        /// <summary>
        /// The control panel type
        /// </summary>
        [SerializeField]
        private PANEL_CONTROL_TYPE m_PanelType;
        public PANEL_CONTROL_TYPE PanelType
        {
            get { return m_PanelType; }
        }

        /// <summary>
        /// The main pause control panel
        /// </summary>
        [SerializeField]
        protected MainPauseControlPanel m_MainPauseControlPanel = null;

        #region IPauseState
        /// <summary>
        /// On right button pressed
        /// </summary>
        public virtual void onRightPressed()
        {

        }

        /// <summary>
        /// On left button pressed
        /// </summary>
        public virtual void onLeftPressed()
        {

        }

        /// <summary>
        /// On top button pressed
        /// </summary>
        public virtual void onTopPressed()
        {

        }

        /// <summary>
        /// On bottom button pressed
        /// </summary>
        public virtual void onBottomPressed()
        {

        }

        /// <summary>
        /// On trigger/ select button pressed
        /// </summary>
        public virtual void onSelectPressed(System.Action a_onReturnControlToMainPanel)
        {
            
        }

        /// <summary>
        /// On the state is selected
        /// </summary>
        public virtual void onStateSelected()
        {

        }

        #endregion IPauseState

        public override void onStateEnter(string a_strOldState)
        {
            gameObject.SetActive(true);
        }

        public override void onStateExit(string a_strNewState)
        {
            gameObject.SetActive(false);
        }
    }
}