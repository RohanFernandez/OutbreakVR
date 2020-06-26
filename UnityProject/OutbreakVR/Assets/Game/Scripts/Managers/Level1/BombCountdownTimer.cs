using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class BombCountdownTimer : MonoBehaviour
    {
        [SerializeField]
        private float m_fTotalTime = 60.0f;

        private float m_fCurrentTimeLeft = 0.0f;

        [SerializeField]
        private UnpooledAudioSource m_AudSrc = null;

        [SerializeField]
        private string m_strAudClipID = string.Empty;

        [SerializeField]
        private string m_strTaskOnBlast = string.Empty;

        void OnEnable()
        {
            m_fCurrentTimeLeft = m_fTotalTime;
            m_AudSrc.play(m_strAudClipID, true, 1.0f);
        }

        void OnDisable()
        {
            m_AudSrc.stop();
        }

        void Update()
        {
            m_fCurrentTimeLeft -= Time.deltaTime;

            //if (m_fCurrentTimeLeft < m_fClickAtTime)
            //{
            //    m_fClickAtTime -= m_fClickDifference;
            //    m_AudSrc.play(m_strAudClipID, false, 1.0f); ;
            //}

            if (m_fCurrentTimeLeft <= 0.0f)
            {
                TaskManager.ExecuteSequence(m_strTaskOnBlast);
                m_AudSrc.stop();
                gameObject.SetActive(false);
            }
        }
    }
}