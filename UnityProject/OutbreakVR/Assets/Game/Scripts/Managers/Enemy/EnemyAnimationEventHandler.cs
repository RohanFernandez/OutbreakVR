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
        /// Anim event called when enemy strikes and should inflict famage on the player
        /// </summary>
        public void onStrikeAttack()
        {
            m_EnemyBase.onStrikeAttack();
        }
    }
}