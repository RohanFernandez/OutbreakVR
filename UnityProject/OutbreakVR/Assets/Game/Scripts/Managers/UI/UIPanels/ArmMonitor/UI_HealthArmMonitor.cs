using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class UI_HealthArmMonitor : AbsUISingleton
    {
        [SerializeField]
        private List<UnityEngine.UI.Image> m_lstHeartImages = null;

        public override void initialize()
        {

        }

        public override void destroy()
        { 
        
        }

        /// <summary>
        /// Updates the weapon related information
        /// </summary>
        public void updateInterface(int a_iPlayerHealth)
        {
            int l_iHeartImageCount = m_lstHeartImages.Count;
            int l_iHealthClamped = Mathf.Clamp(a_iPlayerHealth, 0, 99);

            int l_iHeartsVisible = l_iHealthClamped == 0 ? 0 : (l_iHealthClamped / 25) + 1;

            for (int l_iHeartImageIndex = 0; l_iHeartImageIndex < l_iHeartImageCount; l_iHeartImageIndex++)
            {
                m_lstHeartImages[l_iHeartImageIndex].gameObject.SetActive(l_iHeartsVisible != 0 && l_iHeartsVisible > l_iHeartImageIndex);
            }
        }
    }
}