#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace A1ST
{
    [CustomEditor(typeof(AvatarUtils))]
    public class AvatarUtilsEditor : BaseEditor
    {
        public override void OnInspectorGUI()
        {
            var avatarUtils = (AvatarUtils)target;

            base.OnInspectorGUI();

            GUILayout.Space(20);

            if (GUILayout.Button("Fix All"))
            {
                avatarUtils.SetAnchor();
                avatarUtils.SetBounds();
                avatarUtils.DisableClothObjects();
            }

            if (GUILayout.Button("Set Anchor Override"))
                avatarUtils.SetAnchor();

            if (GUILayout.Button("Set Bounds"))
                avatarUtils.SetBounds();

            if (GUILayout.Button("Disable Cloth Objects"))
                avatarUtils.DisableClothObjects();

            if (isA1STNamespace)
                LogoGUI();
        }
    }
}

#endif
