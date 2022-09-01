using UnityEditor;
using UnityEngine;

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace A1STAndCo
{
    [CustomEditor(typeof(SelectionHelper))]

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public class SelectionHelperEditor : BaseEditor
    {
        public override void OnInspectorGUI()
        {
            SelectionHelper selectionHelper = (SelectionHelper)target;

            // Selection Helper /w Base Editor modifications starts here
            base.OnInspectorGUI();

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////

            // Button Layout starts here
            if (GUILayout.Button("Prepare Selection"))
            {
                selectionHelper.GetAllGameObjects();
                selectionHelper.PopulateLists();
            }

            if (GUILayout.Button("Select Objects To Be Enabled (For Debug)"))
            {
                selectionHelper.SelectClothInMerged();
            }

            if (GUILayout.Button("Select Objects To Be Disabled (For Debug)"))
            {
                selectionHelper.SelectManualInMerged();
            }

            // At the very end, inserts the Logo
            if (isA1STNamespace)
                LogoGUI();
        }
    }
}