#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace A1STAndCo
{
    [CustomPropertyDrawer(typeof(ClothManager))]
    public class ClothManagerDraw : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var clothManagerName = property.FindPropertyRelative("clothManagerName");
            var clothManagerPrefab = property.FindPropertyRelative("clothManagerPrefab");
            var clothAvatar = property.FindPropertyRelative("clothAvatar");

            var labelPos = new Rect(position.x, position.y, position.width, position.height);

            position = EditorGUI.PrefixLabel(
                labelPos,
                GUIUtility.GetControlID(FocusType.Passive),
                new GUIContent(clothManagerName.stringValue)
            );

            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            var widthSize = position.width / 3;
            float offsetSize = 3;

            var pos1 = new Rect(position.x, position.y, widthSize - offsetSize, position.height);

            var pos2 = new Rect(
                position.x + widthSize * 1,
                position.y,
                widthSize - offsetSize,
                position.height
            );

            var pos3 = new Rect(position.x + widthSize * 2, position.y, widthSize, position.height);

            EditorGUI.PropertyField(pos1, clothManagerName, GUIContent.none);
            EditorGUI.PropertyField(pos2, clothManagerPrefab, GUIContent.none);
            EditorGUI.PropertyField(pos3, clothAvatar, GUIContent.none);

            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
}

#endif
