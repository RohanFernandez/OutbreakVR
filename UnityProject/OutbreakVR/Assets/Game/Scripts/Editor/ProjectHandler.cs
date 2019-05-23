﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

#if UNITY_EDITOR
namespace ns_Mashmo
{
    [ExecuteInEditMode]
    public class ProjectHandler : EditorWindow
    {
        /// <summary>
        /// The singleton, makes sure there is only a single
        /// </summary>
        private static ProjectHandler s_Instance = null;

        #region SCRIPTABLE OBJECT CREATION

        private const string XML_PATH_TASK_LIST = "Assets\\Game\\GameResources\\TaskListData\\";
        private const string XML_TASK_LIST_LEVEL1_NAME = "TaskListLevel1";
        private const string XML_TASK_LIST_STORE_LOCATION = "Assets\\Game\\Resources\\TaskListAssets\\";
        private const string XML_EXTENSION = ".xml";

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

        #endregion SCRIPTABLE OBJECT CREATION

        #region SEQUENCE EXECUTION



        #endregion SEQUENCE EXECUTION

        #region EDITOR POP UP WINDOW

        [UnityEditor.MenuItem("Outbreak//Project Helper Window")]
        static void OpenPopUpWindow()
        {
            if (s_Instance == null)
            {
                s_Instance = EditorWindow.CreateInstance<ProjectHandler>() as ProjectHandler;
            }
            s_Instance.title = "Project Helper Window";
            s_Instance.Show();
        }

        Rect m_RectSequenceExecute;
        string m_strSequenceToExecute = string.Empty;

        void OnGUI()
        {
            GUILayout.Label("Scriptable Objects", EditorStyles.boldLabel);
            if (GUILayout.Button("Create Task List Scriptable Objects", GUILayout.Width(250)))
            {
                CreateAllScriptableObject();
            }

            GUILayout.Space(10.0f);

            GUILayout.Label("Task List", EditorStyles.boldLabel);
            m_strSequenceToExecute = EditorGUILayout.TextField("Sequence Name:", m_strSequenceToExecute);
            if (GUILayout.Button("Execute Sequence", GUILayout.Width(120)))
            {
                TaskManager.ExecuteSequece(m_strSequenceToExecute);
            }
        }


        #endregion EDITOR POP UP WINDOW
    }



}
#endif