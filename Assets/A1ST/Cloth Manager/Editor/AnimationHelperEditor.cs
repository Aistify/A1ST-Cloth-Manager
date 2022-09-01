#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace A1STAndCo
{
    [CustomEditor(typeof(AnimationHelper))]
    
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public class AnimationHelperEditor : BaseEditor
    {
        public override void OnInspectorGUI()
        {
            AnimationHelper animationHelper = (AnimationHelper) target;
                
            base.OnInspectorGUI();

            if (GUILayout.Button("Generate Animations"))
            {
                animationHelper.GenerateOnAnimationClip();
                animationHelper.GenerateOffAnimationClip();
            }
            
            if (isA1STNamespace)
                LogoGUI();
        }
    }
}

#endif