#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;

namespace A1STAndCo
{
    [Component("Animation Helper", "Helps make animations to toggle clothes.")]
    public class AnimationHelper : MonoBehaviour
    {
        [HideInInspector]
        public string animationName;
        public List<GameObject> toggleOnList;
        public List<GameObject> toggleOffList;

        public void GenerateOffAnimationClip()
        {
            if (animationName == null) animationName = "DebugAnim";
            string assetPath = string.Format("Assets/A1ST Scripts/Animations/{0} Off.anim", animationName);

            toggleOnList = gameObject.GetComponent<SelectionHelper>().clothingSelection;
            toggleOffList = gameObject.GetComponent<SelectionHelper>().additionalOriginalObjectsToDisable;

            AnimationClip clip = new AnimationClip();

            ToggleList(toggleOnList, clip, false);
            ToggleList(toggleOffList, clip, true);

            if (File.Exists(assetPath))
            {
                print("Asset already exists, deleting and remaking");
                AssetDatabase.DeleteAsset(assetPath);
            }

            AssetDatabase.CreateAsset(clip, assetPath);
        }

        public void GenerateOnAnimationClip()
        {
            if (animationName == null) animationName = "DebugAnim";
            string assetPath = string.Format("Assets/A1ST Scripts/Animations/{0} On.anim", animationName);

            toggleOnList = gameObject.GetComponent<SelectionHelper>().clothingSelection;
            toggleOffList = gameObject.GetComponent<SelectionHelper>().additionalOriginalObjectsToDisable;

            AnimationClip clip = new AnimationClip();

            ToggleList(toggleOnList, clip, true);
            ToggleList(toggleOffList, clip, false);

            if (File.Exists(assetPath))
            {
                print("Asset already exists, deleting and remaking");
                AssetDatabase.DeleteAsset(assetPath);
            }

            AssetDatabase.CreateAsset(clip, assetPath);
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

        private void ToggleList(List<GameObject> listToProcess, AnimationClip clipToAddTo, Boolean toggle = true)
        {
            int state = 1;

            if (toggle == false)
            {
                state = 0;

                foreach (var toggleObject in listToProcess)
                {
                    string relativePath = GetGameObjectPath(toggleObject.transform);
                    
                    // Temporarily disabled due to VRC updates
                    /*TempPosRot t = new TempPosRot();

                    t.pX = toggleObject.transform.localPosition.x;
                    t.pY = toggleObject.transform.localPosition.y;
                    t.pZ = toggleObject.transform.localPosition.z;

                    t.rX = TransformUtils.GetInspectorRotation(toggleObject.transform).x;
                    t.rY = TransformUtils.GetInspectorRotation(toggleObject.transform).y;
                    t.rZ = TransformUtils.GetInspectorRotation(toggleObject.transform).z;*/

                    /*clipToAddTo.SetCurve(relativePath, typeof(Transform), "position.x",
                        AnimationCurve.EaseInOut(0, t.pX, .01f, t.pX));
                    clipToAddTo.SetCurve(relativePath, typeof(Transform), "position.y",
                        AnimationCurve.EaseInOut(0, t.pY, .01f, t.pY));
                    clipToAddTo.SetCurve(relativePath, typeof(Transform), "position.z",
                        AnimationCurve.EaseInOut(0, t.pZ, .01f, t.pZ));

                    clipToAddTo.SetCurve(relativePath, typeof(Transform), "rotation.x",
                        AnimationCurve.EaseInOut(0, t.rX, .01f, t.rX));
                    clipToAddTo.SetCurve(relativePath, typeof(Transform), "rotation.y",
                        AnimationCurve.EaseInOut(0, t.rY, .01f, t.rY));
                    clipToAddTo.SetCurve(relativePath, typeof(Transform), "rotation.z",
                        AnimationCurve.EaseInOut(0, t.rZ, .01f, t.rZ));*/
                    clipToAddTo.SetCurve(relativePath, typeof(GameObject), "m_IsActive",
                        AnimationCurve.EaseInOut(0, state, .01f, state));
                }
            }
            else
            {
                foreach (var toggleObject in listToProcess)
                {
                    string relativePath = GetGameObjectPath(toggleObject.transform);

                    clipToAddTo.SetCurve(relativePath, typeof(GameObject), "m_IsActive",
                        AnimationCurve.EaseInOut(0, state, .01f, state));
                }
            }
        }

        private class TempPosRot
        {
            public float pX { set; get; }
            public float pY { set; get; }
            public float pZ { set; get; }

            public float rX { set; get; }
            public float rY { set; get; }
            public float rZ { set; get; }
        }
    }
}

#endif