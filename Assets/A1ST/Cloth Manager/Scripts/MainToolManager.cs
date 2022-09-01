#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Globalization;


namespace A1STAndCo
{
    [Component("A1ST Cloth Manager", "For when laziness takes over.")]
    public class MainToolManager : MonoBehaviour
    {
        public GameObject originalAvatar;
        public List<GameObject> clothGameObjects;
        public GameObject mergedAvatar;
        public List<GameObject> originalObjectToDisable;

        public List<ClothManager> clothManagerList;

        public void CreateClothManagers()
        {
            clothManagerList = new List<ClothManager>();

            GameObject clothManagerPrefab =
                AssetDatabase.LoadAssetAtPath<GameObject>("Assets/A1ST Scripts/Prefabs/Cloth Manager.prefab");

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

                    tPrefab.name = tName + " Manager";

                    ClothManager tClothItem = new ClothManager();

                    tClothItem.clothManagerPrefab = tPrefab;
                    tClothItem.clothManagerName = tName;
                    tClothItem.clothAvatar = clothAvatar;


                    clothManagerList.Add(tClothItem);
                }
            }
        }

        public void GenerateAllAnimations()
        {
            foreach (var clothManager in clothManagerList)
            {
                clothManager.clothManagerPrefab.GetComponent<SelectionHelper>().GetAllGameObjects();
                clothManager.clothManagerPrefab.GetComponent<SelectionHelper>().PopulateLists();
                clothManager.clothManagerPrefab.GetComponent<AnimationHelper>().GenerateOnAnimationClip();
                clothManager.clothManagerPrefab.GetComponent<AnimationHelper>().GenerateOffAnimationClip();
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