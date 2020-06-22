using Boo.Lang.Environments;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class ControlsMonitorScreenState : MonitorScreenStateBase
    {
        [SerializeField]
        private List<GameObject> m_lstPages = null;

        [SerializeField]
        private int m_iCurrentPageIndex = 0;

        public override void onStateEnter(string a_strOldState)
        {
            base.onStateEnter(a_strOldState);
            hideAllPages();
            goToPageIndex(0);
        }


        private void goToPageIndex(int a_iPageIndex)
        {
            m_iCurrentPageIndex = a_iPageIndex;
            hideAllPages();
            m_lstPages[m_iCurrentPageIndex].SetActive(true);
        }

        public void goToNextPage()
        {
            goToPageIndex((m_iCurrentPageIndex + 1) % m_lstPages.Count);
        }

        public void goToPrevPage()
        {
            goToPageIndex(m_iCurrentPageIndex - 1 < 0 ? m_lstPages.Count - 1 : m_iCurrentPageIndex - 1);
        }

        private void hideAllPages()
        {
            int l_iPageCount = m_lstPages.Count;
            for (int l_iPageIndex = 0; l_iPageIndex < l_iPageCount; l_iPageIndex++)
            {
                m_lstPages[l_iPageIndex].SetActive(false);
            }
        }
    }
}