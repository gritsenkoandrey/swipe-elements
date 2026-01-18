using SwipeElements.Game.ECS.Providers;
using UnityEngine;

namespace SwipeElements.Infrastructure.Services.StaticDataService.StaticData
{
    [CreateAssetMenu(fileName = nameof(ElementConfig), menuName = "Configs/" + nameof(ElementConfig))]
    public sealed class ElementConfig : ScriptableObject
    {
        [field: SerializeField] public ElementProvider[] Elements { get; private set; }
        [field: SerializeField] public float SwipeSpeed { get; private set;}
        [field: SerializeField] public float GravitySpeed { get; private set;}
        [field: SerializeField] public float DestroyAnimationTime { get; private set;}
        [field: SerializeField] public float IdleAnimationTime { get; private set;}
    }
}