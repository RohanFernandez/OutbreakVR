using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class TaskWeapon : TaskBase
    {
        #region ATTRIBUTE_KEY
        private const string ATTRIBUTE_CODE = "Code";
        private const string ATTRIBUTE_CATEGORY_TYPE = "CategoryType";
        private const string ATTRIBUTE_WEAPON_TYPE = "WeaponType";
        private const string ATTRIBUTE_TOTAL_BULLETS = "TotalBullets";

        private const string ATTRIBUTE_VALUE_CODE_CURRENT_CATEGORY = "SetCurrentCategory";
        private const string ATTRIBUTE_VALUE_CODE_WEAPON_IN_CATEGORY = "SetCategoryWeapon";
        private const string ATTRIBUTE_VALUE_CODE_WEAPON_SET_BULLETS = "SetBulletCount";
        #endregion ATTRIBUTE_KEY

        /// <summary>
        /// code of instructions
        /// </summary>
        private string m_strCode = string.Empty;

        public override void onInitialize()
        {
            base.onInitialize();
            m_strCode = getString(ATTRIBUTE_CODE);
        }

        public override void onExecute()
        {
            base.onExecute();

            string l_strWeaponCategoryType = getString(ATTRIBUTE_CATEGORY_TYPE);
            string l_strWeaponType = getString(ATTRIBUTE_WEAPON_TYPE);

            WEAPON_CATEGORY_TYPE l_WeaponCategoryType = WEAPON_CATEGORY_TYPE.MELEE;
            if (!string.IsNullOrEmpty(l_strWeaponCategoryType))
            {
                l_WeaponCategoryType = (WEAPON_CATEGORY_TYPE)System.Enum.Parse(typeof(WEAPON_CATEGORY_TYPE), l_strWeaponCategoryType);
            }

            WEAPON_TYPE l_WeaponType = WEAPON_TYPE.NONE;
            if (!string.IsNullOrEmpty(l_strWeaponType))
            {
                l_WeaponType = (WEAPON_TYPE)System.Enum.Parse(typeof(WEAPON_TYPE), l_strWeaponType);
            }

            switch (m_strCode)
            {
                case ATTRIBUTE_VALUE_CODE_CURRENT_CATEGORY:
                    {
                        WeaponManager.SetCategoryAsCurrent(l_WeaponCategoryType);
                        break;
                    }
                case ATTRIBUTE_VALUE_CODE_WEAPON_IN_CATEGORY:
                    {
                        WeaponManager.SetCurrentWeaponInCategory(l_WeaponCategoryType, l_WeaponType);
                        break;
                    }
                case ATTRIBUTE_VALUE_CODE_WEAPON_SET_BULLETS:
                    {
                        int l_iTotalBullets = getInt(ATTRIBUTE_TOTAL_BULLETS);
                        WeaponManager.SetBulletCountInWeapon(l_WeaponType, l_iTotalBullets);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            onComplete();
        }
    }
}