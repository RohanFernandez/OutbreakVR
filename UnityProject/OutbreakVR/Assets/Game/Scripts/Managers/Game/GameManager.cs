using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public enum LEVEL_TYPE
    {
        LEVEL1
    }

    public class GameManager : AbsGroupComponentHandler
    {
        /// <summary>
        /// Singleton instance
        /// </summary>
        private static GameManager s_Instance = null;

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
            base.initialize();
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
            base.destroy();
            s_Instance = null;
        }
    }
}