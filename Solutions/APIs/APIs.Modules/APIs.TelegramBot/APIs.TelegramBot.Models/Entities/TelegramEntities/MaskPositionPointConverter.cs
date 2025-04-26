using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIs.TelegramBot.Model.Entities
{
    internal partial class MaskPositionPointConverter : JsonConverter<MaskPositionPoint>
    {
        public override void WriteJson(JsonWriter writer, MaskPositionPoint value, JsonSerializer serializer) =>
            writer.WriteValue(value switch
            {
                MaskPositionPoint.Forehead => "forehead",
                MaskPositionPoint.Eyes => "eyes",
                MaskPositionPoint.Mouth => "mouth",
                MaskPositionPoint.Chin => "chin",
                (MaskPositionPoint)0 => "unknown",
                _ => throw new NotSupportedException(),
            });

        public override MaskPositionPoint ReadJson(
            JsonReader reader,
            Type objectType,
        MaskPositionPoint existingValue,
            bool hasExistingValue,
            JsonSerializer serializer
        ) =>
            JToken.ReadFrom(reader).Value<string>() switch
            {
                "forehead" => MaskPositionPoint.Forehead,
                "eyes" => MaskPositionPoint.Eyes,
                "mouth" => MaskPositionPoint.Mouth,
                "chin" => MaskPositionPoint.Chin,
                _ => 0,
            };
    }
}
