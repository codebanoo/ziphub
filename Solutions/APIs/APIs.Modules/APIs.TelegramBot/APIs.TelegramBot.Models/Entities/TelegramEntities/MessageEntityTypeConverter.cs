using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIs.TelegramBot.Model.Entities
{
    internal partial class MessageEntityTypeConverter : JsonConverter<MessageEntityType>
    {
        public override void WriteJson(JsonWriter writer, MessageEntityType value, JsonSerializer serializer) =>
            writer.WriteValue(value switch
            {
                MessageEntityType.Mention => "mention",
                MessageEntityType.Hashtag => "hashtag",
                MessageEntityType.BotCommand => "bot_command",
                MessageEntityType.Url => "url",
                MessageEntityType.Email => "email",
                MessageEntityType.Bold => "bold",
                MessageEntityType.Italic => "italic",
                MessageEntityType.Code => "code",
                MessageEntityType.Pre => "pre",
                MessageEntityType.TextLink => "text_link",
                MessageEntityType.TextMention => "text_mention",
                MessageEntityType.PhoneNumber => "phone_number",
                MessageEntityType.Cashtag => "cashtag",
                MessageEntityType.Underline => "underline",
                MessageEntityType.Strikethrough => "strikethrough",
                MessageEntityType.Spoiler => "spoiler",
                MessageEntityType.CustomEmoji => "custom_emoji",
                (MessageEntityType)0 => "unknown",
                _ => throw new NotSupportedException(),
            });

        public override MessageEntityType ReadJson(
            JsonReader reader,
            Type objectType,
        MessageEntityType existingValue,
            bool hasExistingValue,
            JsonSerializer serializer
        ) =>
            JToken.ReadFrom(reader).Value<string>() switch
            {
                "mention" => MessageEntityType.Mention,
                "hashtag" => MessageEntityType.Hashtag,
                "bot_command" => MessageEntityType.BotCommand,
                "url" => MessageEntityType.Url,
                "email" => MessageEntityType.Email,
                "bold" => MessageEntityType.Bold,
                "italic" => MessageEntityType.Italic,
                "code" => MessageEntityType.Code,
                "pre" => MessageEntityType.Pre,
                "text_link" => MessageEntityType.TextLink,
                "text_mention" => MessageEntityType.TextMention,
                "phone_number" => MessageEntityType.PhoneNumber,
                "cashtag" => MessageEntityType.Cashtag,
                "underline" => MessageEntityType.Underline,
                "strikethrough" => MessageEntityType.Strikethrough,
                "spoiler" => MessageEntityType.Spoiler,
                "custom_emoji" => MessageEntityType.CustomEmoji,
                _ => 0,
            };
    }
}
