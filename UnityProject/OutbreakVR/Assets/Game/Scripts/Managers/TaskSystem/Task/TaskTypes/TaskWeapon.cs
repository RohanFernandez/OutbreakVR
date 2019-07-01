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

        private const string ATTRIBUTE_VALUE_CODE_CURRENT_CATEGORY = "SetCurrentCategory";
        private const string ATTRIBUTE_VALUE_CODE_WEAPON_IN_CATEGORY = "SetCategoryWeapon";
        #endregion ATTRIBUTE_KEY

        /// <summary>
        /// code of instructions
        /// </summary>
        private string m_strCode = string.Empty;

        /// <summary>
        /// The type of weapon category
        /// </summary>
        private WEAPON_CATEGORY_TYPE m_WeaponCategoryType;

        /// <summary>
        /// The weapon type to set
        /// </summary>
        private WEAPON_TYPE m_WeaponType;

        public override void onInitialize()
        {
            base.onInitialize();
            m_strCode = getString(ATTRIBUTE_CODE);
        }

        public override void onExecute()
        {
            base.onExecute();

            switch (m_strCode)
            {
                case ATTRIBUTE_VALUE_CODE_CURRENT_CATEGORY:
                    {
                        string l_strWeaponCategoryType = getString(ATTRIBUTE_CATEGORY_TYPE);
                        if (!string.IsNullOrEmpty(l_strWeaponCategoryType))
                        {
                            WEAPON_CATEGORY_TYPE l_WeaponCategoryType = (WEAPON_CATEGORY_TYPE)System.Enum.Parse(typeof(WEAPON_CATEGORY_TYPE), l_strWeaponCategoryType);
                            WeaponManager.SetCategoryAsCurrent(l_WeaponCategoryType);
                        }
                        break;
                    }
                case ATTRIBUTE_VALUE_CODE_WEAPON_IN_CATEGORY:
                    {
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

                        WeaponManager.SetCurrentWeaponInCategory(l_WeaponCategoryType, l_WeaponType);
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