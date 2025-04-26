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
    /// This object contains information about one answer option in a poll.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class PollOption
    {
        /// <summary>
        /// Option text, 1-100 characters
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Text { get; set; } = default!;

        /// <summary>
        /// Number of users that voted for this option
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public int VoterCount { get; set; }
    }
}
