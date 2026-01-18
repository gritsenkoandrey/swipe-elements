using System.IO;
using SwipeElements.Game.Views;
using SwipeElements.Infrastructure.Serialize;
using UnityEditor;
using UnityEngine;

namespace SwipeElements.Game.Scripts.Infrastructure.Serialize.Editor
{
    public sealed class SerializeTool : EditorWindow
    {
        private string _savePath = "Assets/Game/Prefabs/Game/Levels/";
        private string _fileName = "Level_00.json";

        private LevelView _levelView;
        
        [MenuItem("Tools/Serialize Level")]
        public static void ShowWindow()
        {
            GetWindow<SerializeTool>("Serialize Tool");
        }

        private void OnGUI()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Serialize Tool", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            
            _savePath = EditorGUILayout.TextField("Save Path", _savePath);
            _fileName = EditorGUILayout.TextField("File Name", _fileName);
            _levelView = (LevelView)EditorGUILayout.ObjectField("Level View", _levelView, typeof(LevelView), true);
            
            EditorGUILayout.Space();
            
            EditorGUILayout.BeginHorizontal();
            
            if (GUILayout.Button("Serialize", GUILayout.Width(120)))
            {
                Serialize();
            }
            
            EditorGUILayout.EndHorizontal();
        }

        private void Serialize()
        {
            if (_levelView == null)
            {
                Debug.LogError("Level View is null!");
                
                return;
            }
            
            string json = _levelView.Serialize();
            
            string fullPath = Path.Combine(_savePath, _fileName);
            
            if (Directory.Exists(_savePath) == false)
            {
                Directory.CreateDirectory(_savePath);
            }
            
            File.WriteAllText(fullPath, json);
            
            AssetDatabase.Refresh();
            
            Debug.Log($"Level saved to {fullPath}");
        }
    }
}