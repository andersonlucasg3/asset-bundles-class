#if !UNITY_EDITOR || ENABLE_EDITOR_BUNDLES
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using AssetBundlesClass.Shared.Pools;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AssetBundlesClass.Game.AssetBundlesSystem
{
    internal class __AssetBundlesRuntimeLoader : AssetBundlesLoader
    {
        private readonly Dictionary<string, AssetBundle> _assetBundles = new Dictionary<string, AssetBundle>();

        protected internal __AssetBundlesRuntimeLoader(string baseUrl) : base(baseUrl) { }

        public override IEnumerator LoadAssetBundle(string assetBundleName, Func<bool, IEnumerator> onComplete)
        {
            if (HasCache(assetBundleName))
            {
                if (!IsAssetBundleLoaded(assetBundleName)) 
                    yield return BundleDownloadCompleted(true, AssetBundlesFileSystem.GetFullPath(assetBundleName));
                yield return onComplete.Invoke(true);
            }
            else
            {
                AssetBundlesDownloader downloader = new AssetBundlesDownloader(_baseUrl, AssetBundlesFileSystem.GetFullPath(assetBundleName));
                yield return downloader.DownloadFromName(assetBundleName, BundleDownloadCompleted);
                yield return onComplete.Invoke(_assetBundles.ContainsKey(assetBundleName));                
            }
        }

        public override void UnloadAll()
        {
            foreach (AssetBundle bundles in _assetBundles.Values) bundles.Unload(true);
        }

        public override bool HasCache(string assetBundleName) => AssetBundlesFileSystem.AssetBundleExists(assetBundleName);
        public override bool IsAssetBundleLoaded(string assetBundleName) => _assetBundles.ContainsKey(assetBundleName);

        public override IEnumerator Load<TAsset>(string assetBundleName, string assetName, Func<TAsset, IEnumerator> onComplete)
        {
            if (!HasCache(assetBundleName))
            {
                yield return onComplete.Invoke(null);
                yield break;
            }

            if (IsAssetBundleLoaded(assetBundleName)) 
                yield return TryLoad(assetBundleName, assetName, onComplete);
            else
            {
                IEnumerator OnComplete(bool success)
                {
                    if (!success) throw new ArgumentException("Should be true.", nameof(success));
                    yield return TryLoad(assetBundleName, assetName, onComplete);
                }

                yield return LoadAssetBundle(assetBundleName, OnComplete);
            }
        }

        public override IEnumerator LoadMany<TAsset>(string assetBundleName, string[] assetNames, Func<TAsset[], IEnumerator> onComplete)
        {
            if (!HasCache(assetBundleName))
            {
                yield return onComplete.Invoke(null);
                yield break;
            }

            if (IsAssetBundleLoaded(assetBundleName))
                yield return TryLoadMany(assetBundleName, assetNames, onComplete);
            else
            {
                IEnumerator OnComplete(bool success)
                {
                    if (!success) throw new ArgumentException("Should be true.", nameof(success));
                    yield return TryLoadMany(assetBundleName, assetNames, onComplete);
                }

                yield return LoadAssetBundle(assetBundleName, OnComplete);
            }
        }

        private IEnumerator BundleDownloadCompleted(bool success, string filePath)
        {
            if (!success) throw new ArgumentException("Should be true", nameof(success));
            AssetBundleCreateRequest operation = AssetBundle.LoadFromFileAsync(filePath);
            while (!operation.isDone) yield return new WaitForEndOfFrame();
            if (!operation.assetBundle) yield break;
            string assetBundleName = Path.GetFileNameWithoutExtension(filePath);
            _assetBundles.Add(assetBundleName, operation.assetBundle);
        }

        private IEnumerator TryLoad<TAsset>(string assetBundleName, string assetName, Func<TAsset, IEnumerator> onComplete) where TAsset : Object
        {
            if (_assetBundles.TryGetValue(assetBundleName, out AssetBundle assetBundle)) 
                yield return onComplete.Invoke(assetBundle.LoadAsset<TAsset>(assetName));
            else yield return onComplete?.Invoke(null);
        }

        private IEnumerator TryLoadMany<TAsset>(string assetBundleName, string[] assetNames, Func<TAsset[], IEnumerator> onComplete) where TAsset : Object
        {
            if (!_assetBundles.TryGetValue(assetBundleName, out AssetBundle assetBundle)) 
                yield return onComplete.Invoke(null);
            else
            {
                using ListPool<TAsset> resultAssets = ListPool<TAsset>.Rent();
                for (int index = 0; index < assetNames.Length; index++)
                {
                    resultAssets.Add(assetBundle.LoadAsset<TAsset>(assetNames[index]));
                    yield return new WaitForEndOfFrame();
                }
                yield return onComplete.Invoke(resultAssets.ToArray());                
            }
        }
    }
}
#endif