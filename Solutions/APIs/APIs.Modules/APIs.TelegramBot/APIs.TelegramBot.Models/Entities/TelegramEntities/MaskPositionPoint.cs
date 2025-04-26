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
    /// The part of the face relative to which the mask should be placed.
    /// </summary>
    [JsonConverter(typeof(MaskPositionPointConverter))]
    public enum MaskPositionPoint
    {
        /// <summary>
        /// The forehead
        /// </summary>
        Forehead = 1,

        /// <summary>
        /// The eyes
        /// </summary>
        Eyes,

        /// <summary>
        /// The mouth
        /// </summary>
        Mouth,

        /// <summary>
        /// The chin
        /// </summary>
        Chin
    }
}
