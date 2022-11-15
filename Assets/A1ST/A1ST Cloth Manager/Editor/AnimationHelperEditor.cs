#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace A1ST
{
    [CustomEditor(typeof(AnimationHelper))]
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public class AnimationHelperEditor : BaseEditor
    {
        public override void OnInspectorGUI()
        {
            var animationHelper = (AnimationHelper)target;

            base.OnInspectorGUI();

            if (GUILayout.Button("Generate Animations"))
            {
                animationHelper.GenerateAnimationClip(true);
                animationHelper.GenerateAnimationClip(false);
            }

            if (isA1STNamespace)
                LogoGUI();
        }
    }
}

#endif
