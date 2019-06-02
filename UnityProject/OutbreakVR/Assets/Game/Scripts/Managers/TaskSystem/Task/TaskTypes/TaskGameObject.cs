using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class TaskGameObject : TaskBase
    {
        #region ATTRIBUTE_KEY
        private const string ATTRIBUTE_GAMEOBJECT_NAME = "GameObject";
        private const string ATTRIBUTE_SET_ACTIVE = "IsActive";
        #endregion ATTRIBUTE_KEY

        private GameObject m_GameObject = null;
        private bool m_bIsSetActive = false;

        public override void onInitialize()
        {
            base.onInitialize();
            m_GameObject = GameObject.Find(getString(ATTRIBUTE_GAMEOBJECT_NAME));
            m_bIsSetActive = getBool(ATTRIBUTE_SET_ACTIVE);
        }

        public override void onExecute()
        {
            base.onExecute();
            if (m_GameObject != null)
            {
                m_GameObject.SetActive(m_bIsSetActive);
            }
            onComplete();
        }
    }
}