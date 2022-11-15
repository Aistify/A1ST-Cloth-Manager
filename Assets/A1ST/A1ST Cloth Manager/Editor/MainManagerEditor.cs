#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace A1ST
{
    [CustomEditor(typeof(MainManager))]
    public class MainManagerEditor : BaseEditor
    {
        public override void OnInspectorGUI()
        {
            var mainManager = (MainManager)target;

            base.OnInspectorGUI();

            GUILayout.Space(20);

            if (GUILayout.Button("Create Cloth Managers"))
                mainManager.CreateClothManagers();

            if (GUILayout.Button("Generate All Animations"))
                mainManager.GenerateAllAnimations();

            if (isA1STNamespace)
                LogoGUI();
        }
    }
}

#endif
