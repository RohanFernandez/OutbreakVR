using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class RegisteredGameObject : MonoBehaviour
    {
        /// <summary>
        /// unique ID to identify this gameobject
        /// </summary>
        [SerializeField]
        private string m_strRegisteredID = string.Empty;

        [SerializeField]
        private bool m_bIsActiveOnStart = true;

        void Awake()
        {
            GameObjectManager.registerGameObj(m_strRegisteredID, this);
            gameObject.SetActive(m_bIsActiveOnStart);
        }

        void OnDestroy()
        {
            GameObjectManager.unregisterGameObj(m_strRegisteredID);
        }
    }
}