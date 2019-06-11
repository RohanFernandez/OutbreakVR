using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class ItemDropPool : MonoObjectPool<ItemDropBase>
    {
        public ItemDropPool(ItemDropBase a_ItemDropPrefab, GameObject a_Parent)
            : base(a_ItemDropPrefab, a_Parent)
        {

        }
    }
}