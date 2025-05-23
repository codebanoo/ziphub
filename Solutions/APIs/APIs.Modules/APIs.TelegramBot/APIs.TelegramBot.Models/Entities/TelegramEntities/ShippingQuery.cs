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
    /// This object contains information about an incoming shipping query.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class ShippingQuery
    {
        /// <summary>
        /// Unique query identifier
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Id { get; set; } = default!;

        /// <summary>
        /// User who sent the query
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public User From { get; set; } = default!;

        /// <summary>
        /// Bot specified invoice payload
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string InvoicePayload { get; set; } = default!;

        /// <summary>
        /// User specified shipping address
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public ShippingAddress ShippingAddress { get; set; } = default!;
    }
}
