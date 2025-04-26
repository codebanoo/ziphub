using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIs.TelegramBot.Model.Entities
{
    internal partial class UpdateTypeConverter : JsonConverter<UpdateType>
    {
        public override void WriteJson(JsonWriter writer, UpdateType value, JsonSerializer serializer) =>
            writer.WriteValue(value switch
            {
                UpdateType.Unknown => "unknown",
                UpdateType.Message => "message",
                UpdateType.InlineQuery => "inline_query",
                UpdateType.ChosenInlineResult => "chosen_inline_result",
                UpdateType.CallbackQuery => "callback_query",
                UpdateType.EditedMessage => "edited_message",
                UpdateType.ChannelPost => "channel_post",
                UpdateType.EditedChannelPost => "edited_channel_post",
                UpdateType.ShippingQuery => "shipping_query",
                UpdateType.PreCheckoutQuery => "pre_checkout_query",
                UpdateType.Poll => "poll",
                UpdateType.PollAnswer => "poll_answer",
                UpdateType.MyChatMember => "my_chat_member",
                UpdateType.ChatMember => "chat_member",
                UpdateType.ChatJoinRequest => "chat_join_request",
                _ => throw new NotSupportedException(),
            });

        public override UpdateType ReadJson(
            JsonReader reader,
            Type objectType,
        UpdateType existingValue,
            bool hasExistingValue,
            JsonSerializer serializer
        ) =>
            JToken.ReadFrom(reader).Value<string>() switch
            {
                "unknown" => UpdateType.Unknown,
                "message" => UpdateType.Message,
                "inline_query" => UpdateType.InlineQuery,
                "chosen_inline_result" => UpdateType.ChosenInlineResult,
                "callback_query" => UpdateType.CallbackQuery,
                "edited_message" => UpdateType.EditedMessage,
                "channel_post" => UpdateType.ChannelPost,
                "edited_channel_post" => UpdateType.EditedChannelPost,
                "shipping_query" => UpdateType.ShippingQuery,
                "pre_checkout_query" => UpdateType.PreCheckoutQuery,
                "poll" => UpdateType.Poll,
                "poll_answer" => UpdateType.PollAnswer,
                "my_chat_member" => UpdateType.MyChatMember,
                "chat_member" => UpdateType.ChatMember,
                "chat_join_request" => UpdateType.ChatJoinRequest,
                _ => UpdateType.Unknown,
            };
    }
}
