#if !ENABLE_EDITOR_BUNDLES
using System;
using System.Collections;
using AssetBundlesClass.Extensions;
using AssetBundlesClass.Shared.Pools;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AssetBundlesClass.Game.AssetBundlesSystem
{
    public class __AssetBundlesEditorLoader : AssetBundlesLoader
    {
        private string[] _allAssetBundles = default;
        private string[] _allAssetBundleAssets = default;

        public IEnumerator LoadRootBundle(Action<bool> onComplete)
        {
            _allAssetBundles = AssetDatabase.GetAllAssetBundleNames();
            using ListPool<string> allBundleFiles = ListPool<string>.Rent();
            for (int index = 0; index < _allAssetBundles.Length; index++)
            {
                string[] assetPathsFromBundle = AssetDatabase.GetAssetPathsFromAssetBundle(_allAssetBundles[index]);
                allBundleFiles.AddRange(assetPathsFromBundle);
                yield return new WaitForEndOfFrame();
            }
            _allAssetBundleAssets = allBundleFiles.ToArray();
            
            onComplete.Invoke(true);
        }

        public bool CanLoadFrom(string assetBundleName) => true;

        public TObject Load<TObject>(string name) where TObject : Object
        {
            string found = _allAssetBundleAssets.Find(each => each.EndsWith(name));
            return string.IsNullOrEmpty(found) ? null : AssetDatabase.LoadAssetAtPath<TObject>(found);
        }
    }
}
#endif