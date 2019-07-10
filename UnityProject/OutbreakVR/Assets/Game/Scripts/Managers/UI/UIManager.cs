using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class UIManager : AbsComponentHandler
    {
        /// <summary>
        /// Singleton instance
        /// </summary>
        private static UIManager s_Instance = null;

        /// <summary>
        /// List of all the singleton UI instances in the game
        /// </summary>
        [SerializeField]
        private List<AbsUISingleton> m_lstUISingletonInstances = null;

        /// <summary>
        /// Sets singleton instance to this
        /// </summary>
        public override void initialize()
        {
            if (s_Instance != null)
            {
                return;
            }
            s_Instance = this;
            initAllManagedUIs();
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
            destroyAllManagedUIs();
            s_Instance = null;
        }

        /// <summary>
        /// Inits all the singleton UI's managed
        /// </summary>
        private void initAllManagedUIs()
        {
            int l_iSingletonUICount = m_lstUISingletonInstances.Count;
            for (int l_iUIIndex = 0; l_iUIIndex < l_iSingletonUICount; l_iUIIndex++)
            {
                m_lstUISingletonInstances[l_iUIIndex].initialize();
            }
        }

        /// <summary>
        /// Destroys all the singleton UI's managed
        /// </summary>
        private void destroyAllManagedUIs()
        {
            int l_iSingletonUICount = m_lstUISingletonInstances.Count;
            for (int l_iUIIndex = 0; l_iUIIndex < l_iSingletonUICount; l_iUIIndex++)
            {
                m_lstUISingletonInstances[l_iUIIndex].destroy();
            }
        }

        /// <summary>
        /// Disables all UI's
        /// </summary>
        private void disableAllUI()
        {
            int l_iSingletonUICount = m_lstUISingletonInstances.Count;
            for (int l_iUIIndex = 0; l_iUIIndex < l_iSingletonUICount; l_iUIIndex++)
            {
                m_lstUISingletonInstances[l_iUIIndex].hide();
            }
        }
    }
}