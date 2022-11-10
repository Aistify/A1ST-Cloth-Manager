#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Globalization;
using System.Text.RegularExpressions;

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Main Manager - Stores Data and Functions to create the Cloth Managers
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace A1ST
{
    [Component("A1ST Cloth Manager", "For when laziness takes over.")]
    public class MainManager : MonoBehaviour
    {
        // Data Storage
        [Tooltip("Select the Avatar before clothes were added to it")]
        public GameObject originalAvatar;
        
        [Tooltip("Select any GameObjects to be disabled when new clothes are toggled.")]
        public List<GameObject> originalClothObjects;

        [Tooltip("Select and drag in all the Clothes you merged into the avatar")]
        public List<GameObject> newClothingSets;

        [Tooltip("Select the Avatar with the clothes merged into it")]
        public GameObject mergedAvatar;
        
        [Tooltip("Manage all your Cloth Managers heres")]
        public List<ClothManager> clothManagerList;

        public void CreateClothManagers()
        {
            clothManagerList = new List<ClothManager>();

            // Get Script Root Folder
            string path = new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileName();
            String[] extract = Regex.Split(path, "Assets");
            path = extract[1].TrimEnd('\\');
            path = path.Replace("\\Scripts\\MainManager.cs", "");
            path = "Assets" + path;

            // Get Cloth Manager Prefab
            GameObject clothManagerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(
                path + "\\Prefabs\\Cloth Manager.prefab"
            );
            if (!clothManagerPrefab) Debug.Log("No Cloth Manager Prefab found. Please contact Aist");

            // Deletes old Cloth Managers before Generating new ones
            if (gameObject.transform.childCount > 0)
            {
                Debug.Log("Old Cloth Managers found. Deleting and rebuilding.");
                var oldManagers = new List<GameObject>();
                
                foreach (Transform child in gameObject.transform)
                {
                    oldManagers.Add(child.gameObject);
                }

                foreach (GameObject child in oldManagers)
                {
                    DestroyImmediate(child);
                }
            }
            
            // For each set of clothes ...
            foreach (var clothAvatar in newClothingSets)
            {
                // Instantiate a Cloth Manager Prefab
                GameObject tPrefab = Instantiate(
                    clothManagerPrefab,
                    Vector3.zero,
                    new Quaternion(),
                    gameObject.transform
                );
                
                // Generate a name for it
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                string tName = (clothAvatar.name);
                tName = textInfo.ToTitleCase(tName.ToLower());
                
                // Rename the prefab GameObject
                tPrefab.name = tName + " Manager";
                
                // Set variables once to the components inside the prefab
                tPrefab.GetComponent<SelectionHelper>().clothAvatar = clothAvatar;
                tPrefab.GetComponent<AnimationHelper>().animationName = tName;
                tPrefab.GetComponent<AnimationHelper>().path = path;
                
                // Create a dataset to store information about newly created ClothManager
                ClothManager tClothItem = new ClothManager();
                tClothItem.clothManagerPrefab = tPrefab;
                tClothItem.clothManagerName = tName;
                tClothItem.clothAvatar = clothAvatar;
                
                // Populate Init Lists
                SelectionHelper selectionHelper = tPrefab.GetComponent<SelectionHelper>();
                selectionHelper.GetAllGameObjects();
                selectionHelper.PopulateLists();

                // Add the dataset to the ClothManager List
                clothManagerList.Add(tClothItem);
                EditorGUIUtility.PingObject(tPrefab);
            }
        }
        void OnValidate()
        {
            foreach (var clothManager in clothManagerList)
            {
                clothManager.clothManagerPrefab.name = clothManager.clothManagerName + " Manager";
                clothManager.clothManagerPrefab.GetComponent<AnimationHelper>().animationName =
                    clothManager.clothManagerName;
            }
        }
    }
    
    
}
#endif