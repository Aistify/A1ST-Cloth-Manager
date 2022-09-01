#if UNITY_EDITOR

// Import libraries
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace A1STAndCo
{
    [Component("Selection Helper", "Helps make selections of cloth objects and bones added onto avatars.")]
    public class SelectionHelper : MonoBehaviour
    {
        [Tooltip("Select the Avatar before clothes were added to it")]
        public GameObject originalAvatar;

        [Tooltip("Select the clothes that was to be merged into the Avatar")]
        public GameObject clothAvatar;

        [Tooltip("Select the Avatar with the clothes merged into it")]
        public GameObject mergedAvatar;
        
        [Tooltip("Select any GameObjects to be disabled when clothes are toggled.")]    
        public List<GameObject> originalObjectsToDisable;

        [Tooltip("All GameObjects under Original Avatar")]
        public List<GameObject> originalObjectsList;

        [Tooltip("All GameObjects under Cloth GameObject")]
        public List<GameObject> clothObjectsList;

        [Tooltip("All GameObjects under Merged Avatar")]
        public List<GameObject> mergedObjectsList;

        [Tooltip("List of GameObjects that are in both the Original and Clothes.")]
        public List<GameObject> commonList;

        [Tooltip("List of GameObjects that are not in the Original but in Clothes.")]
        public List<GameObject> differenceList;

        [Tooltip("List of GameObjects that are in both the Merged and Clothes but not in the Original.")]
        public List<GameObject> clothingSelection;

        [Tooltip("List of GameObjects that are not in both the Merged and the Original but in Clothes.")]
        public List<GameObject> clothingNotFound;

        [Tooltip("Add any GameObjects from the Original Avatar to be selected manually.")]
        public List<GameObject> additionalOriginalObjectsToDisable;

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public void GetAllGameObjects()
        {
            // Init Lists
            originalObjectsList = new List<GameObject>();
            clothObjectsList = new List<GameObject>();
            mergedObjectsList = new List<GameObject>();
            additionalOriginalObjectsToDisable = new List<GameObject>();
            
            // Populate Lists
            GetAllGameObjectsL(originalAvatar, originalObjectsList);
            GetAllGameObjectsL(clothAvatar, clothObjectsList);
            GetAllGameObjectsL(mergedAvatar, mergedObjectsList);
            foreach (var rootObject in originalObjectsToDisable)
            {
                additionalOriginalObjectsToDisable.Add(rootObject);
                GetAllGameObjectsL(rootObject, additionalOriginalObjectsToDisable);
            }

            
            
        }

        public void PopulateLists()
        {
            // Init Lists and Class Object
            commonList = new List<GameObject>();
            differenceList = new List<GameObject>();
            clothingSelection = new List<GameObject>();
            clothingNotFound = new List<GameObject>();
            var tempListObj = new TempListObj();
            var tempListObj2 = new TempListObj();
            var tempListObj3 = new TempListObj();

            // Search inside Clothes Object for anything inside Original Avatar Object
            tempListObj = FindMatches(clothObjectsList, originalObjectsList);

            // Populate Lists
            commonList = tempListObj.CListProp;
            differenceList = tempListObj.DListProp;

            //Search inside Merged Avatar Object for GameObjects that are not in the Original Avatar Object
            tempListObj2 = FindMatches(mergedObjectsList, differenceList);

            //Populate Lists
            clothingSelection = tempListObj2.CListProp;

            tempListObj3 = FindMatches(differenceList, clothingSelection);

            //Populate Lists
            clothingNotFound = tempListObj3.DListProp;
        }

        public void SelectClothInMerged()
        {
            //Selects the GameObjects
            Selection.objects = clothingSelection.ToArray();
        }

        public void SelectManualInMerged()
        {
            // Init Class Object
            var tempListObj4 = new TempListObj();
            
            // Search inside the Merged Avatar Object for anything inside the Manual Selection List
            tempListObj4 = FindMatches(mergedObjectsList, additionalOriginalObjectsToDisable);
            
            // Selects the results
            Selection.objects = tempListObj4.CListProp.ToArray();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void GetAllGameObjectsL(GameObject obj, List<GameObject> list)
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
                if (null == child) continue;

                // Else, add the Child GameObject to the passed in List
                list.Add(child.gameObject);

                // And search the Child GameObject if it has any children of it's own
                GetAllGameObjectsL(child.gameObject, list);
            }
        }

        private static TempListObj FindMatches(List<GameObject> listToSearch, List<GameObject> listToFind)
        {
            // Init temp Lists
            var tempCList = new List<GameObject>();
            var tempDList = new List<GameObject>();

            // For each GameObject in the list to search
            foreach (var searchObj in listToSearch)
            {
                // Init bool
                var matchFound = false;

                // Find a GameObject with the same name from the list to find
                foreach (var findObj in listToFind)
                    if (findObj.name == searchObj.name)
                    {
                        // When found add the GameObject to the common List
                        matchFound = true;
                        tempCList.Add(searchObj);
                    }

                // When no matches are found for a GameObject, it is added to the difference List
                if (matchFound == false) tempDList.Add(searchObj);
            }

            // Outputs/Returns a class object containing both common and difference list
            return new TempListObj { CListProp = tempCList, DListProp = tempDList };
        }

        // Class containing two lists to manipulate later
        private class TempListObj
        {
            public List<GameObject> CListProp { get; set; }
            public List<GameObject> DListProp { get; set; }
        }
    }
}
#endif