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
        void onSelectPressed();
    }

    public class PauseManagedStateBase : ManagedState, IPauseState
    {
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
        public virtual void onSelectPressed()
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