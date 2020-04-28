using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public enum PLAYER_KEYS
    {
        _OUTBREAK_NONE                                  = 666,
        _OUTBREAK_CHECKPOINT_LEVEL                      = 667,

        _OUTBREAK_USERNAME                              = 0, // Oculus user name
        _OUTBREAK_USER_ID                               = 1, // Unique ID set by Mashmo server for all games
        _OUTBREAK_SYNCD_COINS                           = 2,
        _OUTBREAK_UNSYNCD_COINS                         = 3,
        
        _OUTBREAK_GAMES_COMPLETED                       = 4,

        _OUTBREAK_LEVEL1_102                            = 102,
        _OUTBREAK_LEVEL1_103                            = 103,
        _OUTBREAK_LEVEL1_104                            = 104,
        _OUTBREAK_LEVEL1_105                            = 105,
        _OUTBREAK_LEVEL1_106                            = 106,
        _OUTBREAK_LEVEL1_107                            = 107,
        _OUTBREAK_LEVEL1_108                            = 108,
        _OUTBREAK_LEVEL1_109                            = 109,
        _OUTBREAK_LEVEL1_110                            = 110,
    }

    public class PlayerDataManager : AbsComponentHandler
    {
        /// <summary>
        /// singleton instance.
        /// </summary>
        private static PlayerDataManager s_Instance = null;

        /// <summary>
        /// The username of the player.
        /// Each attribute of the player data is saved with the Username + _ + Attribute.
        /// </summary>
        private string m_strPlayerUsername = string.Empty;
        public static string PlayerUsername
        {
            get { return s_Instance.m_strPlayerUsername; }
            set { s_Instance.m_strPlayerUsername = value; }
        }
        
        /// <summary>
        /// List of all data entries to be saved.
        /// </summary>
        [SerializeField]
        private List<PlayerDataEntry> m_lstPlayerDataEntries = null;

        /// <summary>
        /// The dictionary that will hold the type of player data to the actual saved player data entry
        /// </summary>
        private Dictionary<PLAYER_KEYS, PlayerDataEntry> m_dictPlayerPrefsData = null;

        /// <summary>
        /// Initializes singleton instance. 
        /// Created only once from the GameManager.
        /// If instance exists then returns null else sets & returns the singleton.
        /// </summary>
        public override void initialize()
        {
            if (s_Instance != null)
            {
                return;
            }
            s_Instance = this;
        }

        /// <summary>
        /// Sets the singleton to null on game destruction.
        /// </summary>
        public override void destroy()
        {
            if (s_Instance != this)
            {
                return;
            }
            s_Instance = null;
        }

        /// <summary>
        /// Does the players data (player prefs) exist on the device
        /// </summary>
        /// <returns></returns>
        public static bool IsPlayerDataExistOnDevice(string a_strUsername)
        {
            return PlayerPrefs.HasKey(PLAYER_KEYS._OUTBREAK_USERNAME.ToString() + a_strUsername);
        }

        /// <summary>
        /// Initializes all player prefs
        /// Sets name of player pref key with prefix as username
        /// </summary>
        public static void InitDataWithUsername(string a_strUsername)
        {
            PlayerDataManager.PlayerUsername = a_strUsername;
            int l_iKeyCount = s_Instance.m_lstPlayerDataEntries.Count;

            s_Instance.m_dictPlayerPrefsData = new Dictionary<PLAYER_KEYS, PlayerDataEntry>(l_iKeyCount);

            for (int l_iCurrentDataEntry = 0; l_iCurrentDataEntry < l_iKeyCount; l_iCurrentDataEntry++)
            {
                PlayerDataEntry l_CurrentPlayerDataEntry = s_Instance.m_lstPlayerDataEntries[l_iCurrentDataEntry];
                l_CurrentPlayerDataEntry.m_strEntryID = PlayerUsername + l_CurrentPlayerDataEntry.m_PlayerKeyType.ToString();

                s_Instance.m_dictPlayerPrefsData.Add(l_CurrentPlayerDataEntry.m_PlayerKeyType, l_CurrentPlayerDataEntry);

                /// Check if data is to be stored in player prefs and if its was prev stored or not
                /// If not stored previously store the value from the list
                /// Else get the data from memory and store it into the dictionary, 'Get' get from memory and set into dictionary
                if (l_CurrentPlayerDataEntry.m_bIsSavedIntoDeviceData )
                {
                    if (PlayerPrefs.HasKey(l_CurrentPlayerDataEntry.m_strEntryID))
                    {
                        switch (l_CurrentPlayerDataEntry.m_EntryType)
                        {
                            case PlayerDataEntry.EntryType.EntryType_BOOL:
                                GetBool(l_CurrentPlayerDataEntry.m_PlayerKeyType);
                                break;
                            case PlayerDataEntry.EntryType.EntryType_FLOAT:
                                GetFloat(l_CurrentPlayerDataEntry.m_PlayerKeyType);
                                break;
                            case PlayerDataEntry.EntryType.EntryType_INT:
                                GetInt(l_CurrentPlayerDataEntry.m_PlayerKeyType);
                                break;
                            case PlayerDataEntry.EntryType.EntryType_STRING:
                                GetString(l_CurrentPlayerDataEntry.m_PlayerKeyType);
                                break;
                        }
                    }
                    else
                    {
                        switch (l_CurrentPlayerDataEntry.m_EntryType)
                        {
                            case PlayerDataEntry.EntryType.EntryType_BOOL:
                                SetBool(l_CurrentPlayerDataEntry.m_PlayerKeyType, l_CurrentPlayerDataEntry.m_boolEntry);
                                break;
                            case PlayerDataEntry.EntryType.EntryType_FLOAT:
                                SetFloat(l_CurrentPlayerDataEntry.m_PlayerKeyType, l_CurrentPlayerDataEntry.m_floatEntry);
                                break;
                            case PlayerDataEntry.EntryType.EntryType_INT:
                                SetInt(l_CurrentPlayerDataEntry.m_PlayerKeyType, l_CurrentPlayerDataEntry.m_intEntry);
                                break;
                            case PlayerDataEntry.EntryType.EntryType_STRING:
                                SetString(l_CurrentPlayerDataEntry.m_PlayerKeyType, l_CurrentPlayerDataEntry.m_stringEntry);
                                break;
                        }
                    }
                }
            }
            //s_Instance.m_lstPlayerDataEntries = null;
        }

        
        #region Set Player Data Entry

        /// <summary>
        /// Sets String.
        /// </summary>
        /// <param name="a_strEntryValue"></param>
        public static void SetString(PLAYER_KEYS a_Key, string a_strEntryValue)
        {
            s_Instance.setData(a_Key, PlayerDataEntry.EntryType.EntryType_STRING, a_strEntryValue);
        }

        /// <summary>
        /// Sets Int 
        /// </summary>
        /// <param name="a_iEntryValue"></param>
        public static void SetInt(PLAYER_KEYS a_Key, int a_iEntryValue)
        {
            s_Instance.setData(a_Key, PlayerDataEntry.EntryType.EntryType_INT, a_iEntryValue.ToString());
        }

        /// <summary>
        /// Sets Float
        /// </summary>
        /// <param name="a_fEntryValue"></param>
        public static void SetFloat(PLAYER_KEYS a_Key, float a_fEntryValue)
        {
            s_Instance.setData(a_Key, PlayerDataEntry.EntryType.EntryType_FLOAT, a_fEntryValue.ToString());
        }

        /// <summary>
        /// Sets bool
        /// </summary>
        /// <param name="a_strEntryID"></param>
        /// <param name="a_bEntryValue"></param>
        public static void SetBool(PLAYER_KEYS a_Key, bool a_bEntryValue)
        {
            s_Instance.setData(a_Key, PlayerDataEntry.EntryType.EntryType_STRING, (a_bEntryValue ? 1 : 0).ToString());
        }

        /// <summary>
        /// Sets data to player prefs with unique id if exist.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a_EntryType"></param>
        /// <param name="a_TypeValue"></param>
        public bool setData(PLAYER_KEYS a_PlayerKey, PlayerDataEntry.EntryType a_EntryType, string a_strTypeValue)
        {
            PlayerDataEntry l_CurrentPlayerEntry = null;
            m_dictPlayerPrefsData.TryGetValue(a_PlayerKey, out l_CurrentPlayerEntry);
            if (l_CurrentPlayerEntry == null)
            {
                Debug.LogError("PlayerDataManager::setData:: Cannot find entry with key :" + a_PlayerKey.ToString());
                return false;
            }
            else if (l_CurrentPlayerEntry.m_EntryType != a_EntryType)
            {
                Debug.LogError("PlayerDataManager::setData:: The Entry ID '" + a_PlayerKey.ToString() + "' already exists in the PlayerPrefs but the type to set '" + a_EntryType +
                    "' is different compared to the type in PlayerPrefs :: '" + l_CurrentPlayerEntry.m_EntryType + "' ");
                return false;
            }

            try
            {
                switch (a_EntryType)
                {
                    case PlayerDataEntry.EntryType.EntryType_STRING:

                        l_CurrentPlayerEntry.m_stringEntry = a_strTypeValue;
                        if (l_CurrentPlayerEntry.m_bIsSavedIntoDeviceData)
                        {
                            PlayerPrefs.SetString(l_CurrentPlayerEntry.m_strEntryID, a_strTypeValue);
                        }
                        break;

                    case PlayerDataEntry.EntryType.EntryType_BOOL:

                        int l_iBoolValue = 0;
                        if (!int.TryParse(l_CurrentPlayerEntry.m_strEntryID, out l_iBoolValue))
                        {
                            throw new UnityException("Cast Exception from string to int.");
                        }
                        else
                        {
                            l_CurrentPlayerEntry.m_boolEntry = (l_iBoolValue != 0);
                            if (l_CurrentPlayerEntry.m_bIsSavedIntoDeviceData)
                            { 
                                PlayerPrefs.SetInt(l_CurrentPlayerEntry.m_strEntryID, l_iBoolValue);
                            }
                        }
                        break;

                    case PlayerDataEntry.EntryType.EntryType_INT:
                        int l_iValue = 0;
                        if (!int.TryParse(a_strTypeValue, out l_iValue))
                        {
                            throw new UnityException("Cast Exception from string to int.");
                        }
                        else
                        {
                            l_CurrentPlayerEntry.m_intEntry = l_iValue;
                            if (l_CurrentPlayerEntry.m_bIsSavedIntoDeviceData)
                            {
                                PlayerPrefs.SetInt(l_CurrentPlayerEntry.m_strEntryID, l_iValue);
                            }
                        }

                        break;

                    case PlayerDataEntry.EntryType.EntryType_FLOAT:
                        float l_fValue = 0.0f;
                        if (!float.TryParse(a_strTypeValue, out l_fValue))
                        {
                            throw new UnityException("Cast Exception from string to float.");
                        }
                        else
                        {
                            l_CurrentPlayerEntry.m_floatEntry = l_fValue;
                            if (l_CurrentPlayerEntry.m_bIsSavedIntoDeviceData)
                            {
                                PlayerPrefs.SetFloat(a_strTypeValue, l_fValue);
                            }
                        }
                        break;

                    default:
                        Debug.LogError("PlayerDataManager::setData:: Cannot set data of type '" + a_EntryType.ToString() + "' with id '" + a_PlayerKey + "'.");
                        break;
                }
            }
            catch (UnityException a_Exception)
            {
                Debug.LogError("PlayerDataManager::setData<T>:: Failed to save player data with ID '" + a_PlayerKey + "'. Does not match the save given enum type of '" + a_EntryType.ToString() + "' EXCEPTION :: " + a_Exception.Message);
                return false;
            }

            return true;
        }

        #endregion Set Player Data Entry

        #region Get Player Data Entry

        /// <summary>
        /// Does the key with id exist in saved data.
        /// </summary>
        /// <param name="a_strEntryID"></param>
        /// <returns></returns>
        private PlayerDataEntry getDataEntryWithKey(PLAYER_KEYS a_PlayerKey)
        {
            PlayerDataEntry l_ReturnDataEntry = null;
            m_dictPlayerPrefsData.TryGetValue(a_PlayerKey, out l_ReturnDataEntry);
            return l_ReturnDataEntry;
        }

        /// <summary>
        /// Returns Player Data Entry stored player prefs string with given Entry ID.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a_strDataEntryID"></param>
        /// <returns></returns>
        public static string GetString(PLAYER_KEYS a_PlayerKey)
        {
            PlayerDataEntry l_returnEntry = s_Instance.getDataEntryWithKey(a_PlayerKey);
            if ((l_returnEntry == null) ||
                (PlayerDataEntry.EntryType.EntryType_STRING != l_returnEntry.m_EntryType))
            {
                Debug.LogError("PlayerDataManager::GetPlayerDataWithEntry:: Unable to get player data with entry :"+ a_PlayerKey.ToString());
                return null;
            }

            if (l_returnEntry.m_bIsSavedIntoDeviceData)
            {
                l_returnEntry.m_stringEntry = PlayerPrefs.GetString(l_returnEntry.m_strEntryID);
            }

            return l_returnEntry.m_stringEntry;
        }

        /// <summary>
        /// Returns Player Data Entry stored player prefs int with given Entry ID.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a_strDataEntryID"></param>
        /// <returns></returns>
        public static int GetInt(PLAYER_KEYS a_PlayerKey)
        {
            PlayerDataEntry l_returnEntry = s_Instance.getDataEntryWithKey(a_PlayerKey);
            if ((l_returnEntry == null) ||
                (PlayerDataEntry.EntryType.EntryType_INT != l_returnEntry.m_EntryType))
            {
                Debug.LogError("PlayerDataManager::GetPlayerDataWithEntry:: Unable to get player data with entry :" + a_PlayerKey.ToString());
                return 0;
            }

            if (l_returnEntry.m_bIsSavedIntoDeviceData)
            {
                l_returnEntry.m_intEntry = PlayerPrefs.GetInt(l_returnEntry.m_strEntryID);
            }

            return l_returnEntry.m_intEntry;
        }

        /// <summary>
        /// Returns Player Data Entry stored player prefs float with given Entry ID.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a_strDataEntryID"></param>
        /// <returns></returns>
        public static float GetFloat(PLAYER_KEYS a_PlayerKey)
        {
            PlayerDataEntry l_returnEntry = s_Instance.getDataEntryWithKey(a_PlayerKey);
            if ((l_returnEntry == null) ||
                (PlayerDataEntry.EntryType.EntryType_FLOAT != l_returnEntry.m_EntryType))
            {
                Debug.LogError("PlayerDataManager::GetPlayerDataWithEntry:: Unable to get player data with entry :" + a_PlayerKey.ToString());
                return 0.0f;
            }

            if (l_returnEntry.m_bIsSavedIntoDeviceData)
            {
                l_returnEntry.m_floatEntry = PlayerPrefs.GetFloat(l_returnEntry.m_strEntryID);
            }

            return l_returnEntry.m_floatEntry;
        }

        /// <summary>
        /// Returns Player Data Entry stored player prefs float with given Entry ID.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a_strDataEntryID"></param>
        /// <returns></returns>
        public static bool GetBool(PLAYER_KEYS a_PlayerKey)
        {
            PlayerDataEntry l_returnEntry = s_Instance.getDataEntryWithKey(a_PlayerKey);
            if ((l_returnEntry == null) ||
                (PlayerDataEntry.EntryType.EntryType_BOOL != l_returnEntry.m_EntryType))
            {
                Debug.LogError("PlayerDataManager::GetPlayerDataWithEntry:: Unable to get player data with entry :" + a_PlayerKey.ToString());
                return false;
            }

            if (l_returnEntry.m_bIsSavedIntoDeviceData)
            {
                l_returnEntry.m_boolEntry = PlayerPrefs.GetInt(l_returnEntry.m_strEntryID) != 0;
            }

            return l_returnEntry.m_boolEntry;
        }

        #endregion Get Player Data Entry
    }
}
