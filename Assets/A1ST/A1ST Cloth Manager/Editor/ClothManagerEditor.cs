#if UNITY_EDITOR

using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using VRC.Dynamics;
using VRC.SDK3.Dynamics.Contact.Components;
using VRC.SDK3.Dynamics.PhysBone.Components;

namespace A1STCloth
{
    [CustomEditor(typeof(ClothManager))]
    public class AvatarDynamicsHelper1Editor : BaseEditor
    {
        private static GUIStyle _titleStyle;
        private bool listReplace;
        private bool selectionReplace;

        public override void OnInspectorGUI()
        {
            ClothManager clothManager = (ClothManager)target;

            if (_titleStyle == null)
            {
                _titleStyle = new GUIStyle(GUI.skin.label);
                _titleStyle.fontSize = 13;
                _titleStyle.alignment = TextAnchor.MiddleCenter;
            }

            base.OnInspectorGUI();

            GUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            GUILayout.Label("Cloth Animators", _titleStyle);

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(5);

            if (GUILayout.Button("Create Cloth Animator"))
            {
                clothManager.CreateClothAnimator();
            }

            GUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            GUILayout.Label("Quick Select", _titleStyle);

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(5);

            clothManager.showPhysBoneList = EditorGUILayout.Foldout(
                clothManager.showPhysBoneList,
                string.Format("{0} List ({1} Item(s))", "Physbones", clothManager.physBones.Count),
                true
            );
            DropdownList(clothManager.showPhysBoneList, clothManager.physBones, "Physbones");

            clothManager.showCollidersList = EditorGUILayout.Foldout(
                clothManager.showCollidersList,
                string.Format("{0} List ({1} Item(s))", "Colliders", clothManager.colliders.Count),
                true
            );
            DropdownList(clothManager.showCollidersList, clothManager.colliders, "Colliders");

            clothManager.showContactsList = EditorGUILayout.Foldout(
                clothManager.showContactsList,
                string.Format("{0} List ({1} Item(s))", "Contacts", clothManager.contacts.Count),
                true
            );
            DropdownList(clothManager.showContactsList, clothManager.contacts, "Contacts");

            clothManager.showMeshesList = EditorGUILayout.Foldout(
                clothManager.showMeshesList,
                string.Format("{0} List ({1} Item(s))", "Meshes", clothManager.meshes.Count),
                true
            );
            DropdownList(clothManager.showMeshesList, clothManager.meshes, "Meshes");

            if (GUILayout.Button("Select All"))
            {
                List<GameObject> targetList = new List<GameObject>();

                targetList.AddRange(clothManager.physBones);
                targetList.AddRange(clothManager.colliders);
                targetList.AddRange(clothManager.contacts);
                targetList.AddRange(clothManager.meshes);

                if (targetList.Count != 0)
                    Selection.objects = targetList.ToArray();
            }

            GUILayout.Space(20);

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            GUILayout.Label("List Managment", _titleStyle);

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(5);

            if (GUILayout.Button("Generate Lists ( Merged vs Target )"))
            {
                if (listReplace)
                {
                    clothManager.physBones.Clear();
                    clothManager.colliders.Clear();
                    clothManager.contacts.Clear();
                    clothManager.meshes.Clear();
                }

                clothManager.GetPhysBones(listReplace);
                clothManager.GetColliders(listReplace);
                clothManager.GetContacts(listReplace);
                clothManager.GetMeshes(listReplace);
            }

            if (GUILayout.Button("Copy Lists ( Data Source )"))
            {
                ClothManager dataSourceRef = clothManager.dataSource.GetComponent<ClothManager>();

                if (listReplace)
                {
                    clothManager.physBones.Clear();
                    clothManager.colliders.Clear();
                    clothManager.contacts.Clear();
                    clothManager.meshes.Clear();
                }

                clothManager.physBones.AddRange(
                    dataSourceRef.physBones?.Except(clothManager.physBones)
                        ?? new List<GameObject>()
                );
                clothManager.colliders.AddRange(
                    dataSourceRef.colliders?.Except(clothManager.colliders)
                        ?? new List<GameObject>()
                );
                clothManager.contacts.AddRange(
                    dataSourceRef.contacts?.Except(clothManager.contacts) ?? new List<GameObject>()
                );
                clothManager.meshes.AddRange(
                    dataSourceRef.meshes?.Except(clothManager.meshes) ?? new List<GameObject>()
                );
            }

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            listReplace = EditorGUILayout.Toggle("Replace Data?", listReplace);

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(20);

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            GUILayout.Label("Selection To List Customization", _titleStyle);

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(5);

            if (GUILayout.Button(string.Format("Set Selection as {0} List", "PhysBones")))
            {
                clothManager.SelectionToList(clothManager.physBones, typeof(VRCPhysBone), true);
            }

            if (GUILayout.Button(string.Format("Set Selection as {0} List", "Colliders")))
            {
                clothManager.SelectionToList(
                    clothManager.physBones,
                    typeof(VRCPhysBoneCollider),
                    true
                );
            }

            if (GUILayout.Button(string.Format("Set Selection as {0} List", "Contacts")))
            {
                clothManager.SelectionToList(clothManager.contacts, typeof(VRCContactSender), true);
                clothManager.SelectionToList(
                    clothManager.contacts,
                    typeof(VRCContactReceiver),
                    false
                );
            }

            if (GUILayout.Button(string.Format("Set Selection as {0} List", "Meshes")))
            {
                clothManager.SelectionToList(
                    clothManager.meshes,
                    typeof(SkinnedMeshRenderer),
                    true
                );
                clothManager.SelectionToList(clothManager.meshes, typeof(MeshRenderer), false);
            }

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            selectionReplace = EditorGUILayout.Toggle("Replace Data?", selectionReplace);

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(20);

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            GUILayout.Label("Toggle GameObjects", _titleStyle);

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(5);

            if (GUILayout.Button("Toggle On"))
            {
                clothManager.ToggleObjects(false);
            }

            if (GUILayout.Button("Toggle Off"))
            {
                clothManager.ToggleObjects(false);
            }

            if (isA1STNamespace)
                LogoGUI();
        }

        public void DropdownList(bool targetBool, List<GameObject> targetList, string targetName)
        {
            if (targetBool)
            {
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
            }

            if (GUILayout.Button(string.Format("Select {0}", targetName)))
            {
                if (targetList.Count != 0)
                    Selection.objects = targetList.ToArray();
            }
        }
    }
}
#endif
