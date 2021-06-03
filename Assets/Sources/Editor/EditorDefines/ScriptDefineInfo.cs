using System;
using UnityEngine;

namespace AssetBundlesClass.Editor.EditorDefines
{
    [Serializable]
    public class ScriptDefineInfo
    {
        [field: SerializeField] public string name { get; private set; }
        public bool enabled;

        public ScriptDefineInfo(string name)
        {
            this.name = name;
            enabled = false;
        }
    }
}