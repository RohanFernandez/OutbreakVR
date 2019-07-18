using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    [System.Serializable]
    public class UserData
    {
        public string m_UserID = string.Empty;
        public string m_UserName = string.Empty;

        public string m_iSyncdCoins = string.Empty;
        public string m_iUnsyncdCoins = string.Empty;

        public string m_WeaponTypeMelee = string.Empty;

        public string m_WeaponTypePrimary = string.Empty;
        public int m_BulletCountPrimary = 0;
        public int m_FirstMagBulletCountPrimary = 0;

        public string m_WeaponTypeSecondary = string.Empty;
        public int m_BulletCountSecondary = 0;
        public int m_FirstMagBulletCountSecondary = 0;

        public string m_CurrentWeaponCategory = string.Empty;

        public int m_iLifeCounter = 0;

        public string m_strCurrentLevel = string.Empty;
    }
}