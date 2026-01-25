using Newtonsoft.Json;
using SwipeElements.Game.ECS.Providers;
using SwipeElements.Game.Views;
using SwipeElements.Infrastructure.Services.LevelDataProvider.Data;
using SwipeElements.Infrastructure.Services.LevelDataProvider.Json;

namespace SwipeElements.Infrastructure.Services.LevelDataProvider.Extensions
{
    public static class SerializeDeserializeExtension
    {
        public static string Serialize(this LevelView level)
        {
            LevelData data = new ()
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

        public static LevelData Deserialize(this string text)
        {
            return JsonConvert.DeserializeObject<LevelData>(text, JsonSettings.Settings);
        }
    }
}