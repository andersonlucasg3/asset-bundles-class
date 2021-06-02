using System.IO;
using UnityEditor;
using UnityEngine;

namespace AssetBundlesClass.Editor.AssetBundlesSystem
{
    public static class AssetBundlesBuilder
    {
        public static readonly string assetBundlesOutputPath = Path.Combine(Directory.GetParent(Application.dataPath)?.ToString() ?? string.Empty, "AssetBundles");
        
        [MenuItem("AssetBundlesClass/Asset Bundles/Build All Bundles (Active Platform Only)")]
        public static void AssetBundlesBuildActivePlatformMenuAction()
        {
            BuildAssetBundles(EditorUserBuildSettings.activeBuildTarget);
        }

        [MenuItem("AssetBundlesClass/Asset Bundles/Build All Bundles (All Supported Platforms)")]
        public static void AssetBundlesBuildSupportedPlatformsMenuAction()
        {
            BuildAssetBundles(BuildTarget.StandaloneWindows64);
            BuildAssetBundles(BuildTarget.StandaloneOSX);
            BuildAssetBundles(BuildTarget.iOS);
            BuildAssetBundles(BuildTarget.Android);

            Debug.Log($"The bundles should be at: {assetBundlesOutputPath}");
        }
        
        private static void BuildAssetBundles(BuildTarget target)
        {
            string platformSpecificOutputPath = Path.Combine(assetBundlesOutputPath, $"{target}");
            
            if (!Directory.Exists(platformSpecificOutputPath)) Directory.CreateDirectory(platformSpecificOutputPath);
            
            Debug.Log($"Building AssetBundles for {target} platform with output path: {platformSpecificOutputPath}");
            
            BuildPipeline.BuildAssetBundles(platformSpecificOutputPath, BuildAssetBundleOptions.ChunkBasedCompression, target);

            Debug.Log($"If nothing goes wrong, the asset bundles were built at path: {platformSpecificOutputPath}");
        }
    }
}