using System.Collections.Generic;
using SwipeElements.Game.ECS.Providers;
using UnityEngine;

namespace SwipeElements.Game.Views
{
    public sealed class LevelView : MonoBehaviour
    {
        [field: SerializeField] public Transform ElementsRoot { get; private set; }
        [field: SerializeField] public SpriteRenderer Background { get; private set; }
        [field: SerializeField] public GridView Grid { get; private set; }
        [field: SerializeField] public List<ElementProvider> Elements { get; private set; }
    }
}