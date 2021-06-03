using AssetBundlesClass.Extensions;
using UnityEditor;
using UnityEngine;

namespace AssetBundlesClass.Editor.EditorDefines
{
    public class EditorDefinesWindow : EditorWindow
    {
        private ScriptingDefinesScriptableObject _scriptingDefinesScriptableObject = default;
        
        private ScriptDefineInfo _defineToAdd = null;
        private int? _indexToDelete = default;
        private string _newScriptDefineName = default;
        private Vector2 _scrollPosition = default;
        private bool _applyScriptingDefines = default;
        
        [MenuItem("Window/Asset Bundles Class/Open Defines Window")]
        private static void OpenWindow() => GetWindow<EditorDefinesWindow>().Show();

        private void Awake() => _scriptingDefinesScriptableObject = ScriptingDefinesScriptableObject.CreateOrLoadAsset();

        private void OnFocus()
        {
            _defineToAdd = null;
            if (_scriptingDefinesScriptableObject) return; 
            _scriptingDefinesScriptableObject = ScriptingDefinesScriptableObject.CreateOrLoadAsset();
        }

        private void OnInspectorUpdate()
        {
            if (!_scriptingDefinesScriptableObject) return;
            
            if (_defineToAdd != null)
            {
                _scriptingDefinesScriptableObject.Add(_defineToAdd);
                _defineToAdd = null;
            }
            
            if (_indexToDelete.HasValue)
            {
                _scriptingDefinesScriptableObject.RemoveAt(_indexToDelete.Value);
                _indexToDelete = null;
            }

            if (!_applyScriptingDefines) return;
            _applyScriptingDefines = false;
            string[] scriptingDefines = _scriptingDefinesScriptableObject.availableScriptingDefines.FilterMap((ScriptDefineInfo value, out string result) =>
            {
                result = value.enabled ? value.name : null;
                return value.enabled;
            });
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, scriptingDefines);
            AssetDatabase.RefreshSettings();
        }

        private void OnGUI()
        {
            if (!_scriptingDefinesScriptableObject) return;
            
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            {
                EditorGUILayout.BeginVertical();
                {
                    for (int index = 0; index < _scriptingDefinesScriptableObject.availableScriptingDefines.Length; index++)
                    {
                        ScriptDefineInfo current = _scriptingDefinesScriptableObject.availableScriptingDefines[index];
                        EditorGUILayout.BeginHorizontal();
                        {
                            current.enabled = EditorGUILayout.ToggleLeft(current.name, current.enabled);
                            EditorGUILayout.Space();
                            if (GUILayout.Button("-", GUILayout.Width(30F))) _indexToDelete = index;
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    
                    EditorGUILayout.Separator();

                    _applyScriptingDefines ^= GUILayout.Button("Apply Scripting Defines", GUILayout.Height(40F));

                    EditorGUILayout.Separator();

                    _newScriptDefineName = EditorGUILayout.TextField(_newScriptDefineName);
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.Space();
                        if (GUILayout.Button("Add new") && !string.IsNullOrEmpty(_newScriptDefineName))
                        {
                            _newScriptDefineName = "";
                            _defineToAdd = new ScriptDefineInfo(_newScriptDefineName);
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndScrollView();
        }
    }
}