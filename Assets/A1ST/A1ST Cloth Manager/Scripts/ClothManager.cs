#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using VRC.Dynamics;
using VRC.SDK3.Dynamics.Contact.Components;
using VRC.SDK3.Dynamics.PhysBone.Components;

namespace A1STCloth
{
    [Component("A1ST Cloth Manager", "Used to hold data to assist other components")]
    public class ClothManager : MonoBehaviour
    {
        [Space(20)]
        [Tooltip("Reference source of data to copy")]
        public GameObject dataSource;

        [Tooltip("Reference to Target Avatar (Original / Cloth) GameObject")]
        public GameObject targetAvatar;

        [Tooltip("Reference to Merged Avatar GameObject")]
        public GameObject mergedAvatar;

        [HideInInspector]
        public List<GameObject> physBones;

        [HideInInspector]
        public List<GameObject> colliders;

        [HideInInspector]
        public List<GameObject> contacts;

        [HideInInspector]
        public List<GameObject> meshes;

        [HideInInspector]
        public bool showPhysBoneList;

        [HideInInspector]
        public bool showCollidersList;

        [HideInInspector]
        public bool showContactsList;

        [HideInInspector]
        public bool showMeshesList;

        public void GetPhysBones(bool replace)
        {
            if (replace)
                physBones = new List<GameObject>();

            List<GameObject> tempMergedList = new List<GameObject>();
            List<GameObject> tempMergedComponents = new List<GameObject>();
            GetAllGameObjectsToList(mergedAvatar, tempMergedList);
            foreach (GameObject gameObj in tempMergedList)
            {
                if (gameObj.GetComponent<VRCPhysBone>())
                {
                    tempMergedComponents.Add(gameObj);
                }
            }

            List<GameObject> tempClothList = new List<GameObject>();
            List<GameObject> tempClothComponents = new List<GameObject>();
            GetAllGameObjectsToList(targetAvatar, tempClothList);
            foreach (GameObject gameObj in tempClothList)
            {
                if (gameObj.GetComponent<VRCPhysBone>())
                {
                    tempClothComponents.Add(gameObj);
                }
            }

            DiffCheckObj MergedvsCloth = new DiffCheckObj();
            MergedvsCloth = FindMatches(tempMergedComponents, tempClothComponents);

            physBones = MergedvsCloth.CommonListProp;
        }

        public void GetColliders(bool replace)
        {
            if (replace)
                colliders = new List<GameObject>();

            List<GameObject> tempMergedList = new List<GameObject>();
            List<GameObject> tempMergedComponents = new List<GameObject>();
            GetAllGameObjectsToList(mergedAvatar, tempMergedList);
            foreach (GameObject gameObj in tempMergedList)
            {
                if (gameObj.GetComponent<VRCPhysBoneCollider>())
                {
                    tempMergedComponents.Add(gameObj);
                }
            }

            List<GameObject> tempClothList = new List<GameObject>();
            List<GameObject> tempClothComponents = new List<GameObject>();
            GetAllGameObjectsToList(targetAvatar, tempClothList);
            foreach (GameObject gameObj in tempClothList)
            {
                if (gameObj.GetComponent<VRCPhysBoneCollider>())
                {
                    tempClothComponents.Add(gameObj);
                }
            }

            DiffCheckObj MergedvsCloth = new DiffCheckObj();
            MergedvsCloth = FindMatches(tempMergedComponents, tempClothComponents);

            colliders = MergedvsCloth.CommonListProp;
        }

        public void GetContacts(bool replace)
        {
            if (replace)
                contacts = new List<GameObject>();

            List<GameObject> tempMergedList = new List<GameObject>();
            List<GameObject> tempMergedComponents = new List<GameObject>();
            GetAllGameObjectsToList(mergedAvatar, tempMergedList);
            foreach (GameObject gameObj in tempMergedList)
            {
                if (
                    gameObj.GetComponent<VRCContactReceiver>()
                    || gameObj.GetComponent<VRCContactSender>()
                )
                {
                    tempMergedComponents.Add(gameObj);
                }
            }

            List<GameObject> tempClothList = new List<GameObject>();
            List<GameObject> tempClothComponents = new List<GameObject>();
            GetAllGameObjectsToList(targetAvatar, tempClothList);
            foreach (GameObject gameObj in tempClothList)
            {
                if (
                    gameObj.GetComponent<VRCContactReceiver>()
                    || gameObj.GetComponent<VRCContactSender>()
                )
                {
                    tempClothComponents.Add(gameObj);
                }
            }

            DiffCheckObj MergedvsCloth = new DiffCheckObj();
            MergedvsCloth = FindMatches(tempMergedComponents, tempClothComponents);

            contacts = MergedvsCloth.CommonListProp;
        }

        public void GetMeshes(bool replace)
        {
            if (replace)
                meshes = new List<GameObject>();

            List<GameObject> tempMergedList = new List<GameObject>();
            List<GameObject> tempMergedComponents = new List<GameObject>();
            GetAllGameObjectsToList(mergedAvatar, tempMergedList);
            foreach (GameObject gameObj in tempMergedList)
            {
                if (
                    gameObj.GetComponent<SkinnedMeshRenderer>()
                    || gameObj.GetComponent<MeshRenderer>()
                )
                {
                    tempMergedComponents.Add(gameObj);
                }
            }

            List<GameObject> tempClothList = new List<GameObject>();
            List<GameObject> tempClothComponents = new List<GameObject>();
            GetAllGameObjectsToList(targetAvatar, tempClothList);
            foreach (GameObject gameObj in tempClothList)
            {
                if (
                    gameObj.GetComponent<SkinnedMeshRenderer>()
                    || gameObj.GetComponent<MeshRenderer>()
                )
                {
                    tempClothComponents.Add(gameObj);
                }
            }

            DiffCheckObj MergedvsCloth = new DiffCheckObj();
            MergedvsCloth = FindMatches(tempMergedComponents, tempClothComponents);

            meshes = MergedvsCloth.CommonListProp;
        }

        public void SelectionToList(List<GameObject> targetList, Type targetType, bool replace)
        {
            if (replace)
            {
                targetList.Clear();
            }

            List<GameObject> temp = new List<GameObject>();
            foreach (GameObject gameObj in Selection.gameObjects)
            {
                if (gameObj.GetComponent(targetType) != null)
                    temp.Add(gameObj);
            }
            targetList.AddRange(temp);
        }

        public void ToggleObjects(bool toggle)
        {
            List<GameObject> toggleList = new List<GameObject>();

            toggleList.AddRange(physBones);
            toggleList.AddRange(colliders);
            toggleList.AddRange(contacts);
            toggleList.AddRange(meshes);

            foreach (GameObject gameObj in toggleList)
            {
                gameObj.SetActive(toggle);
            }
        }

        public void CreateClothAnimator()
        {
            GameObject clothAnimatorGameObj = new GameObject();
            clothAnimatorGameObj.transform.parent = gameObject.transform;
            clothAnimatorGameObj.name =
                gameObject.name + " Variant " + gameObject.transform.childCount;

            ClothAnimator clothAnimator = clothAnimatorGameObj.AddComponent<ClothAnimator>();
            clothAnimator.animationName = clothAnimatorGameObj.name;
            clothAnimator.whitelistExceptions = new List<GameObject>();
            clothAnimator.blacklistExceptions = new List<GameObject>();

            clothAnimator.clothManagerRef = gameObject.GetComponent<ClothManager>();
            Transform originalClothManagerGameObject =
                gameObject.transform.parent.gameObject.transform.Find("Original Clothes Manager");
            clothAnimator.originalClothManagerRef =
                originalClothManagerGameObject.GetComponent<ClothManager>();

            EditorGUIUtility.PingObject(clothAnimatorGameObj);
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

        private class DiffCheckObj
        {
            public List<GameObject> CommonListProp { get; set; }
            public List<GameObject> DifferenceListProp { get; set; }
        }

        private static DiffCheckObj FindMatches(
            List<GameObject> listToSearchIn,
            List<GameObject> listToFind
        )
        {
            // Init temp Lists
            var commonItemsL = new List<GameObject>();
            var differenceItemsL = new List<GameObject>();

            // For each GameObject in the list to search
            foreach (var searchObj in listToSearchIn)
            {
                // Init bool
                var matchFound = false;

                // Find a GameObject with the same name from the list to find
                foreach (var findObj in listToFind)
                    if (findObj.name == searchObj.name)
                    {
                        // When found add the GameObject to the common List
                        matchFound = true;
                        commonItemsL.Add(searchObj);
                    }

                // When no matches are found for a GameObject, it is added to the difference List
                if (matchFound == false)
                    differenceItemsL.Add(searchObj);
            }

            // Returns a class object containing both common and difference list
            return new DiffCheckObj
            {
                CommonListProp = commonItemsL,
                DifferenceListProp = differenceItemsL
            };
        }
    }
}

#endif
