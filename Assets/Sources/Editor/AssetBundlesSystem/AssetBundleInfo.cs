using UnityEditor;
using UnityEngine;

namespace AssetBundlesClass.AssetBundlesSystem
{
    class AssetBundleInfo
    {
        private const string _sharedBundleIdentifier = "_shared";
        
        public readonly string path;
        public readonly string assetBundleName;
        public readonly AssetImporter importer;

        public AssetBundleInfo(Object selectedObject, string bundleName, bool isShared = false)
        {
            path = AssetDatabase.GetAssetPath(selectedObject);
            importer = AssetImporter.GetAtPath(path);
            
            assetBundleName = isShared && !bundleName.StartsWith(_sharedBundleIdentifier) ? $"{_sharedBundleIdentifier}/{bundleName}" : bundleName;
        }

        public AssetBundleInfo(Object selectedObject)
        {
            path = AssetDatabase.GetAssetPath(selectedObject);
            importer = AssetImporter.GetAtPath(path);

            assetBundleName = importer.assetBundleName;
        }
    }
}