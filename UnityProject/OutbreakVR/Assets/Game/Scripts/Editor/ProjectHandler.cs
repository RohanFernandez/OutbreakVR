using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns_Mashmo
{
    [ExecuteInEditMode]
    public class ProjectHandler
    {
#if UNITY_EDITOR
        private const string XML_PATH_TASK_LIST = "Assets\\Game\\GameResources\\TaskListData\\";
        private const string XML_TASK_LIST_LEVEL1_NAME = "TaskListLevel1";
        private const string XML_TASK_LIST_STORE_LOCATION = "Assets\\Game\\Resources\\TaskListAssets\\";
        private const string XML_EXTENSION = ".xml";

        /// <summary>
        /// Creates All the scriptable objects
        /// </summary>
        [UnityEditor.MenuItem("Outbreak/Create all scriptable object")]
        static void CreateAllScriptableObject()
        {
            CreateTaskListScriptableObject(XML_TASK_LIST_LEVEL1_NAME);
        }

        /// <summary>
        /// Creates task list asset with given name and stores it into the location
        /// </summary>
        /// <param name="a_strAssetNameToSave"></param>
        static void CreateTaskListScriptableObject(string a_strAssetNameToSave)
        {
            UnityEditor.AssetDatabase.CreateAsset(TaskList.GetParsedXML(XML_PATH_TASK_LIST + a_strAssetNameToSave + XML_EXTENSION), XML_TASK_LIST_STORE_LOCATION + a_strAssetNameToSave + ".asset");
            UnityEditor.AssetDatabase.SaveAssets();
        }
    }
#endif
}