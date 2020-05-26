using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class UI_TipEntity : MonoBehaviour
    {
        [SerializeField]
        private string m_strTipID = string.Empty;
        public string TipID
        {
            get { return m_strTipID; }
        }

        /// <summary>
        /// Shows/ hides the game object
        /// </summary>
        /// <param name="a_bisActivate"></param>
        public void toggleActive(bool a_bisActivate)
        {
            gameObject.SetActive(a_bisActivate);
        }
    }
}