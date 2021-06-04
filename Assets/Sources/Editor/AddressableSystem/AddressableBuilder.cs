using UnityEditor;
using UnityEditor.AddressableAssets.Settings;

namespace AssetBundlesClass.Editor.AddressableSystem
{
    public class AddressableBuilder
    {
        [MenuItem("AssetBundlesClass/Addressable/Build Addressable")]
        public static void BuildAddressable()
        {
            AddressableAssetSettings.CleanPlayerContent();
            AddressableAssetSettings.BuildPlayerContent();
        }
    }
}