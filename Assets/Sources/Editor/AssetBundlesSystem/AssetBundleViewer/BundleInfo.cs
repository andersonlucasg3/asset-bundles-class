namespace AssetBundlesClass.Editor.AssetBundlesSystem.AssetBundleViewer
{
    public readonly struct BundleInfo
    {
        public readonly string bundleName;
        public readonly string displayableBundleName;
        public readonly bool isUnused;

        public BundleInfo(string bundleName, bool isUnused)
        {
            this.bundleName = bundleName;
            this.isUnused = isUnused;
            displayableBundleName = isUnused ? $"(unused) {bundleName}" : bundleName;
        }
    }
}