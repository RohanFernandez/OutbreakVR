using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public abstract class NonStaticEnemy : EnemyBase
    {
        /// <summary>
        /// The nav mesh agent of this body to manage movement
        /// </summary>
        [SerializeField]
        protected UnityEngine.AI.NavMeshAgent m_NavMeshAgent = null;

        public override void activateEnemy()
        {
            base.activateEnemy();
            m_NavMeshAgent.Warp(PlayerManager.GetPosition());
        }

        public override void Update()
        {
            base.Update();
            if (m_bIsEnemyReady)
            {
                float l_fDistanceToPlayer = Vector3.Distance(PlayerManager.GetPosition(), transform.position);
                if (l_fDistanceToPlayer < 15.0f)
                {
                    m_NavMeshAgent.SetDestination(PlayerManager.GetPosition());
                }
            }
        }
    }
}