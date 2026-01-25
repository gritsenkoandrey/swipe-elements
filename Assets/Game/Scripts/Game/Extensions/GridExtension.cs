using SwipeElements.Game.Views;
using UnityEngine;

namespace SwipeElements.Game.Extensions
{
    public static class GridExtension
    {
        public static Vector3 GetCenter(this GridView grid, int x, int y)
        {
            float posX = grid.Origin.x + x * grid.CellSize + grid.CellSize / 2f;
            float posY = grid.Origin.y + y * grid.CellSize + grid.CellSize / 2f;
            
            return new (posX, posY, 0f);
        }

        public static Vector2 GetOrigin(this GridView grid)
        {
            float totalGridWidth = grid.CellSize * grid.Size.x;
            float startX = grid.FloorAnchor.position.x - totalGridWidth / 2f;
            float startY = grid.FloorAnchor.position.y;
            
            return new (startX, startY);
        }
    }
}