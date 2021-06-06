#if !UNITY_EDITOR || ENABLE_EDITOR_BUNDLES
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace AssetBundlesClass.Game.AssetBundlesSystem
{
    public abstract partial class AssetBundlesLoader
    {
        protected class AssetBundlesDownloader
        {
            public delegate IEnumerator DownloadCallback(bool success, string cacheFilePath);

            private readonly string _assetBundlesRootUrl;
            private readonly string _cacheFilePath;

            public AssetBundlesDownloader(string url, string cacheFilePath)
            {
                if (!url.StartsWith("file://")) url = $"file://{url}";
                _assetBundlesRootUrl = Path.Combine(url, AssetBundlesFileSystem.GetTargetName());
                _cacheFilePath = cacheFilePath;
            }

            public IEnumerator DownloadFromName(string fileName, DownloadCallback onComplete)
            {
                string url = Path.Combine(_assetBundlesRootUrl, fileName);
                yield return DownloadFromUrl(url, onComplete);
            }

            private IEnumerator DownloadFromUrl(string url, DownloadCallback onComplete)
            {
#if ENABLE_DEBUG_LOGS
                Debug.Log($"Downloading file at url: {url}\nSaving at path: {_cacheFilePath}");
#endif

                using UnityWebRequest request = new UnityWebRequest(url) {downloadHandler = new AssetBundlesDownloadHandler(_cacheFilePath)};
                UnityWebRequestAsyncOperation operation = request.SendWebRequest();
                while (!operation.isDone) yield return new WaitForEndOfFrame();

                bool success = request.result == UnityWebRequest.Result.Success;

#if ENABLE_DEBUG_LOGS
                Debug.Log($"Download request result: {request.result}");
                if (!success) Debug.LogError($"Download request error: {request.error}");
#endif

                yield return onComplete.Invoke(success, _cacheFilePath);
            }
        }

        private class AssetBundlesDownloadHandler : DownloadHandlerScript
        {
            private readonly FileStream _fileStream;

            public AssetBundlesDownloadHandler(string cacheFile)
            {
                if (File.Exists(cacheFile)) File.Delete(cacheFile);
                _fileStream = File.Create(cacheFile);
            }

            protected override bool ReceiveData(byte[] bytes, int dataLength)
            {
                _fileStream.Write(bytes, 0, dataLength);
                return true;
            }

            protected override void CompleteContent() => _fileStream.Close();
        }
    }
}
#endif
