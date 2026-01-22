using Newtonsoft.Json;
using SwipeElements.Game.ECS.Providers;
using SwipeElements.Game.Views;
using SwipeElements.Infrastructure.Serialize.Settings;

namespace SwipeElements.Infrastructure.Serialize
{
    public static class LevelSerializer
    {
        public static string Serialize(this LevelView level)
        {
            JsonData data = new ()
            {
                grid = new ()
                {
                    gridSize = level.Grid.Size,
                },
                
                elements = new (level.Elements.Count)
            };

            foreach (ElementProvider element in level.Elements)
            {
                if (element == null)
                {
                    continue;
                }
                
                ElementView view = element.GetData().view;
                
                ElementData elementData = new ()
                {
                    position = view.Position,
                    id = view.Id
                };
                
                data.elements.Add(elementData);
            }
            
            return JsonConvert.SerializeObject(data, JsonSettings.Settings);;
        }

        public static JsonData Deserialize(string text)
        {
            return JsonConvert.DeserializeObject<JsonData>(text, JsonSettings.Settings);
        }
    }
}