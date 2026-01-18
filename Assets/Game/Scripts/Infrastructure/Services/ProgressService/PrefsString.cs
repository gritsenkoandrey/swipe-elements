using UnityEngine;

namespace SwipeElements.Infrastructure.Services.ProgressService
{
    public sealed class PrefsString : PrefsData<string>
    {
        public override void Load(string key, string defaultValue = null)
        {
            base.Load(key, defaultValue);

            Value = PlayerPrefs.GetString(key, defaultValue);
        }

        protected override void Save()
        {
            PlayerPrefs.SetString(Key, Value);
        }
    }
}