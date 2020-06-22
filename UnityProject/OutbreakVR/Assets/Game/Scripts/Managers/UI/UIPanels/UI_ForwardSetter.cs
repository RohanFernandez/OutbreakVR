using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class UI_ForwardSetter : MonoBehaviour
    {
        [SerializeField]
        private Transform m_CamTransform = null;

        void Update()
        {
            transform.SetPositionAndRotation(m_CamTransform.position + m_CamTransform.forward, m_CamTransform.rotation);
        }
    }
}