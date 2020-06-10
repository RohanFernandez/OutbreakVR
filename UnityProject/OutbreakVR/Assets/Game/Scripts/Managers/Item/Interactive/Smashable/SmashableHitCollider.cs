using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class SmashableHitCollider : MonoBehaviour
    {
        /// <summary>
        /// The object to smash
        /// </summary>
        [SerializeField]
        private SmashableBase m_Smashable = null;

        /// <summary>
        /// Starts smash on hit
        /// </summary>
        public void startSmashOnHit()
        {
            m_Smashable.smash();
        }

        public void inflictDamage(int a_iDamage)
        {
            m_Smashable.inflictDamage(a_iDamage);
        }
    }
}