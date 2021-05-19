using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace AssetBundlesClass.AssetBundlesSystem
{
    [CustomEditor(typeof(DefaultAsset))]
    public class AssetBundlesSystemCustomEditor : Editor
    {
        private string[] _existingSharedBundles = default;
        private int _selectedIndex = 0;

        private void OnEnable()
        {
            // please do not ever do LINQ in a game
            string[] sharedBundles = AssetDatabase.GetAllAssetBundleNames().Select(each => each.StartsWith("_shared") ? each : null).ToArray();
            _existingSharedBundles = sharedBundles;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            bool createAssetBundle;
            bool assignSharedBundle;
            bool createSharedBundle;

            GUI.enabled = true;
            
            EditorGUILayout.LabelField("Custom editor actions", EditorStyles.boldLabel);
            
            EditorGUILayout.BeginHorizontal();
            {
                createAssetBundle = GUILayout.Button("Assign asset bundle");
                EditorGUILayout.Space();
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.Separator();

            _selectedIndex = EditorGUILayout.Popup("Select a shared bundle:", _selectedIndex, _existingSharedBundles);
            EditorGUILayout.BeginHorizontal();
            {
                assignSharedBundle = GUILayout.Button("Assign shared bundle");
                createSharedBundle = GUILayout.Button("Create new shared bundle");
            }
            EditorGUILayout.EndHorizontal();

            // target here is the selected object viewed in the inspector
            if (createAssetBundle) AssetBundleAssigner.AssignAssetBundle(target);
            if (assignSharedBundle) AssetBundleAssigner.AssignSharedBundle(target, _existingSharedBundles[_selectedIndex]);
            if (createSharedBundle) AssetBundleAssigner.CreateSharedBundle(target);
        }
    }
}