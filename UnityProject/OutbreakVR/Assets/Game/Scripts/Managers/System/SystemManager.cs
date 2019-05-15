using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class SystemManager : AbsGroupComponentHandler
    {
        /// <summary>
        /// Singleton instance
        /// </summary>
        private static SystemManager s_Instance = null;

        /// <summary>
        /// This object is the initial manager hence initialize is called by awake
        /// </summary>
        void Awake()
        {
            initialize();
        }

        /// <summary>
        /// This object is marked as a dont destroy object, hence OnDestroy will be called only when the game ends
        /// </summary>
        void OnDestroy()
        {
            destroy();
        }

        public override void initialize()
        {
            if (s_Instance != null)
            {
                return;
            }
            s_Instance = this;
            DontDestroyOnLoad(this);
            base.initialize();
        }

        public override void destroy()
        {
            if (s_Instance != this)
            {
                return;
            }
            base.destroy();
            s_Instance = null;
        }
    }
}