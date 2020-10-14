using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class ItemDropPool : MonoObjectPool<ItemDropBase>
    {
        public ItemDropPool(ItemDropBase a_ItemDropPrefab, GameObject a_Parent, int a_iStartSize = 0)
            : base(a_ItemDropPrefab, a_Parent, a_iStartSize)
        {

        }
    }
}