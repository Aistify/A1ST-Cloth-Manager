#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using VRC.Dynamics;
using VRC.SDK3.Dynamics.Contact.Components;
using VRC.SDK3.Dynamics.PhysBone.Components;

namespace A1STCloth
{
    [CustomEditor(typeof(ClothAnimator))]
    public class ClothAnimatorEditor : BaseEditor
    {
        private bool _showWhitelist;
        private bool _showBlacklist;

        private bool _showBlacklistExceptions;
        private bool _showWhitelistExceptions;

        private static GUIStyle _titleStyle;

        public override void OnInspectorGUI()
        {
            ClothAnimator clothAnimator = (ClothAnimator)target;

            if (_titleStyle == null)
            {
                _titleStyle = new GUIStyle(GUI.skin.label);
                _titleStyle.fontSize = 13;
                _titleStyle.alignment = TextAnchor.MiddleCenter;
            }

            base.OnInspectorGUI();

            GUILayout.Space(10);

            GUILayout.Label("Object Lists", _titleStyle);

            if (
                clothAnimator.clothManagerRef == null
                || clothAnimator.originalClothManagerRef == null
            )
                return;

            _showWhitelist = EditorGUILayout.Foldout(_showWhitelist, "Toggle On Objects", false);
            DropdownList(
                _showWhitelist,
                clothAnimator.phsBonesList,
                "PhysBones",
                clothAnimator.whitelistExceptions
            );
            DropdownList(
                _showWhitelist,
                clothAnimator.collidersList,
                "Colliders",
                clothAnimator.whitelistExceptions
            );
            DropdownList(
                _showWhitelist,
                clothAnimator.contactsList,
                "Contacts",
                clothAnimator.whitelistExceptions
            );
            DropdownList(
                _showWhitelist,
                clothAnimator.meshesList,
                "Meshes",
                clothAnimator.whitelistExceptions
            );

            if (_showWhitelist)
                if (GUILayout.Button("Reload On List"))
                {
                    clothAnimator.ReloadList();
                }

            GUILayout.Space(3);

            _showBlacklist = EditorGUILayout.Foldout(_showBlacklist, "Toggle Off Objects", false);
            DropdownList(
                _showBlacklist,
                clothAnimator.originalClothManagerRef.physBones,
                "PhysBones",
                clothAnimator.blacklistExceptions
            );
            DropdownList(
                _showBlacklist,
                clothAnimator.originalClothManagerRef.colliders,
                "Colliders",
                clothAnimator.blacklistExceptions
            );
            DropdownList(
                _showBlacklist,
                clothAnimator.originalClothManagerRef.contacts,
                "Contacts",
                clothAnimator.blacklistExceptions
            );
            DropdownList(
                _showBlacklist,
                clothAnimator.originalClothManagerRef.meshes,
                "Meshes",
                clothAnimator.blacklistExceptions
            );

            GUILayout.Space(10);

            GUILayout.Label("Exceptions", _titleStyle);

            _showWhitelistExceptions = EditorGUILayout.Foldout(
                _showWhitelistExceptions,
                "Toggle On Exceptions",
                false
            );
            DropdownListExceptions(
                _showWhitelistExceptions,
                clothAnimator.whitelistExceptions,
                "Toggle On Exceptions"
            );

            GUILayout.Space(3);

            _showBlacklistExceptions = EditorGUILayout.Foldout(
                _showBlacklistExceptions,
                "Toggle Off Exceptions",
                false
            );
            DropdownListExceptions(
                _showBlacklistExceptions,
                clothAnimator.blacklistExceptions,
                "Toggle Off Exceptions"
            );

            if (GUILayout.Button("Load reference into Toggle Off Exceptions"))
            {
                clothAnimator.LoadBlacklistExceptions();
            }

            GUILayout.Space(10);

            GUILayout.Label("Animation Generator", _titleStyle);

            EditorGUILayout.BeginHorizontal();

            GUILayout.Label("Animation Clip Name", GUILayout.Width(130));
            clothAnimator.animationName = GUILayout.TextField(clothAnimator.animationName);

            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Generate Animations (WD OFF)"))
            {
                clothAnimator.GenerateAnimationClip(true);
                clothAnimator.GenerateAnimationClip(false);
            }

            if (isA1STNamespace)
                LogoGUI();
        }

        public void DropdownList(
            bool targetBool,
            List<GameObject> targetList,
            string targetName,
            List<GameObject> exceptionList
        )
        {
            if (targetBool)
            {
                GUILayout.Label(targetName, EditorStyles.boldLabel);
                if (targetList.Count == 0)
                {
                    EditorGUILayout.BeginHorizontal();

                    GUILayout.Label("No " + targetName + " Found");

                    EditorGUILayout.EndHorizontal();
                }

                for (int i = 0; i < targetList.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();

                    targetList[i] =
                        EditorGUILayout.ObjectField(targetList[i], typeof(GameObject), true)
                        as GameObject;
                    if (GUILayout.Button("Add to Exceptions List"))
                    {
                        exceptionList.Add(targetList[i]);
                    }

                    EditorGUILayout.EndHorizontal();
                }

                GUILayout.Space(10);
            }
        }

        public void DropdownListExceptions(
            bool targetBool,
            List<GameObject> targetList,
            string targetName
        )
        {
            if (targetBool)
            {
                if (targetList.Count == 0)
                {
                    EditorGUILayout.BeginHorizontal();

                    GUILayout.Label("No " + targetName + " Found");

                    EditorGUILayout.EndHorizontal();
                }

                for (int i = 0; i < targetList.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();

                    targetList[i] =
                        EditorGUILayout.ObjectField(targetList[i], typeof(GameObject), true)
                        as GameObject;
                    if (GUILayout.Button("Remove"))
                    {
                        targetList.Remove(targetList[i]);
                    }

                    EditorGUILayout.EndHorizontal();
                }

                GUILayout.Space(10);
            }
        }
    }
}
#endif
