using System.IO;
using AssetBundlesClass.Shared.Pools;
using UnityEditor;
using UnityEngine;

namespace AssetBundlesClass.Editor.EditorDefines
{
    public class ScriptingDefinesScriptableObject : ScriptableObject
    {
        private const string assetPath = "Assets/Contextual Assets/Editor/ScriptingDefinesConfig.asset";
        
        [SerializeField] public ScriptDefineInfo[] availableScriptingDefines = default;

        [MenuItem("Assets/Create/Scriptable Objects/Editor/ScriptingDefines")]
        public static ScriptingDefinesScriptableObject CreateOrLoadAsset()
        {
            if (File.Exists(assetPath)) return AssetDatabase.LoadAssetAtPath<ScriptingDefinesScriptableObject>(assetPath);

            ScriptingDefinesScriptableObject instance = CreateInstance<ScriptingDefinesScriptableObject>();
            instance.availableScriptingDefines = new ScriptDefineInfo[0];
            AssetDatabase.CreateAsset(instance, assetPath);
            EditorUtility.SetDirty(instance);
            AssetDatabase.SaveAssets();
            return instance;
        }

        public void Add(ScriptDefineInfo info)
        {
            using ListPool<ScriptDefineInfo> defines = ListPool<ScriptDefineInfo>.Rent(availableScriptingDefines);
            defines.Add(info);
            availableScriptingDefines = defines.ToArray();
            
            EditorUtility.SetDirty(this);
        }

        public void RemoveAt(int index)
        {
            using ListPool<ScriptDefineInfo> defines = ListPool<ScriptDefineInfo>.Rent(availableScriptingDefines);
            defines.RemoveAt(index);
            availableScriptingDefines = defines.ToArray();
            
            EditorUtility.SetDirty(this);
        }
    }
}