using System.Collections.Generic;

namespace SwipeElements.Infrastructure.Services.LevelDataProvider.Data
{
    [System.Serializable]
    public sealed class LevelData
    {
        public GridData grid;
        public List<ElementData> elements;
    }
}