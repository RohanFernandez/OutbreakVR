using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ns_Mashmo
{
    public enum ENEMY_HIT_COLLISION
    {
        HIT_COLLISION_DEFAULT           =   0,
        HIT_COLLISION_HEAD              =   1,
        HIT_COLLISION_TORSO             =   2,
        HIT_COLLISION_LEFT_SHOULDER     =   3,
        HIT_COLLISION_RIGHT_SHOULDER    =   4,
        HIT_COLLISION_LEFT_ARM          =   5,
        HIT_COLLISION_RIGHT_ARM         =   6,
        HIT_COLLISION_LEFT_THIGH        =   7,
        HIT_COLLISION_RIGHT_THIGH       =   8,
        HIT_COLLISION_LEFT_CALF         =   9,
        HIT_COLLISION_RIGHT_CALF        =   10,
    }

    public class EnemyHitCollider : MonoBehaviour
    {
        [SerializeField]
        private ENEMY_HIT_COLLISION m_HitCollision;

        /// <summary>
        /// the max resitance the collider can take until
        /// </summary>
        [SerializeField]
        private int m_iMaxResistanceDamage = 20;

        /// <summary>
        /// The character wont play the animation until this resistance damage is 0
        /// </summary>
        private int m_iCurrentDamageResistance = 20;

        /// <summary>
        /// The enemy which this collider is a part of
        /// </summary>
        [SerializeField]
        private EnemyBase m_EnemyBase = null;
        public EnemyBase EnemyBase
        {
            get { return m_EnemyBase; }
        }

        /// <summary>
        /// Damage to inflict on the enemy
        /// </summary>
        [SerializeField]
        private float m_fDamageMultiplier = 1.0f;

        public void inflictDamage(int a_iWeaponDamage, Vector3 a_v3HitPoint)
        {
            int l_iDamageInflicted = (int)(a_iWeaponDamage * m_fDamageMultiplier);
            m_iCurrentDamageResistance -= l_iDamageInflicted;
            m_iCurrentDamageResistance = Mathf.Clamp(m_iCurrentDamageResistance, 0, m_iMaxResistanceDamage);
            m_EnemyBase.inflictDamage(l_iDamageInflicted, a_v3HitPoint, (m_iCurrentDamageResistance == 0) ? m_HitCollision : ENEMY_HIT_COLLISION.HIT_COLLISION_DEFAULT);
        }

        /// <summary>
        /// resets damage counter
        /// </summary>
        public void reset()
        {
            m_iCurrentDamageResistance = m_iMaxResistanceDamage;
        }
    }
}