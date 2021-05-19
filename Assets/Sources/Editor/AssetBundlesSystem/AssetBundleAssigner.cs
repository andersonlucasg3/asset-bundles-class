using UnityEngine;

namespace AssetBundlesClass.Editor.AssetBundlesSystem
{
    public static class AssetBundleAssigner
    {
        public static void AssignBundleName(Object selectedObject, string bundleName, bool shared)
        {
            if (!selectedObject) return;

            AssetBundleInfo info = new AssetBundleInfo(selectedObject, bundleName, shared);
            AssignBundle(info);
        }

        public static void RemoveAssetBundle(Object selectedObject)
        {
            AssetBundleInfo info = new AssetBundleInfo(selectedObject);
            info.importer.SetAssetBundleNameAndVariant(null, null);
        }

        private static void AssignBundle(AssetBundleInfo info) 
            => info.importer.SetAssetBundleNameAndVariant(info.assetBundleName, "");
    }
}