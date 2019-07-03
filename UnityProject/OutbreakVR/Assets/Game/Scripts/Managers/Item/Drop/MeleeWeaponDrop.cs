using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class MeleeWeaponDrop : WeaponDropBase
    {
        public override ITEM_CATEGORY getItemCategoryType()
        {
            return ITEM_CATEGORY.MELEE;
        }
    }
}