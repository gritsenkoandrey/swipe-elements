using UnityEditor;
using UnityEngine;

namespace SwipeElements.Game.Views
{
    public sealed class GridView : MonoBehaviour
    {
        [field: SerializeField] public Vector2Int Size { get; private set; }
        [field: SerializeField] public Transform FloorAnchor { get; private set; }
        
        public float CellSize { get; set; } = 2f;

        public void SetGridSize(Vector2Int size)
        {
            Size = size;
        }

        public Vector3 GetOrigin()
        {
            float totalGridWidth = CellSize * Size.x;
            float startX = FloorAnchor.position.x - (totalGridWidth / 2f);
            float startY = FloorAnchor.position.y;
            
            return new (startX, startY, 0f);
        }
        
        public Vector3 GetCenter(Vector3 origin, int x, int y)
        {
            float posX = origin.x + x * CellSize + CellSize / 2f;
            float posY = origin.y + y * CellSize + CellSize / 2f;
            
            return new (posX, posY, 0f);
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            GUIStyle guiStyle = new ()
            {
                fontSize = 27,
                alignment = TextAnchor.MiddleCenter,
                normal =
                {
                    textColor = Color.green
                }
            };

            Gizmos.color = Color.yellow;
            
            Vector3 origin = GetOrigin();
        
            for (int x = 0; x < Size.x; x++)
            {
                for (int y = 0; y < Size.y; y++)
                {
                    Vector3 center = GetCenter(origin, x, y);
                    Gizmos.DrawWireCube(center, new (CellSize, CellSize, 0.1f));
                    Handles.Label(center,$"{x}:{y}", guiStyle);
                }
            }
        
            Gizmos.color = Color.red;
            
            float totalW = Size.x * CellSize;
            float totalH = Size.y * CellSize;
            Vector3 globalCenter = origin + new Vector3(totalW / 2f, totalH / 2f, 0f);
            
            Gizmos.DrawWireCube(globalCenter, new (totalW, totalH, 0f));
        }
#endif
    }
}