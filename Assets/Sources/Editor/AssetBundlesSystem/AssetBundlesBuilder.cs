using System.IO;
using AssetBundlesClass.Game.AssetBundlesSystem;
using UnityEditor;
using UnityEngine;

namespace AssetBundlesClass.Editor.AssetBundlesSystem
{
    public static class AssetBundlesBuilder
    {
        public static readonly string assetBundlesOutputPath = Path.Combine(Directory.GetParent(Application.dataPath)?.ToString() ?? string.Empty, "AssetBundles");

        [MenuItem("AssetBundlesClass/Asset Bundles/Clean Built Bundles (All)")]
        public static void AssetBundlesCleanAll()
        {
            if (!Directory.Exists(assetBundlesOutputPath)) return;
            Directory.Delete(assetBundlesOutputPath, true);
            Debug.Log("Asset bundles path deleted!");
        }

        [MenuItem("AssetBundlesClass/Asset Bundles/Clean cached bundles")]
        public static void AssetBundlesCleanCachedBundles()
        {
            if (!Directory.Exists(AssetBundlesFileSystem.assetBundlesRootPath)) return;
            Directory.Delete(AssetBundlesFileSystem.assetBundlesRootPath, true);
        }
        
        [MenuItem("AssetBundlesClass/Asset Bundles/Build Bundles (Active Platform Only)")]
        public static void AssetBundlesBuildActivePlatformMenuAction() => BuildAssetBundles();

        [MenuItem("AssetBundlesClass/Asset Bundles/Build Bundles (All Supported Platforms)")]
        public static void AssetBundlesBuildSupportedPlatformsMenuAction()
        {
            BuildAssetBundles(BuildTarget.StandaloneWindows64);
            BuildAssetBundles(BuildTarget.StandaloneOSX);
            BuildAssetBundles(BuildTarget.iOS);
            BuildAssetBundles(BuildTarget.Android);

            Debug.Log($"The bundles should be at: {assetBundlesOutputPath}");
        }

        public static string GetPlatformSpecificOutputPath(BuildTarget? buildTarget = null) => Path.Combine(assetBundlesOutputPath, $"{buildTarget ?? EditorUserBuildSettings.activeBuildTarget}");

        private static void BuildAssetBundles(BuildTarget? buildTarget = null)
        {
            buildTarget ??= EditorUserBuildSettings.activeBuildTarget;
            string platformSpecificOutputPath = GetPlatformSpecificOutputPath(buildTarget);
            
            if (!Directory.Exists(platformSpecificOutputPath)) Directory.CreateDirectory(platformSpecificOutputPath);
            
            Debug.Log($"Building AssetBundles for {buildTarget} platform with output path: {platformSpecificOutputPath}");

            BuildPipeline.BuildAssetBundles(platformSpecificOutputPath, BuildAssetBundleOptions.ChunkBasedCompression, buildTarget.Value);

            Debug.Log($"If nothing goes wrong, the asset bundles were built at path: {platformSpecificOutputPath}");
        }
    }
}