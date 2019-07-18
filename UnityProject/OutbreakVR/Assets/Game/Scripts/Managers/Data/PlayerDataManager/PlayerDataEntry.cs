using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{

    /// <summary>
    /// A single data entry that is serialized for debug and ease of use purpose.
    /// </summary>
    [System.Serializable]
    public class PlayerDataEntry
    {
        /// <summary>
        /// Internal format of saved data type that will be stored into PlayerPrefs.
        /// </summary>
        public enum EntryType
        {
            EntryType_STRING,
            EntryType_INT,
            EntryType_BOOL,
            EntryType_FLOAT,
        }

        /// <summary>
        /// The key type should be unique
        /// </summary>
        [SerializeField]
        public PLAYER_KEYS m_PlayerKeyType = 0;

        /// <summary>
        /// The unique name of the entry
        /// </summary>
        [SerializeField]
        public string m_strEntryID = string.Empty;

        /// <summary>
        /// What is the data type string, int, float, bool
        /// </summary>
        [SerializeField]
        public EntryType m_EntryType;

        /// <summary>
        /// Should this variable be saved into device storage data
        /// </summary>
        [SerializeField]
        public bool m_bIsSavedIntoDeviceData = false;

        #region Data type

        [SerializeField]
        public string m_stringEntry = string.Empty;

        [SerializeField]
        public int m_intEntry = 0;

        [SerializeField]
        public bool m_boolEntry = false;

        [SerializeField]
        public float m_floatEntry = 0.0f;

        #endregion Data type
    }
}