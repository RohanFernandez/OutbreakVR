﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
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
        private const string XML_TASK_LIST_STORE_LOCATION = "Assets\\Game\\Resources\\TaskListAssets\\";

        /// <summary>
        /// Objective List
        /// </summary>
        private const string XML_PATH_OBJECTIVE_LIST    = "Assets\\Game\\GameResources\\ObjectiveListData\\";
        private const string XML_OBJECTIVE_LIST_STORE_LOCATION = "Assets\\Game\\Resources\\ObjectiveListAssets\\";

        private const string XML_EXTENSION = ".xml";

        public enum ASSET_TYPE
        { 
            OBJECTIVE_LIST  = 0,
            TASK_LIST       = 1
        }

        static void CreateAllScriptableObject()
        {
            CreateAssetsOfType(XML_PATH_TASK_LIST, ASSET_TYPE.TASK_LIST);
            CreateAssetsOfType(XML_PATH_OBJECTIVE_LIST, ASSET_TYPE.OBJECTIVE_LIST);
        }

        /// <summary>
        /// Creates assets from the given XML folder
        /// </summary>
        /// <param name="a_strXMLPath"></param>
        /// <param name="a_AssetType"></param>
        private static void CreateAssetsOfType(string a_strXMLPath, ASSET_TYPE a_AssetType)
        {
            DirectoryInfo l_info = new DirectoryInfo(a_strXMLPath);
            FileInfo[] l_fileInfo = l_info.GetFiles();
            foreach (FileInfo l_file in l_fileInfo)
            {
                if (l_file.FullName.Contains(".xml.meta"))
                {
                    continue;
                }
                else if (l_file.FullName.Contains(".xml"))
                {
                    string l_strFileName = l_file.Name.Remove(l_file.Name.Length - XML_EXTENSION.Length, XML_EXTENSION.Length);

                    switch (a_AssetType)
                    {
                        case ASSET_TYPE.OBJECTIVE_LIST:
                            {
                                CreateObjectiveListScriptableObject(l_file.FullName, l_strFileName);
                                break;
                            }
                        case ASSET_TYPE.TASK_LIST:
                            {
                                CreateTaskListScriptableObject(l_file.FullName, l_strFileName);
                                break;
                            }
                    }
                }
                else
                {
                    Debug.LogError("ProjectHandler::CreateAllScriptableObject:: Cannot create scriptable object with file with name at location '" + l_file.FullName + "'");
                }
            }
        }

        /// <summary>
        /// Creates task list asset with given name and stores it into the location
        /// </summary>
        /// <param name="a_strAssetNameToSave"></param>
        static void CreateTaskListScriptableObject(string a_strDataPathName, string a_strAssetName)
        {
            UnityEditor.AssetDatabase.CreateAsset(TaskList.GetTaskListFromXML(a_strDataPathName), XML_TASK_LIST_STORE_LOCATION + a_strAssetName + ".asset");
            UnityEditor.AssetDatabase.SaveAssets();
        }

        /// <summary>
        /// Creates objective list asset with given name and stores it into the location
        /// </summary>
        /// <param name="a_strAssetNameToSave"></param>
        static void CreateObjectiveListScriptableObject(string a_strDataPathName, string a_strAssetName)
        {
            UnityEditor.AssetDatabase.CreateAsset(ObjectiveList.GetObjectiveList(a_strDataPathName), XML_OBJECTIVE_LIST_STORE_LOCATION + a_strAssetName + ".asset");
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
        string m_strTriggerObjective = string.Empty;

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
            if (GUILayout.Button("Log Tasks", GUILayout.Width(120)))
            {
                TaskManager.LogRunningSequences();
            }

            GUILayout.Space(10.0f);

            GUILayout.Label("ObjectiveList", EditorStyles.boldLabel);
            m_strTriggerObjective = EditorGUILayout.TextField("Objective ID:", m_strTriggerObjective);
            if (GUILayout.Button("Trigger Objective", GUILayout.Width(120)))
            {
                ObjectiveManager.TriggerObjective(m_strTriggerObjective);
            }
            if (GUILayout.Button("Log Objectives", GUILayout.Width(120)))
            {
                ObjectiveManager.LogRunningObjectives();
            }

            GUILayout.Space(10.0f);
        }

        #endregion EDITOR POP UP WINDOW
    }
}
#endif