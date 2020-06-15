using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public abstract class RangedAttackEnemy : NonStaticEnemy
    {
        [SerializeField]
        protected Transform m_GunTransformRayPoint = null;

        public override ENEMY_ATTACK_TYPE getEnemyAttackType()
        {
            return ENEMY_ATTACK_TYPE.RANGED;
        }

        /// <summary>
        /// update action called when the enemy is in the alert state 
        /// </summary>
        protected override void onAlertStateUpdate()
        {
            base.onAlertStateUpdate();
            Vector3 l_v3PlayerPosition = PlayerManager.GetPosition();

            bool l_bIsPlayerVisibleForShot =  isPlayerDetected();

            m_fCurrAlertTimeCounter = l_bIsPlayerVisibleForShot ? m_fMaxAlertTime : (m_fCurrAlertTimeCounter - Time.deltaTime);

            if (m_fCurrAlertTimeCounter <= 0.0f)
            {
                NavState = ENEMY_STATE.IDLE;
            }
            else
            {
                Vector3 l_v3DirectionToPlayer = (l_v3PlayerPosition - m_GunTransformRayPoint.position).normalized;
                float l_fDistanceToPlayer = Vector3.Distance(l_v3PlayerPosition, transform.position);

                bool l_bCanFireTohitPlayer = l_bIsPlayerVisibleForShot && (l_fDistanceToPlayer <= m_fMaxDamagePlayerDamageRadius);
                bool l_bIsFacingPlayer = Vector3.Dot(m_GunTransformRayPoint.forward, transform.forward) > 0.95f;

                ///shoot the player
                if (l_bCanFireTohitPlayer && l_bIsFacingPlayer)
                {
                    m_Animator.SetTrigger(ANIM_TRIGGER_ATTACK);
                    stopNavigation();
                }
                //else if (l_bCanFireTohitPlayer && !l_bIsFacingPlayer)
                //{ 
                //    //m_NavMeshAgent.updateRotation = false;
                //    transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(l_v3DirectionToPlayer, Vector3.up), 270.0f * Time.deltaTime);
                //    //m_NavMeshAgent.updateRotation = true;
                //}
                ///Find a better location to shoot from
                else
                {
                    if (l_fDistanceToPlayer > m_fAlertStoppingDistance)
                    {
                        m_Animator.SetTrigger(ANIM_TRIGGER_WALK);
                        setDestination(l_v3PlayerPosition);
                        startNavigation();
                    }
                }
                transform.rotation = Quaternion.RotateTowards(Quaternion.LookRotation(m_GunTransformRayPoint.forward, Vector3.up), Quaternion.LookRotation(l_v3DirectionToPlayer, Vector3.up), 360.0f * Time.deltaTime);
            }
        }

        public override void onGunFired()
        {
            base.onGunFired();
        }
    }
}