using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class WeaponCategory
    {
        public WEAPON_CATEGORY_TYPE m_WeaponCategoryType;
        public WEAPON_TYPE m_WeaponType = WEAPON_TYPE.NONE;
        private HashSet<WEAPON_TYPE> m_RegisteredWeaponTypes = null;

        /// <summary>
        /// Initilizes the weapon category
        /// </summary>
        /// <param name="a_WeaponCategoryType"></param>
        public WeaponCategory(WEAPON_CATEGORY_TYPE a_WeaponCategoryType)
        {
            m_WeaponCategoryType = a_WeaponCategoryType;
            m_RegisteredWeaponTypes = new HashSet<WEAPON_TYPE>();
            m_RegisteredWeaponTypes.Add(WEAPON_TYPE.NONE);
        }

        /// <summary>
        /// Adds weapon type to registered in category
        /// </summary>
        /// <param name="a_WeaponType"></param>
        public void addWeaponTypeToCategory(WEAPON_TYPE a_WeaponType)
        {
            m_RegisteredWeaponTypes.Add(a_WeaponType);
        }

        /// <summary>
        /// Is weapon type registered in category
        /// </summary>
        /// <param name="a_WeaponType"></param>
        /// <returns></returns>
        public bool isWeaponTypeinCategoryExist(WEAPON_TYPE a_WeaponType)
        {
            return m_RegisteredWeaponTypes.Contains(a_WeaponType);
        }
    }
}