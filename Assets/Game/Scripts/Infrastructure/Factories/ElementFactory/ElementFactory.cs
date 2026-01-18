using System.Collections.Generic;
using JetBrains.Annotations;
using SwipeElements.Game.ECS.Providers;
using SwipeElements.Infrastructure.Services.StaticDataService;
using SwipeElements.Infrastructure.Services.StaticDataService.StaticData;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace SwipeElements.Infrastructure.Factories.ElementFactory
{
    [UsedImplicitly]
    public sealed class ElementFactory : IElementFactory
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IObjectResolver _objectResolver;

        private Dictionary<string, ElementProvider> _elements;
        
        public ElementFactory(IStaticDataService staticDataService, IObjectResolver objectResolver)
        {
            _staticDataService = staticDataService;
            _objectResolver = objectResolver;
        }

        void IElementFactory.Init()
        {
            ElementConfig config = _staticDataService.GetElementConfig();
            
            _elements = new (config.Elements.Length);

            foreach (ElementProvider element in config.Elements)
            {
                _elements.Add(element.GetData().view.Id, element);
            }
        }

        bool IElementFactory.TryCreateElement(string id, Transform root, out ElementProvider elementProvider)
        {
            if (_elements.TryGetValue(id, out ElementProvider value))
            {
                ElementProvider element = _objectResolver.Instantiate(value, root);
                
                elementProvider = element;
                return true;
            }
            
            elementProvider = null;
            return false;
        }
    }
}