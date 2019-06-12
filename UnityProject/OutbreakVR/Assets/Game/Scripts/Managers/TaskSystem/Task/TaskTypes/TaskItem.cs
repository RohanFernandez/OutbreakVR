using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class TaskItem : TaskBase
    {
        #region ATTRIBUTE_KEY
        private const string ATTRIBUTE_ITEM_TYPE = "ItemType";
        private const string ATTRIBUTE_CODE = "Code";
        private const string ATTRIBUTE_POSITION = "Position";

        private const string ATTRIBUTE_VALUE_CODE_RETURN_ALL = "ReturnAll";
        #endregion ATTRIBUTE_KEY

        /// <summary>
        /// The item type to set in the environment
        /// </summary>
        private ITEM_TYPE m_ItemType;

        /// <summary>
        /// code of instructions
        /// </summary>
        private string m_strCode = string.Empty;

        /// <summary>
        /// The position to spawn the item
        /// </summary>
        private Vector3 m_v3Position = Vector3.zero;

        public override void onInitialize()
        {
            base.onInitialize();

            string l_strItemType = getString(ATTRIBUTE_ITEM_TYPE);
            m_strCode = getString(ATTRIBUTE_CODE);

            m_v3Position = getVec3(ATTRIBUTE_POSITION);

            if (!string.IsNullOrEmpty(l_strItemType))
            {
                m_ItemType = (ITEM_TYPE)System.Enum.Parse(typeof(ITEM_TYPE), l_strItemType);
            }
        }

        public override void onExecute()
        {
            base.onExecute();

            if (string.IsNullOrEmpty(m_strCode))
            {
                ItemDropBase l_ItemDrop = ItemDropManager.GetItemDrop(m_ItemType);
                l_ItemDrop.gameObject.SetActive(true);
                l_ItemDrop.transform.position = m_v3Position;
            }
            else
            {
                switch (m_strCode)
                {
                    case ATTRIBUTE_VALUE_CODE_RETURN_ALL:
                        {
                            ItemDropManager.ReturnAllToPool();
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
            onComplete();
        }
    }
}