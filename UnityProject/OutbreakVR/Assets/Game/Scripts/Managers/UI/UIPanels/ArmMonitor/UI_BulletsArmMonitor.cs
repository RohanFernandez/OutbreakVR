using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class UI_BulletsArmMonitor : AbsUISingleton
    {
        #region WEAPON

        /// <summary>
        /// The first weapon mag bullet count
        /// </summary>
        [SerializeField]
        private TMPro.TMP_Text m_txtBulletCount = null;

        #endregion WEAPON

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
            WeaponBase l_CurrentWeaponBase = WeaponManager.GetCurrentWeaponBase();

            if (l_CurrentWeaponBase != null)
            {
                GunWeaponBase l_GunWeaponBase = null;
                bool l_bIsWeaponAGun = l_CurrentWeaponBase.m_WeaponCategoryType != WEAPON_CATEGORY_TYPE.MELEE;

                if (l_bIsWeaponAGun)
                {
                    l_GunWeaponBase = (GunWeaponBase)l_CurrentWeaponBase;
                    m_txtBulletCount.text = l_GunWeaponBase.BulletCountInFirstMag.ToString() + " / " + l_GunWeaponBase.TotalBullets.ToString();
                }
                else
                {
                    m_txtBulletCount.text = string.Empty;
                }
            }
        }
    }
}
