using System;
using System.Collections.Generic;
using SwipeElements.Infrastructure.Services.StaticDataService;
using SwipeElements.Infrastructure.Services.StaticDataService.StaticData;
using SwipeElements.UI.Screens;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace SwipeElements.UI
{
    public sealed class ScreenService : MonoBehaviour, IScreenService
    {
        [SerializeField] private Transform _root;
        
        private IStaticDataService _staticDataService;
        private IObjectResolver _objectResolver;
        private readonly Dictionary<Type, BaseScreen> _screens = new ();
        
        [Inject]
        private void Construct(IObjectResolver objectResolver, IStaticDataService staticDataService)
        {
            _objectResolver = objectResolver;
            _staticDataService = staticDataService;
        }

        bool IScreenService.TryShow<TScreen>(out TScreen screen)
        {
            Type type = typeof(TScreen);
            
            if (_screens.TryGetValue(type, out BaseScreen value))
            {
                value.Show();
                
                screen = value as TScreen;
                
                return true;
            }
            
            ScreenConfig config = _staticDataService.GetScreenConfig();

            foreach (BaseScreen baseScreen in config.Screens)
            {
                if (baseScreen.GetType() == type)
                {
                    BaseScreen instance = _objectResolver.Instantiate(baseScreen, _root);

                    instance.Show();
                        
                    _screens.Add(type, instance);
                        
                    screen = instance as TScreen;
                    return true;
                }
            }
                
            Debug.LogError($"Screen not found: {type.Name}");
            
            screen = null;
            return false;
        }
        
        void IScreenService.TryHide<TScreen>()
        {
            Type type = typeof(TScreen);
            
            if (_screens.TryGetValue(type, out BaseScreen value))
            {
                value.Hide();
            }
            else
            {
                Debug.LogError($"Screen not found: {type.Name}");
            }
        }

        bool IScreenService.TryGet<TScreen>(out TScreen screen)
        {
            Type type = typeof(TScreen);
            
            if (_screens.TryGetValue(type, out BaseScreen value))
            {
                screen = value as TScreen;
                return true;
            }

            screen = null;
            return false;
        }
    }
}