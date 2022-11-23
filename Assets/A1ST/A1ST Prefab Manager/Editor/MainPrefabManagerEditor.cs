#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace A1STPrefab
{
    [CustomEditor(typeof(MainPrefabManager))]
    public class MainPrefabManagerEditor : BaseEditor
    {
        public override void OnInspectorGUI()
        {
            MainPrefabManager mainPrefabManager = (MainPrefabManager)target;
            base.OnInspectorGUI();

            List<PrefabStruct> prefabList = mainPrefabManager.prefabs;

            EditorGUILayout.Space(10);

            EditorGUILayout.LabelField("Prefabs", EditorStyles.whiteLabel);

            if (prefabList == null || prefabList.Count == 0)
            {
                EditorGUILayout.LabelField(
                    "No prefabs inside " + mainPrefabManager.gameObject.name
                );
            }
            else
            {
                for (int i = 0; i < prefabList.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();

                    EditorGUILayout.LabelField(prefabList[i].prefabName);
                    prefabList[i].prefabName = EditorGUILayout.TextField(prefabList[i].prefabName);
                    prefabList[i].prefab =
                        EditorGUILayout.ObjectField(prefabList[i].prefab, typeof(GameObject), true)
                        as GameObject;

                    EditorGUILayout.EndHorizontal();
                }
            }

            EditorGUILayout.Space(20);

            if (GUILayout.Button("Update Prefabs List"))
                mainPrefabManager.UpdateList();

            EditorGUILayout.Space(5);

            if (GUILayout.Button("Rename Prefab Names"))
                mainPrefabManager.RenamePrefabs();

            if (GUILayout.Button("Rename Meshes"))
                mainPrefabManager.RenameMeshes();

            if (GUILayout.Button("Manage Animators"))
                mainPrefabManager.ManageAnimator();

            if (isA1STNamespace)
                LogoGUI();
        }
    }
}
#endif
