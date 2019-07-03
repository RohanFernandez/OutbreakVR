using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public abstract class WeaponDropBase : ItemDropBase
    {
        [SerializeField]
        private WEAPON_TYPE m_WeaponType;
        public WEAPON_TYPE WeaponType
        {
            get { return m_WeaponType; }
        }
    }
}