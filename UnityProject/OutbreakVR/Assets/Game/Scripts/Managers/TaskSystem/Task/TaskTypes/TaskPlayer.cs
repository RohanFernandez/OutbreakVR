using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class TaskPlayer : TaskBase
    {
        #region ATTRIBUTE_KEY
        private const string ATTRIBUTE_CODE = "Code";
        private const string ATTRIBUTE_PLAYER_STATE = "PlayerState";

        private const string ATTRIBUTE_VALUE_CODE_PLAYER_STATE = "SetPlayerState";
        #endregion ATTRIBUTE_KEY

        /// <summary>
        /// code of instructions
        /// </summary>
        private string m_strCode = string.Empty;

        /// <summary>
        /// The player state to change to
        /// </summary>
        private PLAYER_STATE m_PlayerState;

        public override void onInitialize()
        {
            base.onInitialize();
            m_strCode = getString(ATTRIBUTE_CODE);

            //parse player state
            string l_strPlayerState = getString(ATTRIBUTE_PLAYER_STATE);
            if (!string.IsNullOrEmpty(l_strPlayerState))
            {
                m_PlayerState = (PLAYER_STATE)System.Enum.Parse(typeof(PLAYER_STATE), l_strPlayerState);
            }
        }

        public override void onExecute()
        {
            base.onExecute();

            switch (m_strCode)
            {
                case ATTRIBUTE_VALUE_CODE_PLAYER_STATE:
                    {
                        PlayerManager.SetPlayerState(m_PlayerState);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            onComplete();
        }
    }
}