using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    [System.Serializable]
    public class WeaponInfo
    {
        /// <summary>
        /// The category the weapon is in
        /// </summary>
        [SerializeField]
        public WEAPON_CATEGORY_TYPE m_CategoryType;

        /// <summary>
        /// The type of the weapon
        /// </summary>
        [SerializeField]
        public WEAPON_TYPE m_WeaponType;

        /// <summary>
        /// The total number of bullets
        /// </summary>
        [SerializeField]
        public int m_iTotalBulletsCount = 0;

        /// <summary>
        /// The number of bullets currently in the first mag
        /// </summary>
        [SerializeField]
        public int m_iBulletInFirstMag = 0;
    }

    [System.Serializable]
    public class WeaponInventoryStructure
    {
        [SerializeField]
        public WeaponInfo m_MeleeWeaponInfo = null;

        [SerializeField]
        public WeaponInfo m_PrimaryWeaponInfo = null;

        [SerializeField]
        public WeaponInfo m_SecondaryWeaponInfo = null;

        [SerializeField]
        public WEAPON_CATEGORY_TYPE m_WeaponCateogoryType; 
    }
}