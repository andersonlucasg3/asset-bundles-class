using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AssetBundlesClass.Editor.AssetBundlesSystem.AssetBundleViewer
{
    public class AssetBundlePingSelector
    {
        private static readonly string[] _emptyArray = Array.Empty<string>();
        
        private Vector2 _scrollPosition = default;
        private int _lastSelectedIndex = -1;
        private int _selectedIndex = default;
        private string[] _pathsToDisplay = default;
        private BundleInfo _bundleInfo = default;

        public void ReloadData(BundleInfo bundleInfo)
        {
            _bundleInfo = bundleInfo;
            _pathsToDisplay = AssetDatabase.GetAssetPathsFromAssetBundle(bundleInfo.bundleName);
            _selectedIndex = 0;
            _lastSelectedIndex = -1;
        }

        public void Clear() => _pathsToDisplay = _emptyArray;

        public void OnGUI()
        {
            if (_pathsToDisplay.Length == 0) return;

            EditorGUILayout.BeginHorizontal();
            {
                int size = EditorStyles.boldLabel.fontSize;
                EditorStyles.boldLabel.fontSize = (int) (size * 1.2F);
                EditorGUILayout.LabelField("Selected asset bundle: ", EditorStyles.largeLabel, GUILayout.ExpandWidth(false));
                EditorGUILayout.LabelField(_bundleInfo.bundleName, EditorStyles.boldLabel, GUILayout.ExpandWidth(false));
                EditorStyles.boldLabel.fontSize = size;
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();

            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            {
                for (int index = 0; index < _pathsToDisplay.Length; index++)
                {
                    if (EditorGUILayout.ToggleLeft(_pathsToDisplay[index], _selectedIndex == index))
                        _selectedIndex = index;
                }
            }
            EditorGUILayout.EndScrollView();

            PingSelectedIndex();
        }

        private void PingSelectedIndex()
        {
            if (_lastSelectedIndex == _selectedIndex) return;
            _lastSelectedIndex = _selectedIndex;
            string assetPath = _pathsToDisplay[_selectedIndex];
            Object asset = AssetDatabase.LoadAssetAtPath<Object>(assetPath);
            EditorGUIUtility.PingObject(asset);
        } 
    }
}