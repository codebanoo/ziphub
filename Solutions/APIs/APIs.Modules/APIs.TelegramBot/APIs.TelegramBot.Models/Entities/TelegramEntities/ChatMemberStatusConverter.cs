using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIs.TelegramBot.Model.Entities
{
    internal partial class ChatMemberStatusConverter : JsonConverter<ChatMemberStatus>
    {
        public override void WriteJson(JsonWriter writer, ChatMemberStatus value, JsonSerializer serializer) =>
            writer.WriteValue(value switch
            {
                ChatMemberStatus.Creator => "creator",
                ChatMemberStatus.Administrator => "administrator",
                ChatMemberStatus.Member => "member",
                ChatMemberStatus.Left => "left",
                ChatMemberStatus.Kicked => "kicked",
                ChatMemberStatus.Restricted => "restricted",
                (ChatMemberStatus)0 => "unknown",
                _ => throw new NotSupportedException(),
            });

        public override ChatMemberStatus ReadJson(
            JsonReader reader,
            Type objectType,
        ChatMemberStatus existingValue,
            bool hasExistingValue,
            JsonSerializer serializer
        ) =>
            JToken.ReadFrom(reader).Value<string>() switch
            {
                "creator" => ChatMemberStatus.Creator,
                "administrator" => ChatMemberStatus.Administrator,
                "member" => ChatMemberStatus.Member,
                "left" => ChatMemberStatus.Left,
                "kicked" => ChatMemberStatus.Kicked,
                "restricted" => ChatMemberStatus.Restricted,
                _ => 0,
            };
    }
}
