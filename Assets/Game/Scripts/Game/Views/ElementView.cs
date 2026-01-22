using System.Runtime.CompilerServices;
using UnityEngine;

namespace SwipeElements.Game.Views
{
    public sealed class ElementView : MonoBehaviour
    {
        [field: SerializeField] public string Id { get; private set; }
        [field: SerializeField] public Vector2Int Position { get; private set; }
        [field: SerializeField] public ElementAnimator Animator { get; private set; }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetPosition(Vector2Int position)
        {
            Position = position;
            
            Animator.Renderer.sortingOrder = position.y + position.x;
        }
    }
}