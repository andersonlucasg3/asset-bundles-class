using System.IO;
using UnityEditor;
using UnityEngine;

namespace AssetBundlesClass.AssetBundlesSystem
{
    class AssetBundleInfo
    {
        public readonly string path;
        public readonly string assetBundleName;
        public readonly AssetImporter importer;

        public AssetBundleInfo(Object selectedObject, bool isShared = false)
        {
            path = AssetDatabase.GetAssetPath(selectedObject);
            importer = AssetImporter.GetAtPath(path);

            string bundleName = RemoveExtensionIfNeeded(path).ToLower();
            assetBundleName = isShared ? $"_shared/{bundleName}" : bundleName;
        }

        public AssetBundleInfo(Object selectedObject, string existingBundleName)
        {
            path = AssetDatabase.GetAssetPath(selectedObject);
            importer = AssetImporter.GetAtPath(path);

            assetBundleName = existingBundleName;
        }

        private static string RemoveExtensionIfNeeded(string path)
        {
            if (!Path.HasExtension(path)) return path;
            string extension = Path.GetExtension(path);
            return path.Replace(extension, "");
        }
    }
}