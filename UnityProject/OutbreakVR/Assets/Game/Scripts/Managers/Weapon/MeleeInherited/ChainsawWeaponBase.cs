using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class ChainsawWeaponBase : MeleeWeaponBase
    {
        /// <summary>
        /// The damage inflicted on the enemy per second
        /// </summary>
        [SerializeField]
        private int m_iDamagePerInterval = 10;

        /// <summary>
        /// Damage after Interval
        /// </summary>
        [SerializeField]
        private float m_fDamageAfterInterval = 1.0f;

        private float m_fTimePassedSinceLastDamageInfliction = 0.0f;

        /// <summary>
        /// List of enemies currently in trigger
        /// </summary>
        private Dictionary<string, EnemyBase> m_dictTriggeredEnemies = new Dictionary<string, EnemyBase>(10);

        /// <summary>
        /// On the chainsaw blade's collider is triggerd by an enemy
        /// </summary>
        /// <param name="a_Collider"></param>
        protected override void OnTriggerEnter(Collider a_Collider)
        {
            base.OnTriggerEnter(a_Collider);
            if (GeneralUtils.IsLayerInLayerMask(WeaponManager.GunHitInteractionLayer, a_Collider.gameObject.layer))
            {
                EnemyHitCollider l_EnemyHitCollider = a_Collider.GetComponent<EnemyHitCollider>();
                if (l_EnemyHitCollider != null)
                {
                    EnemyBase l_EnemyBase = l_EnemyHitCollider.EnemyBase;
                    if (!m_dictTriggeredEnemies.ContainsKey(l_EnemyBase.getID()))
                    {
                        m_dictTriggeredEnemies.Add(l_EnemyBase.getID(), l_EnemyBase);
                    }
                }
            }
        }

        /// <summary>
        /// On trigger exit from the enemy or default
        /// </summary>
        /// <param name="a_Collider"></param>
        protected override void OnTriggerExit(Collider a_Collider)
        {
            base.OnTriggerExit(a_Collider);
            if (GeneralUtils.IsLayerInLayerMask(WeaponManager.GunHitInteractionLayer, a_Collider.gameObject.layer))
            {
                EnemyHitCollider l_EnemyHitCollider = a_Collider.GetComponent<EnemyHitCollider>();
                if (l_EnemyHitCollider != null)
                {
                    EnemyBase l_EnemyBase = l_EnemyHitCollider.EnemyBase;
                    if (m_dictTriggeredEnemies.ContainsKey(l_EnemyBase.getID()))
                    {
                        m_dictTriggeredEnemies.Remove(l_EnemyBase.getID());
                    }
                }
            }
        }

        /// <summary>
        /// Inflicts damage every second
        /// </summary>
        private void Update()
        {
            m_fTimePassedSinceLastDamageInfliction += Time.deltaTime;
            if (m_fTimePassedSinceLastDamageInfliction > m_fDamageAfterInterval)
            {
                m_fTimePassedSinceLastDamageInfliction = 0.0f;
                foreach (KeyValuePair<string, EnemyBase> l_dictEnemy in m_dictTriggeredEnemies)
                {
                    l_dictEnemy.Value.inflictDamage(m_iDamagePerInterval, l_dictEnemy.Value.getRandomHitTransformPoint().position);
                }
            }
        }

        /// <summary>
        /// clears the dictionary that includes all the enemies
        /// </summary>
        private void clearRegisteredEnemies()
        {
            m_dictTriggeredEnemies.Clear();
        }

        private void OnEnable()
        {
            clearRegisteredEnemies();
        }

        private void OnDisable()
        {
            clearRegisteredEnemies();
        }
    }
}