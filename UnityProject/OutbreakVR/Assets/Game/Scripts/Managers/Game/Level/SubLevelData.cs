using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public enum SUB_LEVEL_SAVE_LOAD_DATA_TYPE
    {
        SAVED_LEVEL_DATA            = 0,
        LOAD_FROM_PREVIOUS_LEVEL    = 1,
        LAST_LEVEL_EXIT             = 2 
    }


    [System.Serializable]
    public class SubLevelData
    {
        /// <summary>
        /// The name of the sub level
        /// </summary>
        [SerializeField]
        private string m_strSubLevelName = string.Empty;
        public string SubLevelName
        {
            get { return m_strSubLevelName; }
        }

        /// <summary>
        /// The sub level index
        /// </summary>
        private int m_iSubLevelDataIndex = 0;
        public int SubLevelDataIndex
        {
            set { m_iSubLevelDataIndex = value; }
            get { return m_iSubLevelDataIndex; }
        }

        /// <summary>
        /// The player entry corresponding to the sublevel data to be saved
        /// </summary>
        [SerializeField]
        private PLAYER_KEYS m_SubLevelSaveEntryType = PLAYER_KEYS._OUTBREAK_NONE;

        /// <summary>
        /// The data to be loaded should be taken from the previous sublevel or should use the stored data
        /// </summary>
        [SerializeField]
        private SUB_LEVEL_SAVE_LOAD_DATA_TYPE m_LoadDataType;
        public SUB_LEVEL_SAVE_LOAD_DATA_TYPE LoadDataType
        {
            get { return m_LoadDataType; }
        }

        /// <summary>
        /// The weapons the player holds at the moment
        /// </summary>
        [SerializeField]
        public WeaponInventoryStructure m_WeaponInventory = null;

        /// <summary>
        /// The health meter of the play at the moment
        /// </summary>
        [SerializeField]
        public int m_iPlayerHealth = 0;
    }
}