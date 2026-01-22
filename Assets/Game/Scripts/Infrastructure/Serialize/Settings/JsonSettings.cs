using Newtonsoft.Json;

namespace SwipeElements.Infrastructure.Serialize.Settings
{
    public static class JsonSettings
    {
        public static readonly JsonSerializerSettings Settings = new()
        {
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore,
            Formatting = Formatting.None,
            Converters =
            {
                new Vector2IntConverter()
            }
        };
    }
}