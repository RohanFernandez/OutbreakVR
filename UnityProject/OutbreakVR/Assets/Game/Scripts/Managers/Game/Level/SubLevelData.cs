using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public enum SUB_LEVEL_SAVE_LOAD_DATA_TYPE
    {
        PRE_REGISTERED_LEVEL_DATA   = 0,
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
        [SerializeField]
        private int m_iSubLevelDataIndex = 0;
        public int SubLevelDataIndex
        {
            set { m_iSubLevelDataIndex = value; }
            get { return m_iSubLevelDataIndex; }
        }

        /// <summary>
        /// Is the content of this sub level to be saved on completed
        /// </summary>
        [SerializeField]
        private bool m_bIsCheckpointLevel = false;
        public bool IsCheckpoint
        {
            get { return m_bIsCheckpointLevel; }
        }

        /// <summary>
        /// The player entry corresponding to the sublevel data to be saved
        /// </summary>
        [SerializeField]
        private PLAYER_KEYS m_SubLevelSaveEntryType = PLAYER_KEYS._OUTBREAK_NONE;
        public PLAYER_KEYS SubLevelSaveEntryKeyType
        {
            get { return m_SubLevelSaveEntryType; }
        }

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
        /// Should the m_v3PlayerPosition be set on sub level enter
        /// For ex theis position can be set from other places as well like the task list.
        /// </summary>
        [SerializeField]
        private bool m_bUsePlayerPosition = false;
        public bool UsePlayerPosition
        {
            get{ return m_bUsePlayerPosition;}
        }
        /// <summary>
        /// The position of the player at the start of the level
        /// </summary>
        [SerializeField]
        public Vector3 m_v3PlayerPosition = Vector3.zero;

        /// <summary>
        /// The state o the player on start of this sub level
        /// </summary>
        [SerializeField]
        private PLAYER_STATE m_PlayerStateOnStart = PLAYER_STATE.IN_GAME_MOVEMENT;
        public PLAYER_STATE PlayerStateOnStart
        {
            get { return m_PlayerStateOnStart; }
        }
    }
}