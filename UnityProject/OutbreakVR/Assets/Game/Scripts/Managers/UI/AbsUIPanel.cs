using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public abstract class AbsUIPanel : MonoBehaviour
    {
        public enum UI_CATEGORY
        { 
            UI_GAME     = 0,
            UI_SYSTEM   = 1
        }

        public enum UI_TYPE
        {
            PLAYER_HELMET           = 0,
            LOADING_PANEL           = 1,
            PAUSE_PANEL             = 2,
            SCREEN_FADER            = 3,
            NOTIFICATION_PANEL      = 4,
            HEALTH_ARM_MONITOR      = 5,
            SCREEN_DAMAGE_INDICATOR = 6,
            TRAINING_TIPS           = 7
        }

        /// <summary>
        /// Specifies if the UI is a system UI like a system notification (error/loading)
        /// or a game specific
        /// </summary>
        [SerializeField]
        private UI_CATEGORY m_UICategoryType;
        public UI_CATEGORY UICategoryType
        {
            get { return m_UICategoryType; }
        }

        [SerializeField]
        private UI_TYPE m_UIType;
        public UI_TYPE UIType
        {
            get { return m_UIType; }
        }

        /// <summary>
        /// displays the UI by activating the gameobject
        /// </summary>
        public virtual void show(string a_strCode = "")
        {
            gameObject.SetActive(true);
        }

        /// <summary>
        /// hides the UI by deactivating the gameobject
        /// </summary>
        public virtual void hide()
        {
            gameObject.SetActive(false);
        }
    }
}