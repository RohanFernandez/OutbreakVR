using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class TaskItem : TaskBase
    {
        #region ATTRIBUTE_KEY
        private const string ATTRIBUTE_ITEM_TYPE = "ItemType";
        private const string ATTRIBUTE_IS_RETURN_ALL = "IsReturnAll";
        private const string ATTRIBUTE_POSITION = "Position";
        #endregion ATTRIBUTE_KEY

        /// <summary>
        /// The item type to set in the environment
        /// </summary>
        private ITEM_TYPE m_ItemType;

        /// <summary>
        /// If true returns all items back into the pool
        /// </summary>
        private bool m_bIsReturnAllItems = false;

        /// <summary>
        /// The position to spawn the item
        /// </summary>
        private Vector3 m_v3Position = Vector3.zero;

        public override void onInitialize()
        {
            base.onInitialize();

            string l_strItemType = getString(ATTRIBUTE_ITEM_TYPE);
            m_bIsReturnAllItems = getBool(ATTRIBUTE_IS_RETURN_ALL);

            m_v3Position = getVec3(ATTRIBUTE_POSITION);

            if (!string.IsNullOrEmpty(l_strItemType))
            {
                m_ItemType = (ITEM_TYPE)System.Enum.Parse(typeof(ITEM_TYPE), l_strItemType);
            }
        }

        public override void onExecute()
        {
            base.onExecute();

            if (m_bIsReturnAllItems)
            {
                ItemDropManager.ReturnAllToPool();
            }
            else
            {
                ItemDropBase l_ItemDrop = ItemDropManager.GetItemDrop(m_ItemType);
                l_ItemDrop.gameObject.SetActive(true);
                l_ItemDrop.transform.position = m_v3Position;
            }
            onComplete();
        }
    }
}