using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIs.TelegramBot.Model.Entities
{
    internal partial class StickerTypeConverter : JsonConverter<StickerType>
    {
        public override void WriteJson(JsonWriter writer, StickerType value, JsonSerializer serializer) =>
            writer.WriteValue(value switch
            {
                StickerType.Regular => "regular",
                StickerType.Mask => "mask",
                StickerType.CustomEmoji => "custom_emoji",
                (StickerType)0 => "unknown",
                _ => throw new NotSupportedException(),
            });

        public override StickerType ReadJson(
            JsonReader reader,
            Type objectType,
        StickerType existingValue,
            bool hasExistingValue,
            JsonSerializer serializer
        ) =>
            JToken.ReadFrom(reader).Value<string>() switch
            {
                "regular" => StickerType.Regular,
                "mask" => StickerType.Mask,
                "custom_emoji" => StickerType.CustomEmoji,
                _ => 0,
            };
    }
}
