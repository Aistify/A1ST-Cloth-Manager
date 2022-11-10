#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace A1ST
{
    [CustomEditor(typeof(MainManager))]
    public class MainToolManagerEditor : BaseEditor
    {
        public override void OnInspectorGUI()
        {
            MainManager mainManager = (MainManager)target;

            base.OnInspectorGUI();

            if (GUILayout.Button("Create Cloth Managers"))
            {
                mainManager.CreateClothManagers();
            }

            if (isA1STNamespace)
                LogoGUI();
        }
    }
}

#endif
