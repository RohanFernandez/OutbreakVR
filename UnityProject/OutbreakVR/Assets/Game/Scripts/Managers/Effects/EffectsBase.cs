using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{ 
    public class EffectsBase : MonoBehaviour, IReusable
    {
        /// <summary>
        /// Returns the object back into the effects pool after the given time
        /// </summary>
        [SerializeField]
        private float m_fReturnAfterTime = 0.0f;

        /// <summary>
        /// The current time the effect is alive
        /// </summary>
        private float m_fCurrentReturnTimer = 0.0f;

        [SerializeField]
        private UnpooledAudioSource m_UnpooledAudSrc = null;

        public void onReturnedToPool()
        {
            gameObject.SetActive(false);
        }

        public void onRetrievedFromPool()
        {
            m_fCurrentReturnTimer = 0.0f;
            gameObject.SetActive(true);
        }

        public void Update()
        {
            m_fCurrentReturnTimer += Time.deltaTime;
            if (m_fCurrentReturnTimer >= m_fReturnAfterTime)
            {
                EffectsManager.returnEffectToPool(this);
            }
        }

        public void playAudioID(string a_strAudioID, float a_fVolume = 1.0f, bool a_bIsLoop = false)
        {
            if (m_UnpooledAudSrc != null)
            {
                m_UnpooledAudSrc.play(a_strAudioID, a_bIsLoop, a_fVolume);
            }
        }
    }
}