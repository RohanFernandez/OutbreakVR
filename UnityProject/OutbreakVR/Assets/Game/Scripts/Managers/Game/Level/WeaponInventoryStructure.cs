using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    [System.Serializable]
    public class WeaponInfo
    {
        public virtual WEAPON_CATEGORY_TYPE getCategoryType() { return WEAPON_CATEGORY_TYPE.MAX_WEAPON_CATEGORIES;}

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
    public class MeleeWeaponInfo : WeaponInfo
    {
        /// <summary>
        /// The category the weapon is in
        /// </summary>
        public override WEAPON_CATEGORY_TYPE getCategoryType()
        {
            return WEAPON_CATEGORY_TYPE.MELEE;
        }
    }

    [System.Serializable]
    public class PrimaryWeaponInfo : WeaponInfo
    {
        /// <summary>
        /// The category the weapon is in
        /// </summary>
        public override WEAPON_CATEGORY_TYPE getCategoryType()
        {
            return WEAPON_CATEGORY_TYPE.PRIMARY;
        }
    }

    [System.Serializable]
    public class SecondaryWeaponInfo : WeaponInfo
    {
        /// <summary>
        /// The category the weapon is in
        /// </summary>
        public override WEAPON_CATEGORY_TYPE getCategoryType()
        {
            return WEAPON_CATEGORY_TYPE.SECONDARY;
        }
    }

    [System.Serializable]
    public class WeaponInventoryStructure
    {
        [SerializeField]
        public MeleeWeaponInfo m_MeleeWeaponInfo = null;

        [SerializeField]
        public PrimaryWeaponInfo m_PrimaryWeaponInfo = null;

        [SerializeField]
        public SecondaryWeaponInfo m_SecondaryWeaponInfo = null;

        [SerializeField]
        public WEAPON_CATEGORY_TYPE m_CurrentWeaponCateogoryType; 
    }
}