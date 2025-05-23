﻿using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIs.TelegramBot.Model.Entities
{
    /// <summary>
    /// This object represents changes in the status of a chat member.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class ChatMemberUpdated
    {
        /// <summary>
        /// Chat the user belongs to
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public Chat Chat { get; set; } = default!;

        /// <summary>
        /// Performer of the action, which resulted in the change
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public User From { get; set; } = default!;

        /// <summary>
        /// Date the change was done
        /// </summary>
        [JsonConverter(typeof(UnixDateTimeConverter))]
        [JsonProperty(Required = Required.Always)]
        public DateTime Date { get; set; }

        /// <summary>
        /// Previous information about the chat member
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public ChatMember OldChatMember { get; set; } = default!;

        /// <summary>
        /// New information about the chat member
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public ChatMember NewChatMember { get; set; } = default!;

        /// <summary>
        /// Optional. Chat invite link, which was used by the user to join the chat; for joining by invite link
        /// events only.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ChatInviteLink? InviteLink { get; set; }

        /// <summary>
        /// Optional. <see langword="true"/>, if the user joined the chat via a chat folder invite link
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? ViaChatFolderInviteLink { get; set; }
    }
}
