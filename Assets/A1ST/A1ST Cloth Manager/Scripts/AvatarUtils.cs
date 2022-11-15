#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;
using System.Linq;

namespace A1ST
{
    [Component("Avatar Utilities", "Extra Tools for setting up your avatar faster.")]
    public class AvatarUtils : MonoBehaviour
    {
        [Space(20)]
        public GameObject anchorOverride;

        public void SetAnchor()
        {
            MainManager mainManager = transform.gameObject.GetComponent<MainManager>();
            List<GameObject> mergedObjectsList = new List<GameObject>();
            GetAllGameObjectsToList(mainManager.mergedAvatar, mergedObjectsList);

            if (anchorOverride == null)
                anchorOverride = mergedObjectsList
                    .Where(obj => obj.name == "Chest")
                    .SingleOrDefault();

            foreach (GameObject gameObj in mergedObjectsList)
            {
                if (gameObj.GetComponent(typeof(SkinnedMeshRenderer)) != null)
                {
                    gameObj.GetComponent<SkinnedMeshRenderer>().probeAnchor =
                        anchorOverride.transform;
                }
            }
        }

        public void SetBounds()
        {
            MainManager mainManager = transform.gameObject.GetComponent<MainManager>();
            List<GameObject> mergedObjectsList = new List<GameObject>();
            GetAllGameObjectsToList(mainManager.mergedAvatar, mergedObjectsList);
            Bounds newBounds = new Bounds();
            newBounds.center = Vector3.zero;
            newBounds.extents = Vector3.one;

            foreach (GameObject gameObj in mergedObjectsList)
            {
                if (gameObj.GetComponent(typeof(SkinnedMeshRenderer)) != null)
                {
                    gameObj.GetComponent<SkinnedMeshRenderer>().localBounds = newBounds;
                }
            }
        }

        public void DisableClothObjects()
        {
            MainManager mainManager = transform.gameObject.GetComponent<MainManager>();
            foreach (var clothManager in mainManager.clothManagerList)
            {
                List<GameObject> clothingSelection = clothManager.clothManagerPrefab
                    .GetComponent<SelectionHelper>()
                    .clothingSelection;

                foreach (GameObject gameObj in clothingSelection)
                {
                    gameObj.SetActive(false);
                }
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
    }
}
#endif
