using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class WeaponCategory
    {
        public WEAPON_CATEGORY_TYPE m_WeaponCategoryType;
        public WEAPON_TYPE m_WeaponType = WEAPON_TYPE.NONE;

        /// <summary>
        /// Initilizes the weapon category
        /// </summary>
        /// <param name="a_WeaponCategoryType"></param>
        public WeaponCategory(WEAPON_CATEGORY_TYPE a_WeaponCategoryType, WEAPON_TYPE a_WeaponType = WEAPON_TYPE.NONE)
        {
            m_WeaponCategoryType = a_WeaponCategoryType;
            m_WeaponType = a_WeaponType;
        }
    }
}