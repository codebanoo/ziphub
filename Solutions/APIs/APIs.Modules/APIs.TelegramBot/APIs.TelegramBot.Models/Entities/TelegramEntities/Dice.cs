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
    /// This object represents a dice with random value
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class Dice
    {
        /// <summary>
        /// Emoji on which the dice throw animation is based
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Emoji { get; set; } = default!;
        /// <summary>
        /// Value of the dice, 1-6 for <see cref="Telegram.Bot.Types.Enums.Emoji.Dice" /> (“🎲”),
        /// <see cref="Darts" /> (“🎯”) and <see cref="Bowling"/> ("🎳"), 1-5 for <see cref="Basketball" /> (“🏀”) and
        /// <see cref="Football" />("⚽"), and values 1-64 for <see cref="SlotMachine" /> ("🎰"). Defaults to
        /// <see cref="Telegram.Bot.Types.Enums.Emoji.Dice" /> (“🎲”)
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public int Value { get; set; }
    }
}
