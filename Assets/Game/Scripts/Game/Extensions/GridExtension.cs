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
    }
}