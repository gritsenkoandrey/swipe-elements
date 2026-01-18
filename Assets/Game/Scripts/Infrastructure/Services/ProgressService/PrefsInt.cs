using UnityEngine;

namespace SwipeElements.Infrastructure.Services.ProgressService
{
    public sealed class PrefsInt : PrefsData<int>
    {
        public override void Load(string key, int defaultValue = 0)
        {
            base.Load(key, defaultValue);

            Value = PlayerPrefs.GetInt(key, defaultValue);
        }

        protected override void Save()
        {
            PlayerPrefs.SetInt(Key, Value);
        }
    }
}