using System.IO;
using AssetBundlesClass.Editor.AssetBundlesSystem;
using AssetBundlesClass.Extensions;
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
                scenes = EditorBuildSettings.scenes.FilterMap((EditorBuildSettingsScene each, out string result) =>
                {
                    result = each.enabled ? each.path : null;
                    return each.enabled;
                }),
                options = BuildOptions.Development | BuildOptions.CompressWithLz4,
                target = EditorUserBuildSettings.activeBuildTarget,
                targetGroup = EditorUserBuildSettings.selectedBuildTargetGroup,
                locationPathName = Path.Combine(_buildPlatformPath, "game"),
                assetBundleManifestPath = Path.Combine(AssetBundlesBuilder.GetPlatformSpecificOutputPath(), $"{_activeBuildTargetName}.manifest"),
            };
            BuildReport report = BuildPipeline.BuildPlayer(playerOptions);
            Debug.Log(report.summary.result);
        }
    }
}