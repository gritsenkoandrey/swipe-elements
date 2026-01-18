using UnityEngine;
using UnityEngine.UI;

namespace SwipeElements.Utils
{
    [RequireComponent(typeof(CanvasRenderer))]
    [AddComponentMenu("UI/Raycast Blocker")]
    public sealed class RaycastBlocker : Graphic
    {
        protected override void OnPopulateMesh(VertexHelper vertexHelper)
        {
            vertexHelper.Clear();
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = new (1, 0, 0, 0.3f);
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawCube(Vector3.zero, rectTransform.rect.size);
            
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(Vector3.zero, rectTransform.rect.size);
        }
#endif
    }
}