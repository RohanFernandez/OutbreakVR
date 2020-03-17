using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class InventoryHealth : InventoryItem
    {
        /// <summary>
        /// Health to be added on small health cannister pickup
        /// </summary>
        [SerializeField]
        private int m_iSmallHealthValue = 15;

        /// <summary>
        /// Health to be added on medium health cannister pickup
        /// </summary>
        [SerializeField]
        private int m_iMediumHealthValue = 35;

        /// <summary>
        /// Health to be added on large health cannister pickup
        /// </summary>
        [SerializeField]
        private int m_iLargeHealthValue = 60;

        /// <summary>
        /// Gets the health value to add with type
        /// </summary>
        /// <returns></returns>
        public int getHealthValueWithPickupType(ITEM_TYPE a_ItemType)
        {
            int l_iHealthToAdd = 0;
            switch (a_ItemType)
            {
                case ITEM_TYPE.ITEM_HEALTH_SMALL:
                    {
                        l_iHealthToAdd = m_iSmallHealthValue;
                        break;
                    }
                case ITEM_TYPE.ITEM_HEALTH_MEDIUM:
                    {
                        l_iHealthToAdd = m_iMediumHealthValue;
                        break;
                    }
                case ITEM_TYPE.ITEM_HEALTH_LARGE:
                    {
                        l_iHealthToAdd = m_iLargeHealthValue;
                        break;
                    }
                default:
                    {
                        Debug.LogError("InventoryHealth::getHealthValueWithPickupType:: Cannot find health value to add with item pickup of type : '"+ a_ItemType.ToString() + "'");
                        break;
                    }
            }

            return l_iHealthToAdd;
        }
    }
}