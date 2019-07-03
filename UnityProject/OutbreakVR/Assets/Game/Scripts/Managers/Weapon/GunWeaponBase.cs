using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class GunWeaponBase : WeaponBase
    {
        [SerializeField]
        private BULLETS_TYPE m_BulletsType;
        public BULLETS_TYPE BulletsType
        {
            get { return m_BulletsType; }
        }

        [SerializeField]
        private int m_iMaxMagazinesAllowed = 10;

        [SerializeField]
        private int m_iSingleMagazineBulletCapacity = 10;

        [SerializeField]
        private int m_iCurrentBulletCountInMagazine = 10;

        [SerializeField]
        private int m_iCurrentMagazineCount = 10;

        public override void fire()
        {
            
        }

        public override void reload()
        {

        }
    }
}