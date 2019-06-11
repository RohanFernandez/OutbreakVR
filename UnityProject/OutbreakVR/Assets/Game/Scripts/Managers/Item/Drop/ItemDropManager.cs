using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public enum ITEM_TYPE
    {
        ITEM_FN57
    }

    public class ItemDropManager : AbsComponentHandler, IReuseManager
    {
        /// <summary>
        /// Singleton instance
        /// </summary>
        private static ItemDropManager s_Instance = null;

        /// <summary>
        /// Dictionary of item type to the item drop pool
        /// </summary>
        private Dictionary<ITEM_TYPE, ItemDropPool> m_dictItemDropPool = null;

        /// <summary>
        /// List of all unique item drop types
        /// </summary>
        [SerializeField]
        private List<ItemDropBase> m_lstItemDropObjects = null;

        /// <summary>
        /// Sets singleton to this
        /// </summary>
        public override void initialize()
        {
            if (s_Instance != null)
            {
                return;
            }
            s_Instance = this;
            m_dictItemDropPool = new Dictionary<ITEM_TYPE, ItemDropPool>(10);
            initItemDropDictionary();
        }

        /// <summary>
        /// sets singleton to null
        /// </summary>
        public override void destroy()
        {
            if (s_Instance != this)
            {
                return;
            }
            s_Instance = null;
        }

        /// <summary>
        /// Creats and sets up all drop items into the dictionary from the list of all drop items
        /// </summary>
        private void initItemDropDictionary()
        {
            int l_iItemDropTypeCount = m_lstItemDropObjects.Count;
            for (int l_iItemDropIndex = 0; l_iItemDropIndex < l_iItemDropTypeCount; l_iItemDropIndex++)
            {
                ItemDropBase l_ItemDropType = m_lstItemDropObjects[l_iItemDropIndex];
                m_dictItemDropPool.Add(l_ItemDropType.getItemType(), new ItemDropPool( l_ItemDropType, this.gameObject));
            }
        }

        /// <summary>
        /// Returns an Item Drop game object from the pool
        /// </summary>
        /// <param name="a_ItemType"></param>
        /// <returns></returns>
        public static ItemDropBase GetItemDrop(ITEM_TYPE a_ItemType)
        {
            ItemDropPool l_ItemDropPool = s_Instance.getPool(a_ItemType);
            return (l_ItemDropPool == null) ? null : l_ItemDropPool.getObject();
        }

        /// <summary>
        /// Returns item drop back into its respective pool
        /// </summary>
        public static void ReturnItemToPool(ItemDropBase a_ItemDrop)
        {
            ItemDropPool l_ItemDropPool = s_Instance.getPool(a_ItemDrop.getItemType());
            l_ItemDropPool.returnToPool(a_ItemDrop);
        }

        /// <summary>
        ///  Gets item drop pool of the given type
        /// </summary>
        /// <param name="a_ItemType"></param>
        /// <returns></returns>
        private ItemDropPool getPool(ITEM_TYPE a_ItemType)
        {
            ItemDropPool l_ItemDropPool = null;
            if (!s_Instance.m_dictItemDropPool.TryGetValue(a_ItemType, out l_ItemDropPool))
            {
                Debug.LogError("ItemDropManager::GetItemDrop:: Item of type '" + a_ItemType.ToString() + "' is not registered in the unique item drop list");
            }
            return l_ItemDropPool;
        }

        /// <summary>
        /// Returns all items back into its respective pools
        /// </summary>
        public static void ReturnAllToPool()
        {
            s_Instance.returnAllToPool();
        }

        public void returnAllToPool()
        {
            foreach (KeyValuePair<ITEM_TYPE, ItemDropPool> l_Item in m_dictItemDropPool)
            {
                l_Item.Value.returnAll();
            }
        }
    }
}