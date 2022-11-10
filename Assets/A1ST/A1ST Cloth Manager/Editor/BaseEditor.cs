#if UNITY_EDITOR

using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using System.Text.RegularExpressions;
using Object = UnityEngine.Object;

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Editor Template for A1ST namespace
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// Looks for anything that derives from MonoBehaviour
[CustomEditor(typeof(MonoBehaviour), editorForChildClasses: true)]
public class BaseEditor : Editor
{
    // Inits various variables
    private static GUIStyle _titleStyle;
    private static Texture2D _A1STLogo;
    private ComponentAttribute _componentAttribute;
    public Boolean isA1STNamespace;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    // Usually OnEnable() is called when the inspector window is opened up
    protected virtual void OnEnable()
    {
        // Determines if the namespace of the target object belongs to A1ST
        string targetNamespace = target.GetType().Namespace;
        if (!string.IsNullOrEmpty(targetNamespace))
            isA1STNamespace = targetNamespace.StartsWith("A1ST");

        // Gets component attributes from target
        if (_componentAttribute == null)
            _componentAttribute = GetComponentAttribute(target);
    }

    public override void OnInspectorGUI()
    {
        // If target is within A1ST namespace, display the header
        if (isA1STNamespace)
            HeaderGUI(_componentAttribute);

        base.OnInspectorGUI();
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void LogoGUI()
    {
        if (_A1STLogo == null){
            string path = new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileName();
            String[] extract = Regex.Split(path, "Assets");
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
                    (GUILayout.Width(Screen.width * .8f))
                );

                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
            }

            GUILayout.Space(20f);
        }
    }

    private static ComponentAttribute GetComponentAttribute(Object obj)
    {
        // Gets the component attribute from the target Object
        return obj.GetType().GetCustomAttribute<ComponentAttribute>()
            ??
            // If there is no component attribute, make one
            new ComponentAttribute(obj.GetType().ToString());
    }
}
#endif
