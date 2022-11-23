#if UNITY_EDITOR

using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace A1STCloth
{
    [Component("A1ST Main Cloth Manager", "Used to manage and hold all other Cloth Managers")]
    public class MainClothManager : MonoBehaviour
    {
        [Space(10)] public GameObject originalAvatar;

        public List<GameObject> clothPrefabs;

        public GameObject mergedAvatar;

        [HideInInspector] public List<ClothManagerStruct> clothManagers;

        [HideInInspector] public bool showManagerList;

        public void CreateNewManagers()
        {
            if (clothPrefabs.Count == 0)
            {
                print("A1ST || Error : No Cloth Prefabs Found");
                return;
            }

            if (transform.Find("Original Clothes Manager") == null)
            {
                GenerateClothManager(originalAvatar, gameObject);
                var originalClothManager = transform
                    .Find(originalAvatar.name + " Manager")
                    .gameObject;
                originalClothManager.GetComponent<ClothManager>().name = "Original Clothes Manager";
            }

            foreach (var clothPrefab in clothPrefabs)
            {
                if (transform.Find(clothPrefab.name + " Manager") != null)
                    return;

                GenerateClothManager(clothPrefab, gameObject);
            }

            UpdateList();
        }

        public void GenerateClothManager(GameObject target, GameObject parent)
        {
            var rootPath = GetScriptRoot();
            var clothManagerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(
                rootPath + "\\Prefabs\\ClothManagerPrefab ( For Manual Setup ).prefab"
            );

            var tCManager = Instantiate(
                clothManagerPrefab,
                Vector3.zero,
                new Quaternion(),
                parent.transform
            );

            var textInfo = new CultureInfo("en-US", false).TextInfo;
            var tName = target.name;
            tCManager.name = tName + " Manager";

            var tclothManager = tCManager.GetComponent<ClothManager>();
            tclothManager.targetAvatar = target;
            tclothManager.mergedAvatar = mergedAvatar;
            tclothManager.GetPhysBones(true);
            tclothManager.GetColliders(true);
            tclothManager.GetContacts(true);
            tclothManager.GetMeshes(true);

            if (tclothManager.name != originalAvatar.name + " Manager")
                tclothManager.ToggleObjects(false);
            else
                tclothManager.ToggleObjects(true);


            EditorGUIUtility.PingObject(tCManager);
        }

        public void UpdateList()
        {
            if (gameObject.transform.childCount == 0)
            {
                print("A1ST || Error : No child objects under Main Cloth Manager GameObject");
                return;
            }

            var tempL = new List<GameObject>();
            GetAllGameObjectsToList(gameObject, tempL);

            clothManagers = new List<ClothManagerStruct>();
            foreach (var gameObj in tempL)
            {
                if (gameObj.GetComponent<ClothManager>() == null)
                    return;

                var temp = new ClothManagerStruct();
                temp.clothManager = gameObj.gameObject;
                temp.clothManagerName = gameObj.name;
                temp.clothPrefab = gameObj.GetComponent<ClothManager>().targetAvatar;

                clothManagers.Add(temp);
            }
        }

        private void GetAllGameObjectsToList(GameObject obj, List<GameObject> list)
        {
            // If no GameObject or List is passed in, cancel the command and return an error message
            if (obj == null || list == null)
            {
                Debug.Log("No GameObject or List passed in to process");
                return;
            }

            // For each Child GameObject within the Parent
            foreach (Transform child in obj.transform)
            {
                // If there is no Child GameObject, return
                if (null == child)
                    continue;

                // Else, add the Child GameObject to the passed in List
                list.Add(child.gameObject);

                // And search the Child GameObject if it has any children of it's own
                GetAllGameObjectsToList(child.gameObject, list);
            }
        }

        private string GetScriptRoot()
        {
            var path = new StackTrace(true).GetFrame(0).GetFileName();
            var extract = Regex.Split(path, "Assets");
            path = extract[1].TrimEnd('\\');
            path = path.Replace("\\Scripts\\MainClothManager.cs", "");
            path = "Assets" + path;
            return path;
        }
    }
}

#endif