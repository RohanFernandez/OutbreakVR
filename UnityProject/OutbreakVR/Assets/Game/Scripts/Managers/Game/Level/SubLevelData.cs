using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public enum SUB_LEVEL_LOAD_DATA_TYPE
    {
        SAVED_LEVEL_DATA,
        LOAD_FROM_PREVIOUS_LEVEL
    }


    [System.Serializable]
    public class SubLevelData
    {
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
        /// The data to be loaded should be taken from the previous sublevel or should use the stored data
        /// </summary>
        [SerializeField]
        private SUB_LEVEL_LOAD_DATA_TYPE m_LoadDataType;
        public SUB_LEVEL_LOAD_DATA_TYPE LoadDataType
        {
            get { return m_LoadDataType; }
        }

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
        /// Is the level progress to be saved on completion
        /// </summary>
        [SerializeField]
        private bool m_bIsSaveLevelOnEnd = false;
        public bool IsSaveLevelOnEnd
        {
            get { return m_bIsSaveLevelOnEnd; }
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