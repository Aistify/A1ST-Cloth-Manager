#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using VRC.SDK3.Dynamics.PhysBone.Components;

namespace A1ST
{
    [Component("Selection Helper", "Helps make selections of clothes and bones to later animate.")]
    public class SelectionHelper : MonoBehaviour
    {
        [Tooltip("Reference to clothing GameObject")]
        public GameObject clothAvatar;

        [Tooltip("Overview of the GameObjects that will be toggled off. (Set Via Main Manager)")]
        public List<GameObject> originalClothes;

        [Tooltip("Overview of GameObjects that will be toggled on.")]
        public List<GameObject> clothingSelection;

        [Tooltip("Add any additional GameObjects to be toggled off.")]
        public List<GameObject> extraToDisable;

        [HideInInspector]
        public List<GameObject> objectsToDisable;

        private List<GameObject> _clothObjectsList;
        private List<GameObject> _mergedObjectsList;

        private List<GameObject> _originalObjectsList;

        public void GetAllGameObjects()
        {
            if (transform.parent.gameObject.GetComponent<MainManager>() == null)
            {
                print("ERR: NO MAIN MANAGER FOUND");
                return;
            }
            // Init Lists
            _originalObjectsList = new List<GameObject>();
            _clothObjectsList = new List<GameObject>();
            _mergedObjectsList = new List<GameObject>();
            objectsToDisable = new List<GameObject>();

            // Populate Lists
            MainManager mainManager = transform.parent.gameObject.GetComponent<MainManager>();
            GetAllGameObjectsToList(mainManager.originalAvatar, _originalObjectsList);
            GetAllGameObjectsToList(mainManager.mergedAvatar, _mergedObjectsList);

            originalClothes = new List<GameObject>();
            List<GameObject> temp = new List<GameObject>();
            temp = mainManager.objectsToDisable;
            var MergedVsOriginal = new DiffCheckObj();
            MergedVsOriginal = FindMatches(_mergedObjectsList, temp);
            originalClothes = MergedVsOriginal.CommonListProp;

            GetAllGameObjectsToList(clothAvatar, _clothObjectsList);
        }

        private void WhyIsThisBroken()
        {
            MainManager mainManager = transform.parent.gameObject.GetComponent<MainManager>();
            originalClothes = mainManager.objectsToDisable;
        }

        public void PopulateLists()
        {
            // Init Lists and Class Object
            var differenceList = new List<GameObject>();
            var NewVsOld = new DiffCheckObj();
            var MergedVsDiff = new DiffCheckObj();
            clothingSelection = new List<GameObject>();
            objectsToDisable = new List<GameObject>();

            NewVsOld = FindMatches(_clothObjectsList, _originalObjectsList);
            differenceList = NewVsOld.DifferenceListProp;

            MergedVsDiff = FindMatches(_mergedObjectsList, differenceList);
            clothingSelection = MergedVsDiff.CommonListProp;

            var MergedVsOriginal = new DiffCheckObj();
            MergedVsOriginal = FindMatches(_mergedObjectsList, originalClothes);
            originalClothes = new List<GameObject>();
            originalClothes = MergedVsOriginal.CommonListProp;

            var MergedVsExtra = new DiffCheckObj();
            MergedVsExtra = FindMatches(_mergedObjectsList, extraToDisable);
            extraToDisable = new List<GameObject>();
            extraToDisable = MergedVsExtra.CommonListProp;

            objectsToDisable.AddRange(originalClothes);
            objectsToDisable.AddRange(extraToDisable);
        }

        public void SelectToBeEnabled()
        {
            //Selects the GameObjects
            Selection.objects = clothingSelection.ToArray();
        }

        public void SelectToBeDisabled()
        {
            // Selects the results
            Selection.objects = objectsToDisable.ToArray();
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
    }
}
#endif
