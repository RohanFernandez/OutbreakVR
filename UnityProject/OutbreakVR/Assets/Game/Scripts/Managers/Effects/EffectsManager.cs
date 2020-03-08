using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class EffectsManager : AbsComponentHandler, IReuseManager
    {
        /// <summary>
        /// Singleton instance
        /// </summary>
        private static EffectsManager s_Instance = null;

        /// <summary>
        /// The prefab that holds the effect particles
        /// </summary>
        [SerializeField]
        private EffectsBase m_EffectPrefab = null;

        /// <summary>
        /// The pool that manages the creation/ handling of the particle effect
        /// </summary>
        private EffectsPool m_EffectsPool = null;

        /// <summary>
        /// Sets singleton instance
        /// </summary>
        public override void initialize()
        {
            if (s_Instance != null)
            {
                return;
            }
            s_Instance = this;

            m_EffectsPool = new EffectsPool(m_EffectPrefab, gameObject);
        }

        /// <summary>
        /// Destroys singleton instance
        /// </summary>
        public override void destroy()
        {
            if (s_Instance != this)
            {
                return;
            }
            s_Instance = null;
        }

        /// <summary>
        /// Returns the effects object
        /// Sets the time after which to return it back into the pool
        /// </summary>
        /// <returns></returns>
        public static EffectsBase getEffectsBase()
        {
            EffectsBase l_EffectsBase = s_Instance.m_EffectsPool.getObject();
            return l_EffectsBase;
        }

        /// <summary>
        /// Returns the effects object
        /// Sets the time after which to return it back into the pool
        /// </summary>
        /// <returns></returns>
        public static void returnEffectToPool(EffectsBase a_EffectBase)
        {
            s_Instance.m_EffectsPool.returnToPool(a_EffectBase);
        }

        /// <summary>
        /// Returns all back into the pool
        /// </summary>
        public void returnAllToPool()
        {
            m_EffectsPool.returnAll();
        }
    }
}