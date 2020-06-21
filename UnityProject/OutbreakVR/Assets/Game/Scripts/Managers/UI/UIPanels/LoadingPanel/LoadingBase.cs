using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class LoadingBase : MonoBehaviour
    {
        [SerializeField]
        private string m_strLoadingPanelID = string.Empty;
        public string LoadingPanelID
        {
            get { return m_strLoadingPanelID; }
        }

        public virtual void show()
        {
            gameObject.SetActive(true);
        }

        public virtual void hide()
        {
            gameObject.SetActive(false);
        }
    }
}