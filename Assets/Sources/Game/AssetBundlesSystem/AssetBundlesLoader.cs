using System;
using System.Collections;
using JetBrains.Annotations;
using Object = UnityEngine.Object;

namespace AssetBundlesClass.Game.AssetBundlesSystem
{
    public abstract partial class AssetBundlesLoader
    {
        public static AssetBundlesLoader shared { get; private set; } = default;

#if !UNITY_EDITOR || ENABLE_EDITOR_BUNDLES
        private readonly string _baseUrl = default;
#endif

#if UNITY_EDITOR && !ENABLE_EDITOR_BUNDLES
        public static void Initialize()
#else
        public static void Initialize(string assetBundlesBaseUrl)
#endif
        {
            if (shared != null) return;
            
#if UNITY_EDITOR && !ENABLE_EDITOR_BUNDLES
            shared ??= new AssetBundlesEditorLoader();
#else
            shared ??= new AssetBundlesRuntimeLoader(assetBundlesBaseUrl);
#endif
        }

#if !UNITY_EDITOR || ENABLE_EDITOR_BUNDLES
        protected AssetBundlesLoader(string baseUrl) => _baseUrl = baseUrl;
#endif

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
