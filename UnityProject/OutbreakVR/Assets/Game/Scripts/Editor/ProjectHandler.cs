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


        /// <summary>
        /// Task List
        /// </summary>
        private const string XML_PATH_TASK_LIST        = "Assets\\Game\\GameResources\\TaskListData\\";
        private const string XML_TASK_LIST_COMMON_NAME = "TaskListCommon";
        private const string XML_TASK_LIST_LEVEL00_NAME = "TaskListLevel00";
        private const string XML_TASK_LIST_LEVEL0_NAME = "TaskListLevel0";
        private const string XML_TASK_LIST_LEVEL1_NAME = "TaskListLevel1";
        private const string XML_TASK_LIST_TRAINING_NAME = "TaskListTraining";
        private const string XML_TASK_LIST_TESTING1_NAME = "TaskListTesting1";
        
        private const string XML_TASK_LIST_STORE_LOCATION = "Assets\\Game\\Resources\\TaskListAssets\\";

        /// <summary>
        /// Objective List
        /// </summary>
        private const string XML_PATH_OBJECTIVE_LIST    = "Assets\\Game\\GameResources\\ObjectiveListData\\";
        private const string XML_OBJECTIVE_LIST_LEVEL00_NAME = "ObjectiveListLevel00";
        private const string XML_OBJECTIVE_LIST_LEVEL0_NAME = "ObjectiveListLevel0";
        private const string XML_OBJECTIVE_LIST_LEVEL1_NAME = "ObjectiveListLevel1";
        private const string XML_OBJECTIVE_LIST_TRAINING_NAME = "ObjectiveListTraining";
        private const string XML_OBJECTIVE_LIST_TESTING1_NAME = "ObjectiveListTesting1";
        private const string XML_OBJECTIVE_LIST_STORE_LOCATION = "Assets\\Game\\Resources\\ObjectiveListAssets\\";

        private const string XML_EXTENSION = ".xml";

        static void CreateAllScriptableObject()
        {
            ///Task list creation
            CreateTaskListScriptableObject(XML_TASK_LIST_LEVEL00_NAME);
            CreateTaskListScriptableObject(XML_TASK_LIST_LEVEL0_NAME);
            CreateTaskListScriptableObject(XML_TASK_LIST_LEVEL1_NAME);
            CreateTaskListScriptableObject(XML_TASK_LIST_TRAINING_NAME);
            CreateTaskListScriptableObject(XML_TASK_LIST_COMMON_NAME);

            ///Objective list creation
            CreateObjectiveListScriptableObject(XML_OBJECTIVE_LIST_LEVEL00_NAME);
            CreateObjectiveListScriptableObject(XML_OBJECTIVE_LIST_LEVEL0_NAME);
            CreateObjectiveListScriptableObject(XML_OBJECTIVE_LIST_LEVEL1_NAME);
            CreateObjectiveListScriptableObject(XML_OBJECTIVE_LIST_TRAINING_NAME);

            //DEBUG
            CreateTaskListScriptableObject(XML_TASK_LIST_TESTING1_NAME);
            CreateObjectiveListScriptableObject(XML_OBJECTIVE_LIST_TESTING1_NAME);
        }

        /// <summary>
        /// Creates task list asset with given name and stores it into the location
        /// </summary>
        /// <param name="a_strAssetNameToSave"></param>
        static void CreateTaskListScriptableObject(string a_strAssetNameToSave)
        {
            UnityEditor.AssetDatabase.CreateAsset(TaskList.GetTaskListFromXML(XML_PATH_TASK_LIST + a_strAssetNameToSave + XML_EXTENSION), XML_TASK_LIST_STORE_LOCATION + a_strAssetNameToSave + ".asset");
            UnityEditor.AssetDatabase.SaveAssets();
        }

        /// <summary>
        /// Creates objective list asset with given name and stores it into the location
        /// </summary>
        /// <param name="a_strAssetNameToSave"></param>
        static void CreateObjectiveListScriptableObject(string a_strAssetNameToSave)
        {
            UnityEditor.AssetDatabase.CreateAsset(ObjectiveList.GetObjectiveList(XML_PATH_OBJECTIVE_LIST + a_strAssetNameToSave + XML_EXTENSION), XML_OBJECTIVE_LIST_STORE_LOCATION + a_strAssetNameToSave + ".asset");
            UnityEditor.AssetDatabase.SaveAssets();
        }

        #endregion SCRIPTABLE OBJECT CREATION

        #region SEQUENCE EXECUTION



        #endregion SEQUENCE EXECUTION

        #region EDITOR POP UP WINDOW

        [UnityEditor.MenuItem("Outbreak//Project Helper Window")]
        private static void OpenPopUpWindow()
        {
            if (s_Instance == null)
            {
                s_Instance = (ProjectHandler)EditorWindow.GetWindow(typeof(ProjectHandler));//EditorWindow.CreateInstance<ProjectHandler>() as ProjectHandler;
                s_Instance.title = "Project Helper Window";
                s_Instance.Show();
            }
        }

        Rect m_RectSequenceExecute;
        string m_strSequenceToExecute = string.Empty;
        string m_strLevelToTransition = string.Empty;

        void OnGUI()
        {
            GUILayout.Label("Level", EditorStyles.boldLabel);
            m_strLevelToTransition = EditorGUILayout.TextField("Level Name:", m_strLevelToTransition);
            if (GUILayout.Button("Transition", GUILayout.Width(120)))
            {
                LevelManager.GoToLevel(m_strLevelToTransition);
            }

            GUILayout.Space(10.0f);

            GUILayout.Label("Scriptable Objects", EditorStyles.boldLabel);
            if (GUILayout.Button("Create All Scriptable Objects", GUILayout.Width(250)))
            {
                CreateAllScriptableObject();
            }

            GUILayout.Space(10.0f);

            GUILayout.Label("Task List", EditorStyles.boldLabel);
            m_strSequenceToExecute = EditorGUILayout.TextField("Sequence Name:", m_strSequenceToExecute);
            if (GUILayout.Button("Execute Sequence", GUILayout.Width(120)))
            {
                TaskManager.ExecuteSequence(m_strSequenceToExecute);
            }
            if (GUILayout.Button("Stop Sequence", GUILayout.Width(120)))
            {
                TaskManager.StopSequence(m_strSequenceToExecute);
            }
            if (GUILayout.Button("Stop All", GUILayout.Width(120)))
            {
                TaskManager.StopAll();
            }

            GUILayout.Space(10.0f);

            GUILayout.Label("LOGGER", EditorStyles.boldLabel);
            if (GUILayout.Button("Log Running Tasks", GUILayout.Width(120)))
            {
                TaskManager.LogRunningSequences();
            }
        }

        #endregion EDITOR POP UP WINDOW
    }
}
#endif