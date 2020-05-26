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

        /// <summary>
        /// toggles UI (shows/hides) with uitype
        /// </summary>
        /// <param name="a_UIType"></param>
        /// <param name="a_bToggleValue"></param>
        public static void ToggleUI(AbsUIPanel.UI_TYPE a_UIType, bool a_bToggleValue, string a_strCode = "" )
        {
            AbsUIPanel l_UIPanel = null;
            int l_iSingletonUICount = s_Instance.m_lstUISingletonInstances.Count;
            for (int l_iUIIndex = 0; l_iUIIndex < l_iSingletonUICount; l_iUIIndex++)
            {
                if (s_Instance.m_lstUISingletonInstances[l_iUIIndex].UIType == a_UIType)
                {
                    l_UIPanel = s_Instance.m_lstUISingletonInstances[l_iUIIndex];
                    break;
                }
            }

            if (l_UIPanel == null) { return; }

            if (a_bToggleValue)
            {
                l_UIPanel.show(a_strCode);
            }
            else
            {
                l_UIPanel.hide();
            }
        }

        /// <summary>
        /// toggles UI (shows/hides) with ui category
        /// </summary>
        /// <param name="a_UIType"></param>
        /// <param name="a_bToggleValue"></param>
        public static void DisableUICategory(AbsUIPanel.UI_CATEGORY a_UICategoryType)
        {
            int l_iSingletonUICount = s_Instance.m_lstUISingletonInstances.Count;
            for (int l_iUIIndex = 0; l_iUIIndex < l_iSingletonUICount; l_iUIIndex++)
            {
                if (s_Instance.m_lstUISingletonInstances[l_iUIIndex].UICategoryType == a_UICategoryType)
                {
                    s_Instance.m_lstUISingletonInstances[l_iUIIndex].hide();
                }
            }
        }
    }
}