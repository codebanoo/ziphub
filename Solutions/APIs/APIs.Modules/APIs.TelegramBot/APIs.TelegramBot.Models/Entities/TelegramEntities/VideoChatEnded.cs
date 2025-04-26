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
    /// This object represents a service message about a video chat ended in the chat.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class VideoChatEnded
    {
        /// <summary>
        /// Video chat duration in seconds
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public int Duration { get; set; }
    }
}
