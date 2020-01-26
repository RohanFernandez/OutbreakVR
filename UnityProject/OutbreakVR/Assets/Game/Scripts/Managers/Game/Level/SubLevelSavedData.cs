using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    [System.Serializable]
    public class SubLevelSavedData
    {
        /// <summary>
        /// The weapons the player holds at the moment
        /// </summary>
        [SerializeField]
        public WeaponInventoryStructure m_WeaponInventory = null;

        /// <summary>
        /// The inventory items the player holds at the moment
        /// </summary>
        [SerializeField]
        public ItemInventoryStructure m_ItemInventory = null;

        /// <summary>
        /// The health meter of the play at the moment
        /// </summary>
        [SerializeField]
        public int m_iPlayerHealth = 0;

        /// <summary>
        /// The position of the player at the start of the level
        /// </summary>
        [SerializeField]
        public Vector3 m_v3PlayerPosition = Vector3.zero;
    }
}
