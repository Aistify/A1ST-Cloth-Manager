#if UNITY_EDITOR

using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Main Manager - Stores Data and Functions to create the Cloth Managers
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace A1ST
{
    [Component("A1ST Cloth Manager", "For when laziness takes over.")]
    public class MainManager : MonoBehaviour
    {
        // Data Storage
        [Space(20)]
        [Tooltip("Select the Avatar before clothes were added to it")]
        public GameObject originalAvatar;

        [Space(10)]
        [Tooltip("Select any GameObjects to be disabled when new clothes are toggled.")]
        public List<GameObject> objectsToDisable;

        [Space(10)]
        [Tooltip("Select and drag in all the Clothes you merged into the avatar")]
        public List<GameObject> clothingPrefabs;

        [Space(10)]
        [Tooltip("Select the Avatar with the clothes merged into it")]
        public GameObject mergedAvatar;

        [Space(10)]
        [Tooltip("Manage all your Cloth Managers heres")]
        public List<ClothManager> clothManagerList;

        private void OnValidate()
        {
            foreach (var clothManager in clothManagerList)
            {
                clothManager.clothManagerPrefab.name = clothManager.clothManagerName + " Manager";
                clothManager.clothManagerPrefab.GetComponent<AnimationHelper>().animationName =
                    clothManager.clothManagerName;
            }
        }

        public void CreateClothManagers()
        {
            // Get Script Root Folder
            var path = new StackTrace(true).GetFrame(0).GetFileName();
            var extract = Regex.Split(path, "Assets");
            path = extract[1].TrimEnd('\\');
            path = path.Replace("\\Scripts\\MainManager.cs", "");
            path = "Assets" + path;

            // Get Cloth Manager Prefab
            var clothManagerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(
                path + "\\Prefabs\\Cloth Manager.prefab"
            );
            if (!clothManagerPrefab)
                Debug.Log("No Cloth Manager Prefab found. Please contact Aist");

            // Deletes old Cloth Managers before Generating new ones
            if (gameObject.transform.childCount > 0)
            {
                Debug.Log("Old Cloth Managers found. Renaming and rebuilding.");
                var oldManagers = new List<GameObject>();

                foreach (Transform child in gameObject.transform)
                    oldManagers.Add(child.gameObject);

                foreach (var child in oldManagers)
                    child.name = child.name + "__Old";
            }

            // For each set of clothes ...
            foreach (var clothAvatar in clothingPrefabs)
            {
                // Instantiate a Cloth Manager Prefab
                var tPrefab = Instantiate(
                    clothManagerPrefab,
                    Vector3.zero,
                    new Quaternion(),
                    gameObject.transform
                );

                // Generate a name for it
                var textInfo = new CultureInfo("en-US", false).TextInfo;
                var tName = clothAvatar.name;
                tName = textInfo.ToTitleCase(tName.ToLower());

                // Rename the prefab GameObject
                tPrefab.name = tName + " Manager";

                // Set variables once to the components inside the prefab
                tPrefab.GetComponent<SelectionHelper>().clothAvatar = clothAvatar;
                tPrefab.GetComponent<AnimationHelper>().animationName = tName;
                tPrefab.GetComponent<AnimationHelper>().path = path;

                // Create a dataset to store information about newly created ClothManager
                var tClothItem = new ClothManager();
                tClothItem.clothManagerPrefab = tPrefab;
                tClothItem.clothManagerName = tName;
                tClothItem.clothAvatar = clothAvatar;

                // Populate Init Lists
                var selectionHelper = tPrefab.GetComponent<SelectionHelper>();
                selectionHelper.GetAllGameObjects();
                selectionHelper.PopulateLists();

                // Add the dataset to the ClothManager List
                clothManagerList.Add(tClothItem);
                EditorGUIUtility.PingObject(tPrefab);
            }
        }

        public void GenerateAllAnimations()
        {
            foreach (var clothManager in clothManagerList)
            {
                clothManager.clothManagerPrefab
                    .GetComponent<AnimationHelper>()
                    .GenerateAnimationClip(true);
                clothManager.clothManagerPrefab
                    .GetComponent<AnimationHelper>()
                    .GenerateAnimationClip(false);
            }
        }
    }
}
#endif
