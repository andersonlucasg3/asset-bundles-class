using System.IO;
using UnityEditor;
using UnityEngine;

namespace AssetBundlesClass.Editor.AssetBundlesSystem
{
    public static class AssetBundlesBuilder
    {
        private static readonly string _assetBundlesOutputPath = Path.Combine(Directory.GetParent(Application.dataPath)?.ToString() ?? string.Empty, "AssetBundles");
        
        [MenuItem("AssetBundlesClass/Asset Bundles/Build All Bundles")]
        public static void AssetBundlesBuildMenuAction()
        {
            if (!Directory.Exists(_assetBundlesOutputPath)) Directory.CreateDirectory(_assetBundlesOutputPath);
            
            BuildPipeline.BuildAssetBundles(_assetBundlesOutputPath, BuildAssetBundleOptions.None, BuildTarget.StandaloneOSX);
        }
    }
}