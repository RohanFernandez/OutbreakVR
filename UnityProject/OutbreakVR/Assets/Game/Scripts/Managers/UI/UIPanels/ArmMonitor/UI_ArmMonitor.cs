using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class UI_ArmMonitor : AbsUISingleton
    {
        #region WEAPON

        /// <summary>
        /// The current weapon type text
        /// </summary>
        [SerializeField]
        private TMPro.TMP_Text m_txtWeaponType = null;

        /// <summary>
        /// The current weapon category text
        /// </summary>
        [SerializeField]
        private TMPro.TMP_Text m_txtWeaponCategory = null;

        /// <summary>
        /// The first weapon mag bullet count
        /// </summary>
        [SerializeField]
        private TMPro.TMP_Text m_txtFirstMagBulletCount = null;

        /// <summary>
        /// The total bullets currently available in the current weapon
        /// </summary>
        [SerializeField]
        private TMPro.TMP_Text m_txtTotalBulletsAvailable = null;

        /// <summary>
        /// The panel gameobject that holds the current weapons bullet
        /// </summary>
        [SerializeField]
        private GameObject m_goBulletPanelParent = null;

        #endregion WEAPON

        #region HEALTH

        /// <summary>
        /// The panel that displays the player health
        /// </summary>
        [SerializeField]
        private GameObject m_goHealthPanel = null;

        [SerializeField]
        private TMPro.TMP_Text m_txtPlayerHealth = null;

        #endregion HEALTH

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
            m_txtPlayerHealth.text = a_iPlayerHealth.ToString();

            WeaponBase l_CurrentWeaponBase = WeaponManager.GetCurrentWeaponBase();

            if (l_CurrentWeaponBase != null)
            {
                m_txtWeaponCategory.text = l_CurrentWeaponBase.m_WeaponCategoryType.ToString();
                m_txtWeaponType.text = l_CurrentWeaponBase.m_WeaponType.ToString();

                bool l_bIsWeaponAGun = l_CurrentWeaponBase.m_WeaponCategoryType != WEAPON_CATEGORY_TYPE.MELEE;
                m_goBulletPanelParent.SetActive(l_bIsWeaponAGun);

                if (l_bIsWeaponAGun)
                {
                    GunWeaponBase l_GunWeaponBase = (GunWeaponBase)l_CurrentWeaponBase;
                    m_txtFirstMagBulletCount.text = l_GunWeaponBase.BulletCountInFirstMag.ToString();
                    m_txtTotalBulletsAvailable.text = l_GunWeaponBase.TotalBullets.ToString();
                }
            }
        }
    }
}