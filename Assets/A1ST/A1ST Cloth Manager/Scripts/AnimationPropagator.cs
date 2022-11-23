#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace A1STCloth
{
    [Component(
        "A1ST Animation Propagator",
        "Propagates animations from one mesh to a list of others."
    )]
    public class AnimationPropagator : MonoBehaviour
    {
        [Space(20)]
        public AnimationClip clip;
        public List<GameObject> meshList;
        public string animationName;

        public void PropagateAnimations()
        {
            AnimationClip outputClip = new AnimationClip();

            foreach (GameObject gameObj in meshList)
            {
                string relativePath = GetGameObjectPath(gameObj.transform);
                relativePath = relativePath.Substring(relativePath.IndexOf("/") + 1);

                EditorCurveBinding[] bindings = AnimationUtility.GetCurveBindings(clip);
                foreach (EditorCurveBinding binding in bindings)
                {
                    print(binding.path + binding.propertyName);
                    Keyframe[] keyframes = AnimationUtility.GetEditorCurve(clip, binding).keys;
                    AnimationCurve curve = new AnimationCurve(keyframes);
                    outputClip.SetCurve(
                        relativePath,
                        typeof(SkinnedMeshRenderer),
                        binding.propertyName,
                        curve
                    );
                }
            }

            string path = GetScriptRoot();

            path = string.Format(path + "\\Animations\\{0}.anim", animationName);

            AssetDatabase.CreateAsset(outputClip, path);
            EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath<Object>(path));
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
            path = path.Replace("\\Scripts\\AnimationPropagator.cs", "");
            path = "Assets" + path;
            return path;
        }
    }
}

#endif
