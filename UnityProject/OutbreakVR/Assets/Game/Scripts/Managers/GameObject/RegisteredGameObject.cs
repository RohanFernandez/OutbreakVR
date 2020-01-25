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

        /// <summary>
        /// Is not registered on Awake or OnDestroy functions
        /// If it is registered on awake, the registerGameObject() could be called before the GameObjectManager is initialized
        /// </summary>
        [SerializeField]
        private bool m_bIsManagedExternally = false;

        void Awake()
        {
            if (m_bIsManagedExternally)
            {
                return;
            }
            registerGameObject();
        }

        public void registerGameObject()
        {
            GameObjectManager.registerGameObj(m_strRegisteredID, this);
            gameObject.SetActive(m_bIsActiveOnStart);
        }

        void OnDestroy()
        {
            if (m_bIsManagedExternally)
            {
                return;
            }
            unregisterGameObject();
        }

        public void unregisterGameObject()
        {
            GameObjectManager.unregisterGameObj(m_strRegisteredID);
        }
    }
}