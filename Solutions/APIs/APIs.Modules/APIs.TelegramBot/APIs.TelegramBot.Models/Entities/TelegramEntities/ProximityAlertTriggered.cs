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
    /// Represents the content of a service message, sent whenever a user in the chat triggers a proximity alert set
    /// by another user.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class ProximityAlertTriggered
    {
        /// <summary>
        /// User that triggered the alert
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public User Traveler { get; set; } = default!;

        /// <summary>
        /// User that set the alert
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public User Watcher { get; set; } = default!;

        /// <summary>
        /// The distance between the users
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public int Distance { get; set; }
    }
}
