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
        private const string ATTRIBUTE_PLAYER_HEALTH = "PlayerHealth";
        private const string ATTRIBUTE_INFLICT_DAMAGE = "InflictDamage";

        private const string ATTRIBUTE_VALUE_CODE_PLAYER_STATE = "SetPlayerState";
        private const string ATTRIBUTE_VALUE_CODE_PLAYER_HEALTH = "SetPlayerHealth";
        private const string ATTRIBUTE_VALUE_CODE_INFLICT_DAMAGE = "InflictDamage";
        #endregion ATTRIBUTE_KEY

        /// <summary>
        /// code of instructions
        /// </summary>
        private string m_strCode = string.Empty;

        public override void onInitialize()
        {
            base.onInitialize();
            m_strCode = getString(ATTRIBUTE_CODE);
        }

        public override void onExecute()
        {
            base.onExecute();

            switch (m_strCode)
            {
                case ATTRIBUTE_VALUE_CODE_PLAYER_STATE:
                    {
                        //parse player state
                        string l_strPlayerState = getString(ATTRIBUTE_PLAYER_STATE);
                        if (!string.IsNullOrEmpty(l_strPlayerState))
                        {
                            PLAYER_STATE l_PlayerState = (PLAYER_STATE)System.Enum.Parse(typeof(PLAYER_STATE), l_strPlayerState);
                            PlayerManager.SetPlayerState(l_PlayerState);
                        }
                        
                        break;
                    }
                case ATTRIBUTE_VALUE_CODE_PLAYER_HEALTH:
                    {
                        PlayerManager.HealthMeter = getInt(ATTRIBUTE_PLAYER_HEALTH);

                        break;
                    }
                case ATTRIBUTE_VALUE_CODE_INFLICT_DAMAGE:
                    {
                        PlayerManager.InflictDamage(getInt(ATTRIBUTE_INFLICT_DAMAGE), DAMAGE_INFLICTION_TYPE.DEFAULT);
                        break; ;
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