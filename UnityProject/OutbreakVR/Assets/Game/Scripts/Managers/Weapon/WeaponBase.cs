using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public abstract class WeaponBase : MonoBehaviour
    {
        [SerializeField]
        public WEAPON_CATEGORY_TYPE m_WeaponCategoryType;

        [SerializeField]
        public WEAPON_TYPE m_WeaponType;
    }
}