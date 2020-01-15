using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public abstract class AbsUIPanel : MonoBehaviour
    {
        public enum UI_TYPE
        { 
            UI_GAME     = 0,
            UI_SYSTEM   = 1
        }

        /// <summary>
        /// Specifies if the UI is a system UI like a system notification (error/loading)
        /// or a game specific
        /// </summary>
        [SerializeField]
        private UI_TYPE m_UIType;
        public UI_TYPE UIType
        {
            get { return m_UIType; }
        }

        /// <summary>
        /// displays the UI by activating the gameobject
        /// </summary>
        public virtual void show()
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