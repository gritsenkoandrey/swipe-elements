using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using SwipeElements.Game.ECS.Providers;
using SwipeElements.Game.Extensions;
using UnityEngine;

namespace SwipeElements.Game.Views
{
    public sealed class LevelView : MonoBehaviour
    {
        [field: SerializeField] public Transform ElementsRoot { get; private set; }
        [field: SerializeField] public SpriteRenderer Background { get; private set; }
        [field: SerializeField] public GridView Grid { get; private set; }
        [field: SerializeField] public List<ElementProvider> Elements { get; private set; }
        
#if UNITY_EDITOR
        [Button]
        private void ActualizeElementsPosition()
        {
            for (int x = 0; x < Grid.Size.x; x++)
            {
                for (int y = 0; y < Grid.Size.y; y++)
                {
                    Vector3 center = Grid.GetCenter(x, y);

                    foreach (ElementProvider element in Elements)
                    {
                        if (element.transform.position == center)
                        {
                            element.GetData().view.SetPosition(new (x, y));
                            
                            break;
                        }
                    }
                }
            }
        }

        [Button]
        private void FindElements()
        {
            Elements = GetComponentsInChildren<ElementProvider>().ToList();
        }

        [Button]
        private void FindGrid()
        {
            Grid = GetComponentInChildren<GridView>();
        }
  #endif
    }
}