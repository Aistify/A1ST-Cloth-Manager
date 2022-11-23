#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace A1STCloth
{
    [CustomEditor(typeof(AnimationPropagator))]
    public class AnimationPropagatorEditor : BaseClothEditor
    {
        public override void OnInspectorGUI()
        {
            AnimationPropagator animationPropagator = (AnimationPropagator)target;

            base.OnInspectorGUI();

            GUILayout.Space(15);
            if (GUILayout.Button("Propagate Animation To Mesh List"))
            {
                animationPropagator.PropagateAnimations();
            }

            if (isA1STNamespace)
                LogoGUI();
        }
    }
}
#endif
