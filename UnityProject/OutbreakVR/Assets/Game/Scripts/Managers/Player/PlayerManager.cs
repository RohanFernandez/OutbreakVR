using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class PlayerManager : AbsComponentHandler
    {
        /// <summary>
        /// Singleton instance
        /// </summary>
        private static PlayerManager s_Instance = null;

        [SerializeField]
        private PlayerController m_PlayerController = null;

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
            m_PlayerController.initialize();
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
            m_PlayerController.destroy();
            s_Instance = null;
        }

        /// <summary>
        /// Gets position of the player
        /// </summary>
        /// <returns></returns>
        public static Vector3 GetPosition()
        {
            return s_Instance.m_PlayerController.transform.position;
        }

        /// <summary>
        /// Gets position of the player
        /// </summary>
        /// <returns></returns>
        public static void SetPosition(Vector3 a_v3Position)
        {
            s_Instance.m_PlayerController.transform.position = a_v3Position;
        }
    }
}