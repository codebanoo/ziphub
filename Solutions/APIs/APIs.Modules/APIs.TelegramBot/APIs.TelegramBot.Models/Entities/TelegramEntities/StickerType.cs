using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIs.TelegramBot.Model.Entities
{
    /// <summary>
    /// Type of the <see cref="Sticker"/>
    /// </summary>
    [JsonConverter(typeof(StickerTypeConverter))]
    public enum StickerType
    {
        /// <summary>
        /// Regular  <see cref="Sticker"/>
        /// </summary>
        Regular = 1,

        /// <summary>
        /// Mask
        /// </summary>
        Mask,

        /// <summary>
        /// Custom emoji
        /// </summary>
        CustomEmoji
    }
}
