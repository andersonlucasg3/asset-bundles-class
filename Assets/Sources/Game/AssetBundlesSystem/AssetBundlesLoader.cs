using System;
using System.Collections;
using JetBrains.Annotations;
using Object = UnityEngine.Object;

namespace AssetBundlesClass.Game.AssetBundlesSystem
{
    public abstract class AssetBundlesLoader
    {
        public static AssetBundlesLoader shared { get; private set; } = default;

        protected readonly string _baseUrl = default;
        
        public static void Initialize(string assetBundlesBaseUrl)
        {
            if (shared != null) return;
            
#if UNITY_EDITOR && !ENABLE_EDITOR_BUNDLES
            shared ??= new __AssetBundlesEditorLoader();
#else
            shared ??= new __AssetBundlesRuntimeLoader(assetBundlesBaseUrl);
#endif
        }

        protected AssetBundlesLoader(string baseUrl) => _baseUrl = baseUrl;

        [UsedImplicitly]
        public abstract IEnumerator LoadAssetBundle(string assetBundleName, Func<bool, IEnumerator> onComplete);

        [UsedImplicitly]
        public abstract void UnloadAll();

        [UsedImplicitly]
        public abstract bool HasCache(string assetBundleName);

        [UsedImplicitly]
        public abstract bool IsAssetBundleLoaded(string assetBundleName);

        [UsedImplicitly]
        public abstract IEnumerator Load<TObject>(string assetBundleName, string assetName, Func<TObject, IEnumerator> onComplete) where TObject : Object;

        [UsedImplicitly]
        public abstract IEnumerator LoadMany<TObject>(string assetBundleName, string[] assetNames, Func<TObject[], IEnumerator> onComplete) where TObject : Object;
    }
}