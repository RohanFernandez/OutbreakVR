using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class TaskGameObject : TaskBase
    {
        #region ATTRIBUTE_KEY
        private const string ATTRIBUTE_GAMEOBJECT_ID  = "GameObject_ID";
        private const string ATTRIBUTE_SET_ACTIVE       = "IsActive";
        #endregion ATTRIBUTE_KEY

        private string m_strGameObjectID = string.Empty;
        private bool m_bIsSetActive = false;

        public override void onInitialize()
        {
            base.onInitialize();
            m_strGameObjectID = getString(ATTRIBUTE_GAMEOBJECT_ID);
            m_bIsSetActive = getBool(ATTRIBUTE_SET_ACTIVE);
        }

        public override void onExecute()
        {
            base.onExecute();
            GameObject l_GameObject = GameObjectManager.GetGameObjectById(m_strGameObjectID);
            if (l_GameObject != null)
            {
                l_GameObject.SetActive(m_bIsSetActive);
            }
            onComplete();
        }
    }
}