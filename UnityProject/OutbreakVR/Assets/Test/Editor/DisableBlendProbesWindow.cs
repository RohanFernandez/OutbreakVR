using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Outbreak
{
    public class ListWrapper : UnityEngine.Object
    {
        public List<GameObject> listExceptions = new List<GameObject>();
    }
    public class DisableBlendProbesWindow : EditorWindow
    {
        [MenuItem("BlendProbes/Disable Probes")]

        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(DisableBlendProbesWindow));
        }

        ListWrapper listWrapper = new ListWrapper();
        void OnGUI()
        {
            listWrapper = (ListWrapper)EditorGUILayout.ObjectField(listWrapper, listWrapper.GetType(), true);

            if (GUILayout.Button("Disable probes"))
            {
                List<MeshRenderer> meshesInScene = new List<MeshRenderer>();
                MeshRenderer[] meshRenderers = (MeshRenderer[])Resources.FindObjectsOfTypeAll(typeof(MeshRenderer)) ;// FindObjectsOfType<MeshRenderer>();

                foreach (MeshRenderer mr in meshRenderers)
                {
                    if (!EditorUtility.IsPersistent(mr.gameObject.transform.root.gameObject) 
                        && !(mr.gameObject.hideFlags == HideFlags.NotEditable 
                        || mr.gameObject.hideFlags == HideFlags.HideAndDontSave))
                        meshesInScene.Add(mr);
                }

                foreach (var mr in meshesInScene)
                {
                    bool bFound = false;
                    foreach (GameObject g in listWrapper.listExceptions)
                    {
                        if (g == mr.gameObject)
                        {
                            bFound = true;
                            break;
                        }
                    }

                    if (bFound)
                    {
                        continue;
                    }

                    //Undo.RecordObject(mr.gameObject, "Disabled reflection probe");
                    Undo.RegisterCompleteObjectUndo(mr.gameObject, "Disabled reflection probe");
                    Undo.FlushUndoRecordObjects();
                    mr.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
                    mr.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;

                    
                }
            }
        }
    }
}
