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
    /// Contains information about Telegram Passport data shared with the bot by the user.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class PassportData
    {
        /// <summary>
        /// Array with information about documents and other Telegram Passport elements that was shared with the bot.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public EncryptedPassportElement[] Data { get; set; } = default!;

        /// <summary>
        /// Encrypted credentials required to decrypt the data.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public EncryptedCredentials Credentials { get; set; } = default!;
    }
}
