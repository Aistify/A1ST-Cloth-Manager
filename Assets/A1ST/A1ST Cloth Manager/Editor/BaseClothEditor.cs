#if UNITY_EDITOR

using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace A1STCloth
{
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class BaseClothEditor : Editor
    {
        private static GUIStyle _titleStyle;
        private static Texture2D _A1STLogo;
        public bool isA1STNamespace;
        private ComponentAttribute _componentAttribute;

        protected virtual void OnEnable()
        {
            var targetNamespace = target.GetType().Namespace;
            if (!string.IsNullOrEmpty(targetNamespace))
                isA1STNamespace = targetNamespace.StartsWith("A1STCloth");

            if (_componentAttribute == null)
                _componentAttribute = GetComponentAttribute(target);
        }

        public override void OnInspectorGUI()
        {
            if (isA1STNamespace)
                HeaderGUI(_componentAttribute);

            base.OnInspectorGUI();
        }

        public static void LogoGUI()
        {
            if (_A1STLogo == null)
            {
                var path = new StackTrace(true).GetFrame(0).GetFileName();
                var extract = Regex.Split(path, "Assets");
                path = extract[1].TrimEnd('\\');
                path = path.Replace("\\Editor\\BaseEditor.cs", "");
                path = "Assets" + path;

                _A1STLogo = AssetDatabase.LoadAssetAtPath<Texture2D>(path + "/Logo/logo.png");
            }

            if (_A1STLogo != null)
            {
                GUILayout.Space(20f);

                EditorGUILayout.BeginHorizontal();

                GUILayout.Label(_A1STLogo);

                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
            }
        }

        private static void HeaderGUI(ComponentAttribute componentAttribute)
        {
            if (componentAttribute != null)
            {
                if (_titleStyle == null)
                {
                    _titleStyle = new GUIStyle(GUI.skin.label);
                    _titleStyle.fontSize = 15;
                    _titleStyle.fontStyle = FontStyle.Bold;
                    _titleStyle.alignment = TextAnchor.MiddleCenter;
                }

                GUILayout.Space(10f);

                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();

                GUILayout.Label(componentAttribute.Name, _titleStyle);

                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();

                if (!string.IsNullOrEmpty(componentAttribute.Description))
                {
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();

                    GUILayout.Box(
                        componentAttribute.Description,
                        GUILayout.Width(Screen.width * .8f)
                    );

                    GUILayout.FlexibleSpace();
                    EditorGUILayout.EndHorizontal();
                }

                GUILayout.Space(20f);
            }
        }

        private static ComponentAttribute GetComponentAttribute(Object obj)
        {
            return obj.GetType().GetCustomAttribute<ComponentAttribute>()
                ?? new ComponentAttribute(obj.GetType().ToString());
        }
    }
}
#endif
