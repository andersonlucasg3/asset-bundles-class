#if !ENABLE_EDITOR_BUNDLES && UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using AssetBundlesClass.Extensions;
using AssetBundlesClass.Shared.Pools;
using UnityEditor;
using Object = UnityEngine.Object;

namespace AssetBundlesClass.Game.AssetBundlesSystem
{
    public abstract partial class AssetBundlesLoader
    {
        protected class AssetBundlesEditorLoader : AssetBundlesLoader
        {
            private readonly Dictionary<string, string[]> _assetBundleFilePathsMap = new Dictionary<string, string[]>();
            
            public override IEnumerator LoadAssetBundle(string assetBundleName, Func<bool, IEnumerator> onComplete)
            {
                if (!_assetBundleFilePathsMap.ContainsKey(assetBundleName)) 
                    _assetBundleFilePathsMap[assetBundleName] = AssetDatabase.GetAssetPathsFromAssetBundle(assetBundleName);
                yield return onComplete.Invoke(true);
            }

            public override void UnloadAll() => _assetBundleFilePathsMap.Clear();

            public override bool HasCache(string assetBundleName) => _assetBundleFilePathsMap.ContainsKey(assetBundleName);
            public override bool IsAssetBundleLoaded(string assetBundleName) => _assetBundleFilePathsMap.ContainsKey(assetBundleName);

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
            
            private IEnumerator TryLoad<TAsset>(string assetBundleName, string assetName, Func<TAsset, IEnumerator> onComplete) where TAsset : Object
            {
                if (_assetBundleFilePathsMap.TryGetValue(assetBundleName, out string[] assetBundle))
                {
                    string found = assetBundle.Find(each => each.EndsWith(assetName));
                    yield return onComplete.Invoke(AssetDatabase.LoadAssetAtPath<TAsset>(found));                    
                }
                else yield return onComplete?.Invoke(null);
            }

            private IEnumerator TryLoadMany<TAsset>(string assetBundleName, string[] assetNames, Func<TAsset[], IEnumerator> onComplete) where TAsset : Object
            {
                if (!_assetBundleFilePathsMap.TryGetValue(assetBundleName, out string[] assetBundle))
                    yield return onComplete.Invoke(null);
                else
                {
                    using ListPool<TAsset> resultAssets = ListPool<TAsset>.Rent();
                    for (int index = 0; index < assetNames.Length; index++)
                    {
                        string current = assetNames[index];
                        string found = assetBundle.Find(each => each.EndsWith(current));
                        resultAssets.Add(AssetDatabase.LoadAssetAtPath<TAsset>(found));
                    }
                    yield return onComplete.Invoke(resultAssets.ToArray());
                }
            }
        }
    }
}
#endif
