using System.Linq;
using UnityEditor;
using UnityEngine;

namespace AssetBundlesClass.Editor.AssetBundlesSystem
{
    [CustomEditor(typeof(DefaultAsset))]
    public class AssetBundlesSystemCustomEditor : UnityEditor.Editor
    {
        private const float _labelsWidth = 140F;
        private const float _defaultSpacing = 30F;
        
        private string[] _existingSharedBundles = default;
        private int _selectedIndex = 0;
        
        private string _newAssetBundleName = default;
        private string _newSharedBundleName = default;

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
            bool removeAssetBundle;

            GUI.enabled = true;
            
            EditorGUILayout.LabelField("Custom editor actions", EditorStyles.boldLabel);
            
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("New asset bundle name:", GUILayout.Width(_labelsWidth));
                _newAssetBundleName = EditorGUILayout.TextField(_newAssetBundleName);
                createAssetBundle = GUILayout.Button("Assign asset bundle");
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.Space(_defaultSpacing);

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Select a shared bundle:", GUILayout.Width(_labelsWidth));
                _selectedIndex = EditorGUILayout.Popup(_selectedIndex, _existingSharedBundles);
                assignSharedBundle = GUILayout.Button("Assign shared bundle");
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.Space(_defaultSpacing);

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("New shared bundle name:", GUILayout.Width(_labelsWidth));
                _newSharedBundleName = EditorGUILayout.TextField(_newSharedBundleName);
                createSharedBundle = GUILayout.Button("Create new shared bundle");
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.Space(_defaultSpacing);

            removeAssetBundle = GUILayout.Button("Remove asset bundle");

            // target here is the selected object viewed in the inspector
            if (createAssetBundle) AssetBundleAssigner.AssignBundleName(target, _newAssetBundleName, false);
            if (assignSharedBundle) AssetBundleAssigner.AssignBundleName(target, _existingSharedBundles[_selectedIndex], true);
            if (createSharedBundle) AssetBundleAssigner.AssignBundleName(target, _newSharedBundleName, true);
            if (removeAssetBundle) AssetBundleAssigner.RemoveAssetBundle(target);
        }
    }
}