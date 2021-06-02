using System.IO;
using AssetBundlesClass.Editor.AssetBundlesSystem;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace AssetBundlesClass.Editor.BuildSystem
{
    public static class GameBuilder
    {
        private static readonly string _activeBuildTargetName = $"{EditorUserBuildSettings.activeBuildTarget}";
        private static readonly string _buildOutputPath = Path.Combine(Directory.GetParent(Application.dataPath)?.ToString() ?? "", "builds");
        private static readonly string _buildPlatformPath = Path.Combine(_buildOutputPath, _activeBuildTargetName);
        
        [MenuItem("AssetBundlesClass/Game Builder/Build (Current Platform)")]
        public static void BuildCurrentPlatform()
        {
            if (!Directory.Exists(_buildPlatformPath)) Directory.CreateDirectory(_buildPlatformPath);
            BuildPlayerOptions playerOptions = new BuildPlayerOptions
            {
                options = BuildOptions.Development,
                target = EditorUserBuildSettings.activeBuildTarget,
                targetGroup = EditorUserBuildSettings.selectedBuildTargetGroup,
                locationPathName = _buildPlatformPath,
                assetBundleManifestPath = Path.Combine(AssetBundlesBuilder.assetBundlesOutputPath, _activeBuildTargetName),
            };
            BuildReport report = BuildPipeline.BuildPlayer(playerOptions);
            Debug.Log(report.summary);
        }
    }
}