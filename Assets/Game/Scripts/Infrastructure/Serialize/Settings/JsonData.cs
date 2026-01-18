using System.Collections.Generic;
using UnityEngine;

namespace SwipeElements.Infrastructure.Serialize.Settings
{
    [System.Serializable]
    public sealed class JsonData
    {
        public GridData grid;
        public List<ElementData> elements;
    }

    [System.Serializable]
    public sealed class GridData
    {
        public Vector2Int gridSize;
    }
    
    [System.Serializable]
    public sealed class ElementData
    {
        public string id;
        public Vector2Int position;
    }
}