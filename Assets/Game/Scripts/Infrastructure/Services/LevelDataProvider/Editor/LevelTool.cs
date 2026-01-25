using System.Collections.Generic;
using System.IO;
using System.Linq;
using SwipeElements.Game.ECS.Providers;
using SwipeElements.Game.Extensions;
using SwipeElements.Game.Views;
using SwipeElements.Infrastructure.Services.LevelDataProvider.Extensions;
using UnityEditor;
using UnityEngine;

namespace SwipeElements.Game.Scripts.Infrastructure.Services.LevelDataProvider.Editor
{
    public sealed class LevelTool : EditorWindow
    {
        private string _savePath = "Assets/Game/Prefabs/Game/Levels/";
        private string _fileName = "Level_00.json";

        private LevelView _levelView;
        
        [MenuItem("Tools/Game/Level Tool")]
        public static void ShowWindow()
        {
            GetWindow<LevelTool>("Level Tool");
        }

        private void OnGUI()
        {
            EditorGUILayout.Space();
            _savePath = EditorGUILayout.TextField("Save Path", _savePath);
            _fileName = EditorGUILayout.TextField("File Name", _fileName);
            _levelView = (LevelView)EditorGUILayout.ObjectField("Level View", _levelView, typeof(LevelView), true);
            EditorGUILayout.Space();
            
            EditorGUILayout.BeginHorizontal();
            
            if (GUILayout.Button("Actualize", GUILayout.Width(120)))
            {
                Actualize();
            }
            
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

        private void Actualize()
        {
            if (_levelView == null)
            {
                Debug.LogError("Level View is null!");
                
                return;
            }
            
            List<ElementProvider> elements = _levelView
                .GetComponentsInChildren<ElementProvider>()
                .ToList();
            
            _levelView.Elements.Clear();
            
            int all = elements.Count;
            int actualized = 0;

            foreach (ElementProvider element in elements)
            {
                _levelView.Elements.Add(element);
            }
            
            Vector2Int size = _levelView.Grid.Size;
            
            float cellSize = _levelView.Grid.CellSize;
            
            _levelView.Grid.Init(size, cellSize);
            
            for (int x = 0; x < _levelView.Grid.Size.x; x++)
            {
                for (int y = 0; y < _levelView.Grid.Size.y; y++)
                {
                    Vector3 center = _levelView.Grid.GetCenter(x, y);

                    foreach (ElementProvider element in _levelView.Elements)
                    {
                        if (element.transform.position == center)
                        {
                            Undo.RecordObject(element, "Update Element Position");
                            
                            element.GetData().view.SetPosition(new (x, y));
                            
                            EditorUtility.SetDirty(element);
                            
                            actualized++;
                            
                            break;
                        }
                    }
                }
            }
            
            EditorUtility.SetDirty(_levelView);
            
            Debug.Log($"Actualized {actualized} of {all} elements");
        }
    }
}