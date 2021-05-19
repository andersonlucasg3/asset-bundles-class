using System;
using System.Threading.Tasks;
using AssetBundlesClass.Extensions;
using UnityEditor;
using UnityEngine;

namespace AssetBundlesClass.Editor.AssetBundlesSystem.AssetBundleViewer
{
    public class AssetBundleViewerWindow : EditorWindow
    {
        private int _lastSelectedIndex = -1;
        private int _selectedIndex = default;
        private BundleInfo[] _bundleInfos = default;

        private readonly AssetBundlePingSelector _pingSelector = new AssetBundlePingSelector();

        [MenuItem("Window/Asset Bundles Class/AssetBundleViewer")]
        public static void OpenViewerWindowMenuAction() => GetWindow<AssetBundleViewerWindow>().Show();

        private void Awake() => titleContent.text = "Asset Bundle Viewer";

        private void OnFocus() => RefreshData();

        private void OnGUI()
        {
            EditorGUI.indentLevel = 1;

            EditorGUILayout.LabelField("Existing asset bundles:", EditorStyles.boldLabel);
            
            EditorGUILayout.Separator();

            DrawListOfBundles();
            
            EditorGUILayout.Separator();
            
            DrawActionButtons(out bool removeUnusedBundles, out bool deleteSelectedBundle);

            EditorGUILayout.Space(50F);
            
            _pingSelector.OnGUI();

            if (deleteSelectedBundle) DeleteSelectedBundle();
            if (removeUnusedBundles) RemoveUnusedBundles();
        }

        private void DrawActionButtons(out bool removeUnusedBundles, out bool deleteSelectedBundle)
        {
            if (_bundleInfos.Length == 0)
            {
                removeUnusedBundles = false;
                deleteSelectedBundle = false;
                return;
            }
            
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.Space();

                deleteSelectedBundle = GUILayout.Button("Delete selected bundle");

                EditorGUILayout.Separator();

                removeUnusedBundles = GUILayout.Button("Remove unused bundles");

                EditorGUILayout.Space(20F);
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawListOfBundles()
        {
            if (_bundleInfos.Length == 0)
            {
                EditorGUILayout.LabelField("There are no bundles to display, please try adding some bundle first.");
                return;
            }
            
            for (int index = 0; index < _bundleInfos.Length; index++)
            {
                EditorGUILayout.BeginHorizontal();
                {
                    BundleInfo info = _bundleInfos[index];
                    if (EditorGUILayout.ToggleLeft(info.isUnused ? "(unused)" : "", _selectedIndex == index, EditorStyles.boldLabel, GUILayout.Width(80F)))
                        _selectedIndex = index;

                    EditorGUILayout.LabelField(info.bundleName);
                }
                EditorGUILayout.EndHorizontal();
            }
            
            PingSelectedBundle();
        }

        private void DeleteSelectedBundle()
        {
            BundleInfo bundleInfo = _bundleInfos[_selectedIndex];

            if (!AssetDatabase.RemoveAssetBundleName(bundleInfo.bundleName, true)) 
                Debug.LogError($"Could not remove bundle with name: {bundleInfo.bundleName}");

            RefreshData();
        }

        private void PingSelectedBundle()
        {
            if (_lastSelectedIndex == _selectedIndex) return;
            _lastSelectedIndex = _selectedIndex;
            _pingSelector.ReloadData(_bundleInfos[_selectedIndex]);
        }

        private void RemoveUnusedBundles()
        {
            AssetDatabase.RemoveUnusedAssetBundleNames();
            RefreshData();
        }
        
        private void RefreshData()
        {
            string[] existingBundles = AssetDatabase.GetAllAssetBundleNames();
            string[] unusedBundles = AssetDatabase.GetUnusedAssetBundleNames();
            _bundleInfos = new BundleInfo[existingBundles.Length];

            Parallel.For(0, existingBundles.Length, index =>
            {
                string current = existingBundles[index];
                _bundleInfos[index] = new BundleInfo(current, unusedBundles.Contains(current));
            });

            _selectedIndex = 0;
            _lastSelectedIndex = -1;
            
            _pingSelector.Clear();
        }
    }
}