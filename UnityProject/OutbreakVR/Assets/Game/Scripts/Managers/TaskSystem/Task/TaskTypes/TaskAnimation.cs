using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class TaskAnimation : TaskBase
    {
        #region ATTRIBUTE_KEY
        private const string ATTRIBUTE_GAME_OBJ_ID  = "GameObject_ID";
        private const string ATTIBUTE_BOOL_VALUE    = "TriggerBool";
        private const string ATTIBUTE_BOOL_ID       = "TriggerBoolID";
        #endregion ATTRIBUTE_KEY

        Animator m_Animator = null;
        bool m_bValue = false;
        string m_strBoolID = string.Empty;

        public override void onInitialize()
        {
            base.onInitialize();
            
            GameObject l_FoundGameObj = GameObjectManager.GetGameObjectById(getString(ATTRIBUTE_GAME_OBJ_ID));
            if (l_FoundGameObj != null)
            {
                m_Animator = l_FoundGameObj.GetComponent<Animator>();
                m_bValue = getBool(ATTIBUTE_BOOL_VALUE, false);
                m_strBoolID = getString(ATTIBUTE_BOOL_ID);
            }
        }

        public override void onExecute()
        {
            base.onExecute();

            if (m_Animator != null)
            {
                m_Animator.SetBool(m_strBoolID, m_bValue);
            }

            onComplete();
        }
    }
}