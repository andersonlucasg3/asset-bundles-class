using UnityEditor;
using UnityEngine;

namespace AssetBundlesClass.AssetBundlesSystem
{
    public static class AssetBundleAssigner
    {
        [MenuItem("AssetBundlesClass/Asset Bundles/Assign Asset Bundles")]
        public static void AssignAssetBundleMenuAction()
        {
            Object selectedObject = Selection.activeObject;
            AssignAssetBundle(selectedObject);
        }

        public static void AssignAssetBundle(Object selectedObject)
        {
            if (!selectedObject) return;

            AssetBundleInfo info = new AssetBundleInfo(selectedObject);
            AssignBundle(info);
        }

        public static void AssignSharedBundle(Object selectedObject, string sharedBundle)
        {
            if (!selectedObject) return;

            AssetBundleInfo info = new AssetBundleInfo(selectedObject, sharedBundle);
            AssignBundle(info);
        }

        public static void CreateSharedBundle(Object selectedObject)
        {
            if (!selectedObject) return;
            
            AssetBundleInfo info = new AssetBundleInfo(selectedObject, true);
            AssignBundle(info);
        }

        private static void AssignBundle(AssetBundleInfo info) 
            => info.importer.SetAssetBundleNameAndVariant(info.assetBundleName, "");
    }
}