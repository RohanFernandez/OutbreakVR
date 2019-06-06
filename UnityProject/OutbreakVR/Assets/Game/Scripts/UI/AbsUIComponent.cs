using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public abstract class AbsUIComponent : MonoBehaviour
    {
        /// <summary>
        /// Displays/ Hides the UI
        /// </summary>
        /// <param name="a_bIsActive"></param>
        public virtual void toggleDisplay(bool a_bIsActive)
        {
            gameObject.SetActive(a_bIsActive);
        }
    }
}