using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public abstract class AbsUIPanel : MonoBehaviour
    {
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