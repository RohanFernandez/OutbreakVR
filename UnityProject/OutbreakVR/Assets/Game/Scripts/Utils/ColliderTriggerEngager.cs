using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public interface IColliderEventReceiver
    {
        void onTriggerEnterCallback(Collider a_Collider);
        void onTriggerExitCallback(Collider a_Collider);
    }

    public class ColliderTriggerEngager : MonoBehaviour
    {
        IColliderEventReceiver m_IColliderEventReceiver = null;

        public void setColliderReceiver(IColliderEventReceiver a_IColliderEventReceiver)
        {
            m_IColliderEventReceiver = a_IColliderEventReceiver;
        }

        private void OnTriggerEnter(Collider a_Collider)
        {
            if (m_IColliderEventReceiver != null)
            {
                m_IColliderEventReceiver.onTriggerEnterCallback(a_Collider);
            }
        }
        private void OnTriggerExit(Collider a_Collider)
        {
            if (m_IColliderEventReceiver != null)
            {
                m_IColliderEventReceiver.onTriggerExitCallback(a_Collider);
            }
        }
    }
}