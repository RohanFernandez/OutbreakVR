using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class Level103_MissileStrikeTask : MonoBehaviour
    {
        [SerializeField]
        private float m_fTimeToStrike = 5.0f;

        [SerializeField]
        private string m_strTaskOnBlast = string.Empty;

        [SerializeField]
        private Transform m_transMissile = null;

        [SerializeField]
        private Transform m_transAirplane = null;

        [SerializeField]
        private Transform m_transBlastPosition = null;

        private float m_fTimeSinceStart = 0.0f;

        Vector3 m_v3MissileDirection = Vector3.zero;
        float m_fMissileDistance = 0.0f;

        Vector3 m_v3PlaneDirection = Vector3.zero;
        float m_fPlaneDistance = 0.0f;

        bool l_bIsHit = false;

        void OnEnable()
        {
            m_fTimeSinceStart = 0.0f;
            l_bIsHit = false;

            m_transBlastPosition.gameObject.SetActive(false);

            m_transMissile.position = m_transMissile.parent.position;
            m_transMissile.gameObject.SetActive(true);

            m_fMissileDistance = Vector3.Distance(m_transBlastPosition.position, m_transMissile.position);
            m_v3MissileDirection = (m_transBlastPosition.position - m_transMissile.position).normalized;

            m_transAirplane.position = m_transAirplane.parent.position;
            m_transAirplane.gameObject.SetActive(true);

            m_fPlaneDistance = Vector3.Distance(m_transBlastPosition.position, m_transAirplane.position);
            m_v3PlaneDirection = (m_transBlastPosition.position - m_transAirplane.position).normalized;
        }

        private void Update()
        {
            if (l_bIsHit) { return; }

            m_fTimeSinceStart += Time.deltaTime;
            if (m_fTimeSinceStart > m_fTimeToStrike)
            {
                l_bIsHit = true;
                TaskManager.ExecuteSequence(m_strTaskOnBlast);
                m_transMissile.gameObject.SetActive(false);
                m_transAirplane.gameObject.SetActive(false);
                m_transBlastPosition.gameObject.SetActive(true);
            }
            else
            {
                float l_fFactor = m_fTimeSinceStart / m_fTimeToStrike;

                m_transMissile.position = m_transMissile.parent.position + m_v3MissileDirection * (m_fMissileDistance * l_fFactor);
                m_transAirplane.position = m_transAirplane.parent.position + m_v3PlaneDirection * (m_fPlaneDistance * l_fFactor);

            }
        }
    }
}