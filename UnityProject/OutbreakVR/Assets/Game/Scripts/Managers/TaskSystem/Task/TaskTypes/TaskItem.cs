using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class TaskItem : TaskBase
    {
        #region ATTRIBUTE_KEY
        private const string ATTRIBUTE_ITEM_TYPE = "ItemType";
        private const string ATTRIBUTE_POSITION = "Position";
        private const string ATTRIBUTE_ROTATION = "Rotation";
        private const string ATTRIBUTE_ITEM_ID = "Item_ID";
        private const string ATTRIBUTE_TRIGGER_ID = "ObjectiveTrigger";
        private const string ATTRIBUTE_PARENT_ID = "ParentID";

        #region ITEM SPECIFIC
        private const string ATTRIBUTE_BULLET_COUNT = "BulletCount";
        private const string ATTRIBUTE_HELMET_STRENGTH_PERCENTAGE = "StrengthPercentage";
        #endregion ITEM SPECIFIC

        private const string ATTRIBUTE_CODE = "Code";

        private const string ATTRIBUTE_VALUE_CODE_RETURN_ALL = "ReturnAll";
        private const string ATTRIBUTE_VALUE_CODE_DEACTIVATE = "Deactivate";
        private const string ATTRIBUTE_VALUE_CODE_ACTIVATE = "Activate";
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
        private Vector3 m_v3Rotation = Vector3.zero;
        private string m_strItemID = string.Empty;
        private string m_strParentID = string.Empty;
        private string m_strObjectiveTriggerOnPickup = string.Empty;

        public override void onInitialize()
        {
            base.onInitialize();

            string l_strItemType = getString(ATTRIBUTE_ITEM_TYPE);
            m_strCode = getString(ATTRIBUTE_CODE);

            m_v3Position = getVec3(ATTRIBUTE_POSITION);
            m_v3Rotation = getVec3(ATTRIBUTE_ROTATION);
            m_strItemID = getString(ATTRIBUTE_ITEM_ID);
            m_strParentID = getString(ATTRIBUTE_PARENT_ID);

            m_strObjectiveTriggerOnPickup = getString(ATTRIBUTE_TRIGGER_ID);

            if (!string.IsNullOrEmpty(l_strItemType))
            {
                m_ItemType = (ITEM_TYPE)System.Enum.Parse(typeof(ITEM_TYPE), l_strItemType);
            }
        }

        public override void onExecute()
        {
            base.onExecute();

            switch (m_strCode)
            {
                case ATTRIBUTE_VALUE_CODE_RETURN_ALL:
                    {
                        ItemDropManager.ReturnAllToPool();
                        break;
                    }
                case ATTRIBUTE_VALUE_CODE_DEACTIVATE:
                    {
                        ItemDropManager.ReturnActiveItemToPool(m_ItemType, m_strItemID);
                        break;
                    }
                case ATTRIBUTE_VALUE_CODE_ACTIVATE:
                    {
                        ItemDropBase l_Item = ItemDropManager.GetItemDrop(m_ItemType, m_strItemID);

                        ///Sets parent if valid
                        if (!string.IsNullOrEmpty(m_strParentID))
                        {
                            GameObject l_goParent = GameObjectManager.GetGameObjectById(m_strParentID);
                            if (l_goParent != null)
                            {
                                l_Item.transform.SetParent(l_goParent.transform);
                                l_Item.transform.localPosition = m_v3Position;
                                l_Item.transform.localRotation = Quaternion.Euler(m_v3Rotation);
                            }
                        }
                        else
                        {
                            l_Item.transform.SetPositionAndRotation(m_v3Position, Quaternion.Euler(m_v3Rotation));
                        }

                        l_Item.ObjectiveTriggerOnPickup = m_strObjectiveTriggerOnPickup;

                        ITEM_CATEGORY l_ItemCategory = l_Item.getItemCategoryType();
                        int l_iBullets = getInt(ATTRIBUTE_BULLET_COUNT);

                        switch (l_ItemCategory)
                        {
                            case ITEM_CATEGORY.BULLET:
                            {
                                BulletDrop l_BulletsDrop = (BulletDrop)l_Item;
                                l_BulletsDrop.BulletCount = l_iBullets;
                                break;
                            }
                            case ITEM_CATEGORY.GUN:
                            {
                                GunWeaponDrop l_GunDrop = (GunWeaponDrop)l_Item;
                                l_GunDrop.BulletCount = l_iBullets;
                                break;
                            }
                            case ITEM_CATEGORY.MELEE:
                            {
                                break;
                            }
                            case ITEM_CATEGORY.INVENTORY:
                            {
                                InventoryDrop l_InventoryDrop = (InventoryDrop)l_Item;
                                ITEM_TYPE l_ItemType = l_InventoryDrop.getItemType();

                                switch (l_ItemType)
                                {
                                        case ITEM_TYPE.ITEM_HELMET:
                                            {
                                                HelmetDrop l_HelmetDrop = (HelmetDrop)l_InventoryDrop;
                                                l_HelmetDrop.StrengthPercentage = getInt(ATTRIBUTE_HELMET_STRENGTH_PERCENTAGE);
                                                break;
                                            }

                                        default:
                                            {
                                                break;
                                            }
                                }

                                break;
                            }
                            default:
                            {
                                break;
                            }
                        }

                        break;
                    }
                default:
                    {
                        break;
                    }

            }
            onComplete();
        }
    }
}