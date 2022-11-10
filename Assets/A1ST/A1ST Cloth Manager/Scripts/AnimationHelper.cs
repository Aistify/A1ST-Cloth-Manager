#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;
using Object = UnityEngine.Object;

namespace A1ST
{
    [Component("Animation Helper", "Helps make animations to toggle clothes.")]
    public class AnimationHelper : MonoBehaviour
    {
        // Variables
        [ReadOnly] public string animationName;
        [HideInInspector]
        public string path;
        private List<GameObject> _toggleOnList;
        private List<GameObject> _toggleOffList;
        private string assetPath;
        
        public void GenerateAnimationClip(Boolean toggle)
        {
            // Get reference to Components and reload/load it's variables
            SelectionHelper selectionHelper = gameObject.GetComponent<SelectionHelper>();
            selectionHelper.GetAllGameObjects();
            selectionHelper.PopulateLists();

            _toggleOnList = selectionHelper.clothingSelection;
            _toggleOffList = selectionHelper.objectsToDisable;

            // Check to see if the MainManager is in control of this animator
            if (animationName == null) animationName = "DebugAnim";
            
            // Get references to GameObjects to make toggles
            _toggleOnList = selectionHelper.clothingSelection;
            _toggleOffList = selectionHelper.objectsToDisable;
            
            // Init clip object
            AnimationClip clip = new AnimationClip();

            // Depending on the value of toggle ...
            if (toggle)
            {
                // Generate Path to save the clip at
                assetPath = string.Format(
                    path + "\\Animations\\{0} On.anim",
                    animationName
                );
                // Adds the GameObject toggles onto the clip object
                ToggleList(_toggleOnList, clip, false);
                ToggleList(_toggleOffList, clip, true);
            }
            else
            {
                // Generate Path to save the clip at
                assetPath = string.Format(
                    path + "\\Animations\\{0} Off.anim",
                    animationName
                );
                // Adds the GameObject toggles onto the clip object
                ToggleList(_toggleOnList, clip, true);
                ToggleList(_toggleOffList, clip, false);
            }
            
            // Check if Animation Folder Exists
            if (!AssetDatabase.IsValidFolder(path + "\\Animations")) AssetDatabase.CreateFolder(path, "Animations");
            
            // Checks if the Path has an already existing clip with the same name and deletes it
            if (File.Exists(assetPath))
            {
                print("Asset already exists, deleting and remaking");
                AssetDatabase.DeleteAsset(assetPath);
            }
            
            // Creates the Animation Clip
            AssetDatabase.CreateAsset(clip, assetPath);
            EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath<Object>(assetPath));
        }
        
        private void ToggleList(
            List<GameObject> listToProcess,
            AnimationClip clipToAddTo,
            Boolean toggle = true
        )
        {
            int state = 1;

            if (toggle == false)
            {
                state = 0;

                foreach (var toggleObject in listToProcess)
                {
                    string relativePath = GetGameObjectPath(toggleObject.transform);
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
                    string relativePath = GetGameObjectPath(toggleObject.transform);
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
            string path = transform.name;
            while (transform.parent != null)
            {
                transform = transform.parent;
                path = transform.name + "/" + path;
            }

            return path;
        }
        
    }
}

#endif
