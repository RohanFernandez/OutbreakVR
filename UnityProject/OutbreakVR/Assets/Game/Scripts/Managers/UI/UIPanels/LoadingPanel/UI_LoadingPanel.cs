using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class UI_LoadingPanel : AbsUISingleton
    {
        public const string LOADING_PANEL_GAME          = "LoadingGame";
        public const string LOADING_PANEL_QUOTE         = "LoadingQuote";
        public const string LOADING_PANEL_OUTBREAK_LOGO = "LoadingOutbreakLogo";
        public const string LOADING_PANEL_COMPANY_LOGO  = "LoadingCompany";

        /// <summary>
        /// singleton instance
        /// </summary>
        private static UI_LoadingPanel s_Instance = null;

        [SerializeField]
        private List<LoadingBase> m_lstLoadingBase = null;

        Dictionary<string, LoadingBase> m_dictLoadingPanels = null;

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
            m_dictLoadingPanels = new Dictionary<string, LoadingBase>(3);

            int l_iLoadingPanelBaseCount = m_lstLoadingBase.Count;
            for (int l_iLoadingPanelBaseIndex = 0; l_iLoadingPanelBaseIndex < l_iLoadingPanelBaseCount; l_iLoadingPanelBaseIndex++)
            {
                LoadingBase l_LoadingBase = m_lstLoadingBase[l_iLoadingPanelBaseIndex];
                m_dictLoadingPanels.Add(l_LoadingBase.LoadingPanelID, l_LoadingBase);
            }
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

        public static void Show(string a_strCode = LOADING_PANEL_GAME)
        {
            s_Instance.show(a_strCode);
        }

        public static void Hide()
        {
            s_Instance.hide();
        }

        public override void hide()
        {
            base.hide();
            hideAllLoadingBases();
        }

        private void hideAllLoadingBases()
        {
            foreach (KeyValuePair<string, LoadingBase> l_LoadingBase in m_dictLoadingPanels)
            {
                l_LoadingBase.Value.hide();
            }
        }

        public override void show(string a_strCode)
        {
            s_Instance.hideAllLoadingBases();
            base.show(a_strCode);

            LoadingBase l_LoadingBase = null;
            if (m_dictLoadingPanels.TryGetValue(a_strCode, out l_LoadingBase))
            {
                l_LoadingBase.show();
            }
        }
    }
}