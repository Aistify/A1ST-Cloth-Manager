﻿using UnityEditor;
using UnityEngine;

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace A1ST
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
            if (GUILayout.Button("Select Objects To Be Enabled (For Debug)"))
            {
                selectionHelper.GetAllGameObjects();
                selectionHelper.PopulateLists();
                selectionHelper.SelectToBeEnabled();
            }

            if (GUILayout.Button("Select Objects To Be Disabled (For Debug)"))
            {
                selectionHelper.GetAllGameObjects();
                selectionHelper.PopulateLists();
                selectionHelper.SelectToBeDisabled();
            }

            // At the very end, inserts the Logo
            if (isA1STNamespace)
                LogoGUI();
        }
    }
}