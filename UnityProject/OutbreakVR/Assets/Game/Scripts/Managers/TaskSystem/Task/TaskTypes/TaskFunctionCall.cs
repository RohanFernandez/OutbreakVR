using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    public class TaskFunctionCall : TaskBase
    {
        #region ATTRIBUTE_KEY
        private const string ATTRIBUTE_CLASS_NAME = "ClassName";
        private const string ATTRIBUTE_PARAM = "Parameters";
        private const string ATTRIBUTE_GAMEOBJ_ID = "GameObject_ID";
        private const string ATTRIBUTE_FUNCTION_NAME = "FunctionName";
        #endregion ATTRIBUTE_KEY

        /// <summary>
        /// Type of the class for which to call the function
        /// </summary>
        private System.Type m_ClassType = null;

        /// <summary>
        /// Name of the static function to call
        /// </summary>
        private string m_strFunctionName = string.Empty;

        /// <summary>
        /// The string ',' separated args array
        /// </summary>
        private string[] m_strarrArgs = null;

        /// <summary>
        /// The gameobject id which holds the component which includes the function
        /// </summary>
        private string m_strGameObjectID = string.Empty;

        /// <summary>
        /// The gameobject reference that will call the function
        /// </summary>
        private GameObject m_GameObjectRef = null;

        public override void onInitialize()
        {
            base.onInitialize();
            m_ClassType = System.Type.GetType(SystemConsts.NAMESPACE_MASHMO + getString(ATTRIBUTE_CLASS_NAME));
            m_strFunctionName = getString(ATTRIBUTE_FUNCTION_NAME);
            m_strarrArgs = getStrArr(ATTRIBUTE_PARAM, ',');
            m_strGameObjectID = getString(ATTRIBUTE_GAMEOBJ_ID);
            m_GameObjectRef = string.IsNullOrEmpty(m_strGameObjectID) ? null : GameObjectManager.GetGameObjectById(m_strGameObjectID);
        }

        public override void onExecute()
        {
            base.onExecute();
            System.Reflection.MethodInfo l_MethodInfo = m_ClassType.GetMethod(m_strFunctionName);
            Object l_refObj = null;
            if (m_GameObjectRef != null)
            {
                l_refObj = m_GameObjectRef.GetComponent(m_ClassType);
                if (l_refObj == null)
                {
                    Debug.LogError("TaskFunctionCall::onExecute:: Component of type '"+ getString(ATTRIBUTE_CLASS_NAME+"' is not attached to the gameobject."));
                    onComplete();
                }
            }
            l_MethodInfo.Invoke(l_refObj, m_strarrArgs);
            onComplete();
        }
    }
}