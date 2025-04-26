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
    /// This object represents a service message about a change in auto-delete timer settings.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class MessageAutoDeleteTimerChanged
    {
        /// <summary>
        /// New auto-delete time for messages in the chat
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public int MessageAutoDeleteTime { get; set; }
    }
}
