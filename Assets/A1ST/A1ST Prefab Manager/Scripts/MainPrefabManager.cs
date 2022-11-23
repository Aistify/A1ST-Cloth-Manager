#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace A1STPrefab
{
    [Component("A1ST Main Prefab Manager", "Used to manage and hold all other prefabs")]
    public class MainPrefabManager : MonoBehaviour
    {
        // Data Storage
        [HideInInspector]
        public List<PrefabStruct> prefabs;

        public GameObject originalAvatar;

        public void UpdateList()
        {
            prefabs = new List<PrefabStruct>();
            if (gameObject.transform.childCount == 0)
            {
                print("A1ST || Error : No child objects under Main Prefab Manager GameObject");
                return;
            }

            foreach (Transform child in gameObject.transform)
            {
                PrefabStruct temp = new PrefabStruct();
                temp.prefab = child.gameObject;
                temp.prefabName = child.gameObject.name;
                prefabs.Add(temp);
            }
        }

        public void RenamePrefabs()
        {
            foreach (var prefabStruct in prefabs)
            {
                prefabStruct.prefab.name = prefabStruct.prefabName;
            }
        }

        public void RenameMeshes()
        {
            foreach (var prefabStruct in prefabs)
            {
                RecursiveRename(prefabStruct.prefab, prefabStruct.prefabName);
            }
        }

        public void ManageAnimator()
        {
            foreach (var prefabStruct in prefabs)
            {
                if (prefabStruct.prefab.GetComponent<Animator>() == null)
                {
                    Animator animator = prefabStruct.prefab.AddComponent<Animator>();
                    animator.avatar = originalAvatar.GetComponent<Animator>().avatar;
                    animator.applyRootMotion = false;
                    animator.cullingMode = AnimatorCullingMode.CullUpdateTransforms;
                }
                else
                {
                    Animator animator = prefabStruct.prefab.GetComponent<Animator>();
                    animator.avatar = originalAvatar.GetComponent<Animator>().avatar;
                    animator.applyRootMotion = false;
                    animator.cullingMode = AnimatorCullingMode.CullUpdateTransforms;
                }
            }
        }

        private void RecursiveRename(GameObject obj, string prefabName)
        {
            if (obj == null)
            {
                Debug.Log("No GameObject or List passed in to process");
                return;
            }

            foreach (Transform child in obj.transform)
            {
                if (null == child)
                    continue;

                if (child.GetComponent<SkinnedMeshRenderer>() != null)
                {
                    if (child.name.Count((c => c == ']')) > 0)
                    {
                        string[] meshName = child.name.Split(']');
                        child.name = String.Format(
                            "[{0}] {1}",
                            prefabName.Trim(),
                            meshName[1].Trim()
                        );
                    }
                    else
                    {
                        child.name = String.Format(
                            "[{0}] {1}",
                            prefabName.Trim(),
                            child.name.Trim()
                        );
                    }
                }

                RecursiveRename(child.gameObject, prefabName);
            }
        }
    }
}
#endif
