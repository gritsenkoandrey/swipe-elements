using System.Runtime.CompilerServices;
using SwipeElements.Game.Extensions;
using UnityEditor;
using UnityEngine;

namespace SwipeElements.Game.Views
{
    public sealed class GridView : MonoBehaviour
    {
        [field: SerializeField] public Vector2Int Size { get; private set; }
        [field: SerializeField] public Transform FloorAnchor { get; private set; }
        public float CellSize { get; private set; } = 2f;
        public Vector2 Origin { get; private set; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Init(Vector2Int size, float cellSize)
        {
            Size = size;
            CellSize = cellSize;
            Origin = this.GetOrigin();
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            GUIStyle guiStyle = new ()
            {
                fontSize = 27,
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = Color.green }
            };

            Gizmos.color = Color.yellow;
            for (int x = 0; x < Size.x; x++)
            {
                for (int y = 0; y < Size.y; y++)
                {
                    Vector3 center = this.GetCenter(x, y);
                    Gizmos.DrawWireCube(center, new (CellSize, CellSize, 0.1f));
                    Handles.Label(center,$"{x}:{y}", guiStyle);
                }
            }
        
            Gizmos.color = Color.red;
            float totalW = Size.x * CellSize;
            float totalH = Size.y * CellSize;
            Vector3 globalCenter = this.GetOrigin() + new Vector2(totalW / 2f, totalH / 2f);
            Gizmos.DrawWireCube(globalCenter, new (totalW, totalH, 0f));
        }
#endif
    }
}