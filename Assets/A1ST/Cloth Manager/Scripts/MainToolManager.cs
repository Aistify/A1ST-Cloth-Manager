#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Globalization;
using System.Text.RegularExpressions;


namespace A1STAndCo
{
    [Component("A1ST Cloth Manager", "For when laziness takes over.")]
    public class MainToolManager : MonoBehaviour
    {
        [Tooltip("Select the Avatar before clothes were added to it")]
        public GameObject originalAvatar;
        
        [Tooltip("Select all the Clothes you want to manage")]
        public List<GameObject> clothGameObjects;
        
        [Tooltip("Select the Avatar with the clothes merged into it")]
        public GameObject mergedAvatar;
        
        [Tooltip("Select any GameObjects to be disabled when clothes are toggled.")]    
        public List<GameObject> originalObjectToDisable;

        [Tooltip("Manage all your Cloth Managers heres")]    
        public List<ClothManager> clothManagerList;

        [HideInInspector] public string mainAssetPath;
        
        public void CreateClothManagers()
        {
            clothManagerList = new List<ClothManager>();

            string getPath = new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileName();
            String[] extract = Regex.Split(getPath,"Assets");
            mainAssetPath = extract[1].TrimEnd('\\');
            mainAssetPath = mainAssetPath.Replace("\\Scripts\\MainToolManager.cs", "");
            mainAssetPath = "Assets" + mainAssetPath;
            
            print(mainAssetPath + "\\Prefabs\\Cloth Manager.prefab");

            GameObject clothManagerPrefab =
                AssetDatabase.LoadAssetAtPath<GameObject>(mainAssetPath + "\\Prefabs\\Cloth Manager.prefab");

            foreach (var clothAvatar in clothGameObjects)
            {
                Boolean exists = new Boolean();
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                string tName = (clothAvatar.name);
                tName = textInfo.ToTitleCase(tName.ToLower());

                foreach (Transform child in gameObject.transform)
                {
                    if (child.name == tName + " Manager") exists = true;
                }

                if (!exists)
                {
                    GameObject tPrefab = Instantiate(clothManagerPrefab,
                        Vector3.zero, new Quaternion(), gameObject.transform);

                    tPrefab.GetComponent<SelectionHelper>().originalAvatar = originalAvatar;
                    tPrefab.GetComponent<SelectionHelper>().clothAvatar = clothAvatar;
                    tPrefab.GetComponent<SelectionHelper>().mergedAvatar = mergedAvatar;
                    tPrefab.GetComponent<SelectionHelper>().originalObjectsToDisable = originalObjectToDisable;
                    tPrefab.GetComponent<AnimationHelper>().animationName = tName;
                    tPrefab.GetComponent<AnimationHelper>().mainAssetPath = mainAssetPath;

                    tPrefab.name = tName + " Manager";

                    ClothManager tClothItem = new ClothManager();

                    tClothItem.clothManagerPrefab = tPrefab;
                    tClothItem.clothManagerName = tName;
                    tClothItem.clothAvatar = clothAvatar;
                    
                    clothManagerList.Add(tClothItem);
                }
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