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
    /// Contains information about a <a href="https://core.telegram.org/bots/webapps ">Web App</a>
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class WebAppInfo
    {
        /// <summary>
        /// An HTTPS URL of a Web App to be opened with additional data as specified in
        /// <a href="https://core.telegram.org/bots/webapps#initializing-web-apps">Initializing Web Apps</a>
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Url { get; set; } = default!;
    }
}
