#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace A1STCloth
{
    [CustomEditor(typeof(MainClothManager))]
    public class MainClothManagerEditor : BaseClothEditor
    {
        public override void OnInspectorGUI()
        {
            MainClothManager mainClothManager = (MainClothManager)target;
            base.OnInspectorGUI();

            List<ClothManagerStruct> prefabList = mainClothManager.clothManagers;

            EditorGUILayout.Space(10);

            mainClothManager.showManagerList = EditorGUILayout.Foldout(
                mainClothManager.showManagerList,
                "Cloth Managers List",
                true
            );

            if (mainClothManager.showManagerList)
            {
                for (int i = 0; i < prefabList.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();

                    EditorGUILayout.LabelField(prefabList[i].clothManagerName);

                    prefabList[i].clothManagerName = EditorGUILayout.TextField(
                        prefabList[i].clothManagerName
                    );

                    prefabList[i].clothManager =
                        EditorGUILayout.ObjectField(
                            prefabList[i].clothManager,
                            typeof(GameObject),
                            true
                        ) as GameObject;

                    prefabList[i].clothPrefab =
                        EditorGUILayout.ObjectField(
                            prefabList[i].clothPrefab,
                            typeof(GameObject),
                            true
                        ) as GameObject;

                    EditorGUILayout.EndHorizontal();
                }
            }

            EditorGUILayout.Space(20);

            if (GUILayout.Button("Create Cloth Managers from Prefabs List"))
            {
                mainClothManager.CreateNewManagers();
                mainClothManager.UpdateList();
            }

            if (GUILayout.Button("Refresh Cloth Managers List"))
                mainClothManager.UpdateList();

            EditorGUILayout.Space(5);

            if (isA1STNamespace)
                LogoGUI();
        }
    }
}
#endif
