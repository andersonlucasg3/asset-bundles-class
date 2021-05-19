using System.IO;
using UnityEditor;
using UnityEngine;

namespace AssetBundlesClass
{
    public static class AssetBundleAssigner
    {
        [MenuItem("AssetBundlesClass/Asset Bundles/Assign Asset Bundles")]
        public static void AssignAssetBundles()
        {
            Object selectedObject = Selection.activeObject;

            if (!selectedObject) return;

            string assetPath = AssetDatabase.GetAssetPath(selectedObject);
            AssetImporter importer = AssetImporter.GetAtPath(assetPath);

            string pathWithoutExtension = assetPath.Replace(Path.GetExtension(assetPath), "");
            importer.SetAssetBundleNameAndVariant(pathWithoutExtension, "");
        }
    }
}