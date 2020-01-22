﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class InventoryItem : MonoBehaviour
    {
        public enum ITEM_CARRY_TYPE
        { 
            CAN_CARRY_ONE       =   0,
            CAN_CARRY_MULTIPLE  =   1,
        }

        /// <summary>
        /// The player can carry only one item of the type or multiple of the same type
        /// </summary>
        [SerializeField]
        private ITEM_CARRY_TYPE m_ItemCarryType;
        public ITEM_CARRY_TYPE ItemCarryType
        {
            get { return m_ItemCarryType; }
        }

        /// <summary>
        /// Is this item currently in inventory
        /// </summary>
        public bool IsItemInInventory
        {
            get { return ItemsInInventory > 0; }
        }

        /// <summary>
        /// No of items of this type that is in the inventory
        /// </summary>
        [SerializeField]
        private int m_iItemsInInventory = 0;
        public int ItemsInInventory
        {
            get { return m_iItemsInInventory; }
            set {
                    if ((ItemCarryType == ITEM_CARRY_TYPE.CAN_CARRY_ONE && value <= 1) ||
                        ItemCarryType == ITEM_CARRY_TYPE.CAN_CARRY_MULTIPLE)
                    {
                        m_iItemsInInventory = value;
                    }
                }
        }

        /// <summary>
        /// The Id of the inventory item
        /// </summary>
        [SerializeField]
        private INVENTORY_ITEM_ID m_InventoryID;
        public INVENTORY_ITEM_ID InventoryID
        {
            get { return m_InventoryID; }
        }

        /// <summary>
        /// Disables inventory item on player
        /// </summary>
        public virtual void enableItem()
        { 
        
        }

        /// <summary>
        /// Disables inventory item on player
        /// </summary>
        public virtual void disableItem()
        {

        }
    }
}