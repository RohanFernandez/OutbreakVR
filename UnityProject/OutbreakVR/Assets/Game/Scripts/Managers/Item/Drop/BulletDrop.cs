using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class BulletDrop : ItemDropBase
    {
        [SerializeField]
        private BULLETS_TYPE m_BulletsType;
        public BULLETS_TYPE BulletsType
        {
            get { return m_BulletsType; }
        }

        [SerializeField]
        private int m_iBulletCount = 0;
        public int BulletCount
        {
            get { return m_iBulletCount; }
            set { m_iBulletCount = value; }
        }

        public override ITEM_CATEGORY getItemCategoryType()
        {
            return ITEM_CATEGORY.BULLET;
        }
    }
}