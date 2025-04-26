using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIs.TelegramBot.Model.Entities
{
    internal partial class ChatTypeConverter : JsonConverter<ChatType>
    {
        public override void WriteJson(JsonWriter writer, ChatType value, JsonSerializer serializer) =>
            writer.WriteValue(value switch
            {
                ChatType.Private => "private",
                ChatType.Group => "group",
                ChatType.Channel => "channel",
                ChatType.Supergroup => "supergroup",
                ChatType.Sender => "sender",
                (ChatType)0 => "unknown",
                _ => throw new NotSupportedException(),
            });

        public override ChatType ReadJson(
            JsonReader reader,
            Type objectType,
        ChatType existingValue,
            bool hasExistingValue,
            JsonSerializer serializer
        ) =>
            JToken.ReadFrom(reader).Value<string>() switch
            {
                "private" => ChatType.Private,
                "group" => ChatType.Group,
                "channel" => ChatType.Channel,
                "supergroup" => ChatType.Supergroup,
                "sender" => ChatType.Sender,
                _ => 0,
            };
    }
}
