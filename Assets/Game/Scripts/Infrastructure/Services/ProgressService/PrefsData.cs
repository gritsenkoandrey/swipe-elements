using System;

namespace SwipeElements.Infrastructure.Services.ProgressService
{
    public abstract class PrefsData<T> : IDisposable
    {
        public event Action<T> Changed = delegate { };

        private T _value;

        protected string Key { get; private set; }

        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                Changed.Invoke(_value);
                Save();
            }
        }

        public virtual void Load(string key, T defaultValue = default) => Key = key;

        protected abstract void Save();

        public void Dispose()
        {
            Changed = null;
        }

        public static implicit operator T(PrefsData<T> data) => data.Value;
    }
}