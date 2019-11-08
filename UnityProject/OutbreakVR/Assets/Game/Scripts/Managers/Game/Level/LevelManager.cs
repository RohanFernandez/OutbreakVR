using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class LevelManager : AbsComponentHandler
    {
        /// <summary>
        /// Singleton instance
        /// </summary>
        private static LevelManager s_Instance = null;

        /// <summary>
        /// The list of all levels in the game
        /// </summary>
        [SerializeField]
        private List<LevelData> m_lstLevelData = null;

        /// <summary>
        /// The level data of the current level the game is in
        /// </summary>
        [SerializeField]
        private string m_strCurrLevelName = string.Empty;

        /// <summary>
        /// The sublevel data of the current sub level the game is in
        /// </summary>
        [SerializeField]
        private string m_strCurrSubLevelName = string.Empty;

        /// <summary>
        /// Sets the singleton instance
        /// </summary>
        public override void initialize()
        {
            if (s_Instance != null)
            {
                return;
            }
            s_Instance = this;

            int l_iLevelDataCount = m_lstLevelData.Capacity;
            for (int l_iLevelDataIndex = 0; l_iLevelDataIndex < l_iLevelDataCount; l_iLevelDataIndex++)
            {
                m_lstLevelData[l_iLevelDataIndex].initialize();
            }
        }

        /// <summary>
        /// Sets singleton to null
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
        /// Gets the level data with the given name from the list of all Level Datas
        /// </summary>
        /// <param name="a_strLevelData"></param>
        /// <returns></returns>
        private LevelData getLevelDataWithName(string a_strLevelData)
        {
            LevelData l_CurrLevelData = null;
            int l_iLevelDataCount = m_lstLevelData.Count;
            for (int l_iCurrLevelData = 0; l_iCurrLevelData < l_iLevelDataCount; l_iCurrLevelData++)
            {
                if (a_strLevelData.Equals(m_lstLevelData[l_iCurrLevelData].LevelName, System.StringComparison.OrdinalIgnoreCase))
                {
                    l_CurrLevelData = m_lstLevelData[l_iCurrLevelData];
                }
            }
            return l_CurrLevelData;
        }

        /// <summary>
        /// Transferring game to level specified
        /// a_strLevelName is in the format "Levelname"+"_"+"LevelID" ex "Level1_100"
        /// </summary>
        /// <param name="a_strLevelName"></param>
        public static void GoToLevel(string a_strLevelName)
        {
            LevelData l_CurrentLevelData = null;
            SubLevelData l_CurrentSubLevelData = null;

            ///Check if the a_strLevelName is in correct format and can be found in the list of level data and sub level data
            #region LevelNameValidityCheck
            string[] l_strarr = a_strLevelName.Split('_');

            bool l_bIsLevelNameFormatCorrect = (l_strarr.Length > 1);

            if (l_bIsLevelNameFormatCorrect)
            {
                string l_strCurrentLevelName = l_strarr[0];
                string l_strCurrentSubLevelName = l_strarr[1];

                l_CurrentLevelData = s_Instance.getLevelDataWithName(l_strCurrentLevelName);
                if (l_CurrentLevelData == null)
                {
                    Debug.LogError("LevelManager::GoToLevel:: The current level data with name '" + l_strCurrentLevelName + "' could not be found");
                    l_bIsLevelNameFormatCorrect = false;
                }
                else
                {
                    s_Instance.m_strCurrLevelName = l_CurrentLevelData.LevelName;
                }

                if (l_bIsLevelNameFormatCorrect)
                {
                    l_CurrentSubLevelData = l_CurrentLevelData.getSubLevelData(l_strCurrentSubLevelName);
                    if (l_CurrentSubLevelData == null)
                    {
                        Debug.LogError("LevelManager::GoToLevel:: The current sub level data with name '" + l_strCurrentSubLevelName + "' could not be found");
                        l_bIsLevelNameFormatCorrect = false;
                    }
                    else
                    {
                        s_Instance.m_strCurrSubLevelName = l_CurrentSubLevelData.SubLevelName;
                    }
                }
            }
            else
            {
                Debug.LogError("LevelManager::GoToLevel:: The level name of load is not in correct format, Level Name '" + a_strLevelName + "'");
            }

            if (!l_bIsLevelNameFormatCorrect)
            {
                return;
            }
            #endregion LevelNameValidityCheck




            ///Loads the current level data to the player
            #region LOAD_LEVEL_DATA

            SubLevelData l_SubLevelDataToLoadToPlayer = null;
            switch (l_CurrentSubLevelData.LoadDataType)
            {
                case SUB_LEVEL_LOAD_DATA_TYPE.LOAD_FROM_PREVIOUS_LEVEL:
                    {
                        l_SubLevelDataToLoadToPlayer = (l_CurrentSubLevelData.SubLevelDataIndex != 0) ? l_CurrentLevelData.lstSubLevels[--l_CurrentSubLevelData.SubLevelDataIndex] :  l_CurrentSubLevelData;
                        break;
                    }
                case SUB_LEVEL_LOAD_DATA_TYPE.SAVED_LEVEL_DATA:
                    {
                        l_SubLevelDataToLoadToPlayer = l_CurrentSubLevelData;
                        break;
                    }
            }

            PlayerManager.HealthMeter = l_SubLevelDataToLoadToPlayer.m_iPlayerHealth;
            WeaponManager.SetCurrentWeaponInventory(l_SubLevelDataToLoadToPlayer.m_WeaponInventory);

            #endregion LOAD_LEVEL_DATA



            ///Load scene will call the callback directly if already loaded
            GameManager.LoadScene(l_CurrentLevelData.LevelName, s_Instance.onLevelSceneLoadComplete);
        }

        

        private void onLevelSceneLoadComplete()
        {

        }
    }
}