﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class EnemyHitCollider : MonoBehaviour
    {
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
            m_EnemyBase.inflictDamage((int)(a_iWeaponDamage * m_fDamageMultiplier), a_v3HitPoint);
        }
    }
}