using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class EnemyShooter : RangedAttackEnemy
    {
        /// <summary>
        /// The particle system that holds the tracer
        /// </summary>
        [SerializeField]
        protected ParticleSystem m_TracerParticleSystem = null;

        /// <summary>
        /// Ref to the audio source
        /// </summary>
        [SerializeField]
        private UnpooledAudioSource m_WeaponAudSrc = null;

        /// <summary>
        /// The id of the aud clip to be played on weapon shot
        /// </summary>
        [SerializeField]
        private string m_strWeaponShootAudClipID = string.Empty;

        [SerializeField]
        private int m_iGunfireDamage = 8;

        /// <summary>
        /// On enemy state changed
        /// </summary>
        protected override void onStateChanged(ENEMY_STATE l_OldNavState, ENEMY_STATE a_NavState)
        {
            base.onStateChanged(l_OldNavState, a_NavState);
            switch (a_NavState)
            {
                case ENEMY_STATE.IDLE:
                    {
                        m_Animator.SetTrigger(ANIM_TRIGGER_IDLE);
                        break;
                    }
            }
        }

        public override void onGunFired()
        {
            base.onGunFired();

            m_TracerParticleSystem.Play();
            m_WeaponAudSrc.play(m_strWeaponShootAudClipID, false, 1.0f);

            m_RayDetector.origin = m_GunTransformRayPoint.position;
            m_RayDetector.direction = m_GunTransformRayPoint.forward;

            RaycastHit l_RaycastHit;
            if (Physics.Raycast(m_RayDetector, out l_RaycastHit, 100.0f, m_AttackLayerMask))
            {
                if ((l_RaycastHit.collider != null) && 
                    (LayerMask.NameToLayer(GameConsts.LAYER_NAME_PLAYER) == l_RaycastHit.collider.gameObject.layer))
                { 
                    PlayerManager.InflictDamage(m_iGunfireDamage, DAMAGE_INFLICTION_TYPE.GUNFIRE);
                    EffectsBase l_EffectsBase = EffectsManager.getEffectsBase();
                    l_EffectsBase.transform.SetPositionAndRotation(l_RaycastHit.point, Quaternion.LookRotation(l_RaycastHit.normal));
                }
            }

        }
    }
}