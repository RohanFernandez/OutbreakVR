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

        public override void onInitialize()
        {
            base.onInitialize();
            m_ClassType = System.Type.GetType(SystemConsts.NAMESPACE_MASHMO + getString(ATTRIBUTE_CLASS_NAME));
            m_strFunctionName = getString(ATTRIBUTE_FUNCTION_NAME);
            m_strarrArgs = getStrArr(ATTRIBUTE_PARAM, ',');
        }

        public override void onExecute()
        {
            base.onExecute();
            System.Reflection.MethodInfo l_MethodInfo = m_ClassType.GetMethod(m_strFunctionName);
            l_MethodInfo.Invoke(null, m_strarrArgs);
            onComplete();
        }
    }
}