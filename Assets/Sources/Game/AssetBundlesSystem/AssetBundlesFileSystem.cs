#if !UNITY_EDITOR || ENABLE_EDITOR_BUNDLES
using System.IO;
using UnityEditor;
using UnityEngine;

namespace AssetBundlesClass.Game.AssetBundlesSystem
{
    public abstract partial class AssetBundlesLoader
    {
        public static class AssetBundlesFileSystem
        {
            private static readonly string assetBundlesPlatformRootPath = default;

            public static readonly string assetBundlesRootPath = default;

            static AssetBundlesFileSystem()
            {
#if UNITY_EDITOR
                string targetName = GetTargetName();
#else
            string targetName = GetTargetName();
#endif
                assetBundlesRootPath = Path.Combine(Application.persistentDataPath, "AssetBundles");
                assetBundlesPlatformRootPath = Path.Combine(assetBundlesRootPath, targetName);
                if (!Directory.Exists(assetBundlesPlatformRootPath)) Directory.CreateDirectory(assetBundlesPlatformRootPath);
            }

            public static bool AssetBundleExists(string assetBundleName) => File.Exists(Path.Combine(assetBundlesPlatformRootPath, assetBundleName));

            public static string GetFullPath(string relativeFilePath) => Path.Combine(assetBundlesPlatformRootPath, relativeFilePath);

#if UNITY_EDITOR
            public static string GetTargetName() => $"{EditorUserBuildSettings.activeBuildTarget}";
#else
        public static string GetTargetName() => Application.platform switch
        {
            RuntimePlatform.OSXPlayer => "StandaloneOSX",
            RuntimePlatform.WindowsPlayer => "StandaloneWindows64",
            RuntimePlatform.IPhonePlayer => "iOS",
            RuntimePlatform.Android => "Android",
            _ => "Unknown"
        };
#endif
        }
    }
}
#endif