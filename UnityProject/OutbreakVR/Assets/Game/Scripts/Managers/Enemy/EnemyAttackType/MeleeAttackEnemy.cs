using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public abstract class MeleeAttackEnemy : NonStaticEnemy
    {
        /// <summary>
        /// Damage inflicted on player on strike
        /// </summary>
        [SerializeField]
        private int m_iStrikeDamage = 10;

        /// <summary>
        /// Ref to the audio source
        /// </summary>
        [SerializeField]
        private UnpooledAudioSource m_ManagedAudioSource = null;

        /// <summary>
        /// The audio clip id of punch 1
        /// </summary>
        [SerializeField]
        private string m_strAudClipIDPunch1 = string.Empty;

        /// <summary>
        /// The audio clip id of punch 2
        /// </summary>
        [SerializeField]
        private string m_strAudClipIDPunch2 = string.Empty;

        /// <summary>
        /// The audio clip id of punch 3
        /// </summary>
        [SerializeField]
        private string m_strAudClipIDPunch3 = string.Empty;
        
        public override ENEMY_ATTACK_TYPE getEnemyAttackType()
        {
            return ENEMY_ATTACK_TYPE.MELEE;
        }

        /// <summary>
        /// Activates use of enemy
        /// </summary>
        public override void activateEnemy()
        {
            base.activateEnemy();
        }

        /// <summary>
        /// deactivates use of enemy
        /// </summary>
        public override void deactivateEnemy()
        {
            base.deactivateEnemy();
        }

        /// <summary>
        /// update action called when the enemy is in the idle state 
        /// </summary>
        protected override void onIdleStateUpdate()
        {
            base.onIdleStateUpdate();
        }

        /// <summary>
        /// update action called when the enemy is in the alert state 
        /// </summary>
        protected override void onAlertStateUpdate()
        {
            base.onAlertStateUpdate();
        }

        /// <summary>
        /// Called on killed
        /// </summary>
        protected override void onKilled()
        {
            base.onKilled();
        }

        /// <summary>
        /// the callback on starting to strike
        /// </summary>
        public override void onStrikeAttackStart(int a_iStrikeAttackIndex = 0)
        {
            base.onStrikeAttackStart();

            //Decide which audio clip to play for punch
            int l_iRandomIndex = Random.Range(1, 4);
            string l_strAudCkipPunchId = string.Empty;
            if (l_iRandomIndex == 1) { l_strAudCkipPunchId = m_strAudClipIDPunch1; }
            else if (l_iRandomIndex == 2) { l_strAudCkipPunchId = m_strAudClipIDPunch2; }
            else { l_strAudCkipPunchId = m_strAudClipIDPunch3; }

            m_ManagedAudioSource.play(l_strAudCkipPunchId, false, 1.0f);
        }

        /// <summary>
        /// on enemy strike attack attempt to hit the player
        /// </summary>
        public override void onStrikeAttackHitDetection(int a_iStrikeIndex = 0)
        {
            base.onStrikeAttackHitDetection(a_iStrikeIndex);
            Vector3 l_v3PlayerPosition = PlayerManager.GetPosition();

            float l_fDistance = Vector3.Distance(transform.position, l_v3PlayerPosition);

            Vector3 l_v3EnemyToPlayerDirection = Vector3.Normalize(l_v3PlayerPosition - transform.position);
            float l_v3EnemyToPlayerDot = Vector3.Dot(l_v3EnemyToPlayerDirection, transform.forward);

            if (l_fDistance <= m_fMaxDamagePlayerDamageRadius &&
                l_v3EnemyToPlayerDot > 0.6f)
            {
                PlayerManager.InflictDamage(m_iStrikeDamage, DAMAGE_INFLICTION_TYPE.STRIKE);
            }
        }
    }
}