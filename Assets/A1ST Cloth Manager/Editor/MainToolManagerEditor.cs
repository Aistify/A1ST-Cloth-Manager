#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace A1STAndCo
{
    [CustomEditor(typeof(MainToolManager))]
    public class MainToolManagerEditor : BaseEditor
    {
        public override void OnInspectorGUI()
        {
            MainToolManager mainToolManager = (MainToolManager) target;
            
            base.OnInspectorGUI();
            
            if (GUILayout.Button("Create Cloth Managers"))
            {
                mainToolManager.CreateClothManagers();
            }
            
            if (isA1STNamespace)
                LogoGUI();
        }
    }
}

#endif