using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class EnemyAnimationEventHandler : MonoBehaviour
    {
        [SerializeField]
        private EnemyBase m_EnemyBase = null;

        /// <summary>
        /// Anim event called when enemy strikes animation starts
        /// </summary>
        public void onStrikeAttackStart(int a_iAttackIndex = 0)
        {
            m_EnemyBase.onStrikeAttackStart(a_iAttackIndex);
        }

        /// <summary>
        /// Anim event called when enemy strikes and should inflict damage on the player
        /// </summary>
        public void onStrikeAttack(int a_iAttackIndex = 0)
        {
            m_EnemyBase.onStrikeAttackHitDetection(a_iAttackIndex);
        }

        /// <summary>
        /// Anim event called when the enemy shoots the gun
        /// </summary>
        public void onGunFired()
        {
            m_EnemyBase.onGunFired();
        }
    }
}