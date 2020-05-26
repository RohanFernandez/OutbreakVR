using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class UI_TrainingTips : AbsUISingleton
    {
        /// <summary>
        /// singleton instance
        /// </summary>
        private static UI_TrainingTips s_Instance = null;

        /// <summary>
        /// The transform of the camera
        /// </summary>
        [SerializeField]
        private Transform m_CamTransform = null;

        /// <summary>
        /// The list of tip entities
        /// </summary>
        [SerializeField]
        private List<UI_TipEntity> m_lstTipEntities = null;

        /// <summary>
        /// The current tip that is displayed
        /// </summary>
        [SerializeField]
        private UI_TipEntity m_CurrentTip = null;

        /// <summary>
        /// initializes, sets singleton to this
        /// </summary>
        public override void initialize()
        {
            if (s_Instance != null)
            {
                return;
            }
            s_Instance = this;
            disableAllTipEntities();
        }

        /// <summary>
        /// sets singleton to null
        /// </summary>
        public override void destroy()
        {
            if (s_Instance != this)
            {
                return;
            }
            s_Instance = null;
        }

        private void Update()
        {
            transform.localRotation = Quaternion.Euler(0.0f, m_CamTransform.rotation.eulerAngles.y, 0.0f);
        }

        public static void Show(string a_strCode)
        {
            s_Instance.show(a_strCode);
        }

        public override void show(string a_strCode)
        {
            if (string.IsNullOrEmpty(a_strCode))
            {
                return;
            }

            if (showTip(a_strCode))
            {
                base.show(a_strCode);
            }
        }

        public static void Hide()
        {
            s_Instance.hide();
        }

        private bool showTip(string a_strCode)
        {
            int l_iTipsCount = m_lstTipEntities.Count;
            for (int l_iTipIndex = 0; l_iTipIndex < l_iTipsCount; l_iTipIndex++)
            {
                UI_TipEntity l_CurrentTip = m_lstTipEntities[l_iTipIndex];
                if (a_strCode.Equals(l_CurrentTip.TipID, System.StringComparison.OrdinalIgnoreCase))
                {
                    if (m_CurrentTip != null)
                    {
                        m_CurrentTip.toggleActive(false);
                    }
                    m_CurrentTip = l_CurrentTip;
                    m_CurrentTip.toggleActive(true);
                    return true;
                }
            }

            return false;
        }

        private void disableAllTipEntities()
        {
            int l_iTipsCount = m_lstTipEntities.Count;
            for (int l_iTipIndex = 0; l_iTipIndex < l_iTipsCount; l_iTipIndex++)
            {
                m_lstTipEntities[l_iTipIndex].toggleActive(false);
            }
        }
    }
}