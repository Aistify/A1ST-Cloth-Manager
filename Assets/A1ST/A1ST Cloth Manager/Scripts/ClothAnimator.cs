#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Windows;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace A1STCloth
{
    [Component("A1ST Cloth Animator", "Generates animations with a blacklist whitelist system.")]
    public class ClothAnimator : MonoBehaviour
    {
        [Space(20)]
        [Tooltip("Components to enable when animation is toggled on")]
        public ClothManager clothManagerRef;

        [Tooltip("Components to disable when animation is toggled on")]
        public ClothManager originalClothManagerRef;

        [Tooltip("List of additional Cloth Managers to grab objects from")]
        public List<ClothManager> additionalClothManagers;

        [Tooltip("Components to disable when animation is toggled on")]
        public ClothManager copyExceptionsRef;

        [HideInInspector]
        public List<GameObject> phsBonesList;

        [HideInInspector]
        public List<GameObject> collidersList;

        [HideInInspector]
        public List<GameObject> contactsList;

        [HideInInspector]
        public List<GameObject> meshesList;

        [HideInInspector]
        public List<GameObject> whitelistExceptions;

        [HideInInspector]
        public List<GameObject> blacklistExceptions;

        [HideInInspector]
        public string animationName;

        public void LoadBlacklistExceptions()
        {
            if (copyExceptionsRef != null)
            {
                blacklistExceptions.AddRange(
                    copyExceptionsRef.physBones?.Except(blacklistExceptions)
                        ?? new List<GameObject>()
                );
                blacklistExceptions.AddRange(
                    copyExceptionsRef.colliders?.Except(blacklistExceptions)
                        ?? new List<GameObject>()
                );
                blacklistExceptions.AddRange(
                    copyExceptionsRef.contacts?.Except(blacklistExceptions)
                        ?? new List<GameObject>()
                );
                blacklistExceptions.AddRange(
                    copyExceptionsRef.meshes?.Except(blacklistExceptions) ?? new List<GameObject>()
                );
            }
        }

        public void ReloadList()
        {
            phsBonesList = new List<GameObject>();
            collidersList = new List<GameObject>();
            contactsList = new List<GameObject>();
            meshesList = new List<GameObject>();

            phsBonesList.AddRange(clothManagerRef.physBones);
            collidersList.AddRange(clothManagerRef.colliders);
            contactsList.AddRange(clothManagerRef.contacts);
            meshesList.AddRange(clothManagerRef.meshes);

            if (additionalClothManagers.Count != 0)
                foreach (ClothManager addClothManager in additionalClothManagers)
                {
                    phsBonesList.AddRange(addClothManager.physBones);
                    collidersList.AddRange(addClothManager.colliders);
                    contactsList.AddRange(addClothManager.contacts);
                    meshesList.AddRange(addClothManager.meshes);
                }
        }

        public void GenerateAnimationClip(bool toggle)
        {
            List<GameObject> whitelistInit = new List<GameObject>();
            List<GameObject> whitelistFinal = new List<GameObject>();

            whitelistInit.AddRange(phsBonesList);
            whitelistInit.AddRange(collidersList);
            whitelistInit.AddRange(contactsList);
            whitelistInit.AddRange(meshesList);

            foreach (GameObject gameObj in whitelistInit)
            {
                if (!whitelistExceptions.Contains(gameObj))
                {
                    whitelistFinal.Add(gameObj);
                }
            }

            List<GameObject> blacklistInit = new List<GameObject>();
            List<GameObject> blacklistFinal = new List<GameObject>();
            blacklistInit.AddRange(originalClothManagerRef.physBones);
            blacklistInit.AddRange(originalClothManagerRef.colliders);
            blacklistInit.AddRange(originalClothManagerRef.contacts);
            blacklistInit.AddRange(originalClothManagerRef.meshes);

            foreach (GameObject gameObj in blacklistInit)
            {
                if (!blacklistExceptions.Contains(gameObj))
                {
                    blacklistFinal.Add(gameObj);
                }
            }

            string path = GetScriptRoot();
            var clip = new AnimationClip();

            if (!AssetDatabase.IsValidFolder(path + "\\Animations"))
                AssetDatabase.CreateFolder(path, "Animations");

            if (animationName == "")
                animationName = "Animation";

            if (toggle)
            {
                path = string.Format(path + "\\Animations\\{0} On.anim", animationName);
                ToggleList(blacklistFinal, clip, false);
                ToggleList(whitelistFinal, clip);
            }
            else
            {
                path = string.Format(path + "\\Animations\\{0} Off.anim", animationName);
                ToggleList(whitelistFinal, clip, false);
                ToggleList(blacklistFinal, clip);
            }

            if (File.Exists(path))
            {
                print("Asset already exists, deleting and remaking");
                AssetDatabase.DeleteAsset(path);
            }

            AssetDatabase.CreateAsset(clip, path);
            EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath<Object>(path));
        }

        private void ToggleList(
            List<GameObject> listToProcess,
            AnimationClip clipToAddTo,
            bool toggle = true
        )
        {
            var state = 1;

            if (toggle == false)
            {
                state = 0;

                foreach (var toggleObject in listToProcess)
                {
                    var relativePath = GetGameObjectPath(toggleObject.transform);
                    relativePath = relativePath.Substring(relativePath.IndexOf("/") + 1);
                    clipToAddTo.SetCurve(
                        relativePath,
                        typeof(GameObject),
                        "m_IsActive",
                        AnimationCurve.EaseInOut(0, state, .01f, state)
                    );
                }
            }
            else
            {
                foreach (var toggleObject in listToProcess)
                {
                    var relativePath = GetGameObjectPath(toggleObject.transform);
                    relativePath = relativePath.Substring(relativePath.IndexOf("/") + 1);
                    clipToAddTo.SetCurve(
                        relativePath,
                        typeof(GameObject),
                        "m_IsActive",
                        AnimationCurve.EaseInOut(0, state, .01f, state)
                    );
                }
            }
        }

        private static string GetGameObjectPath(Transform transform)
        {
            var path = transform.name;
            while (transform.parent != null)
            {
                transform = transform.parent;
                path = transform.name + "/" + path;
            }

            return path;
        }

        private string GetScriptRoot()
        {
            var path = new StackTrace(true).GetFrame(0).GetFileName();
            var extract = Regex.Split(path, "Assets");
            path = extract[1].TrimEnd('\\');
            path = path.Replace("\\Scripts\\ClothAnimator.cs", "");
            path = "Assets" + path;
            return path;
        }
    }
}

#endif
