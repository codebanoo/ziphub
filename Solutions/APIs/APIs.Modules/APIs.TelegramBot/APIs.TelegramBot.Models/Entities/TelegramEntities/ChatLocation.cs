﻿using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIs.TelegramBot.Model.Entities
{
    /// <summary>
    /// Represents a location to which a chat is connected.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class ChatLocation
    {
        /// <summary>
        /// The location to which the supergroup is connected. Can't be a live location.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public Location Location { get; set; } = default!;

        /// <summary>
        /// Location address; 1-64 characters, as defined by the chat owner
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Address { get; set; } = default!;
    }
}
