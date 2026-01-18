using SwipeElements.UI.Screens;

namespace SwipeElements.UI
{
    public interface IScreenService
    {
        bool TryShow<TScreen>(out TScreen screen) where TScreen : BaseScreen;
        void TryHide<TScreen>() where TScreen : BaseScreen;
        bool TryGet<TScreen>(out TScreen screen) where TScreen : BaseScreen;
    }
}