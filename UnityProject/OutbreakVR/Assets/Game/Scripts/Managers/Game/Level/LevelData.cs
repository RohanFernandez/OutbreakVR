using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    [System.Serializable]
    public class LevelData
    {
        /// <summary>
        /// The name of this level
        /// </summary>
        [SerializeField]
        private string m_strLevelName = string.Empty;
        public string LevelName
        {
            get { return m_strLevelName; }
        }

        /// <summary>
        /// The level data index in the list of all level data
        /// </summary>
        [SerializeField]
        private int m_iLevelDataIndex = 0;
        public int LevelDataIndex
        {
            get { return m_iLevelDataIndex; }
        }

        /// <summary>
        /// The scene associated with this level
        /// </summary>
        [SerializeField]
        private string m_strSceneName = string.Empty;
        public string SceneName
        {
            get { return m_strSceneName; }
        }

        /// <summary>
        /// Sets the sub level data index
        /// </summary>
        public void initialize(int a_iLevelDataIndex)
        {
            m_iLevelDataIndex = a_iLevelDataIndex;
            int l_iSubLevelDataCount = m_lstSubLevels.Count;
            for (int l_iSubLevelDataIndex = 0; l_iSubLevelDataIndex < l_iSubLevelDataCount; l_iSubLevelDataIndex++)
            {
                m_lstSubLevels[l_iSubLevelDataIndex].SubLevelDataIndex = l_iSubLevelDataIndex + 1;
            }
        }

        /// <summary>
        /// The total number of sub levels
        /// </summary>
        [SerializeField]
        private List<SubLevelData> m_lstSubLevels = null;
        public List<SubLevelData> LstSubLevels
        {
            get { return m_lstSubLevels; }
        }

        /// <summary>
        /// The level to load on the last sublevel is complete
        /// </summary>
        [SerializeField]
        private string m_strLoadNextLevelOnComplete = string.Empty;
        public string LoadNextLevelOnComplete
        {
            get { return m_strLoadNextLevelOnComplete; }
        }

        /// <summary>
        /// Returns the sub level data name included in the m_lstSubLevels if it exists
        /// </summary>
        /// <param name="a_strSubLevelName"></param>
        /// <returns></returns>
        public SubLevelData getSubLevelData(string a_strSubLevelName)
        {
            SubLevelData l_CurrSubLevelData = null;
            int l_iSubLevelDataCount = m_lstSubLevels.Count;
            for (int l_iCurrSubLevelIndex = 0; l_iCurrSubLevelIndex < l_iSubLevelDataCount; l_iCurrSubLevelIndex++)
            {
                if (a_strSubLevelName.Equals(m_lstSubLevels[l_iCurrSubLevelIndex].SubLevelName, System.StringComparison.OrdinalIgnoreCase))
                {
                    l_CurrSubLevelData = m_lstSubLevels[l_iCurrSubLevelIndex];
                }
            }
            return l_CurrSubLevelData;
        }
    }
}