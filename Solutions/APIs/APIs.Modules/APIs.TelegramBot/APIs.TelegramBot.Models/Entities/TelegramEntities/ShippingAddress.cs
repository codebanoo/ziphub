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
    /// This object represents a shipping address.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class ShippingAddress
    {
        /// <summary>
        /// ISO 3166-1 alpha-2 country code
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string CountryCode { get; set; } = default!;

        /// <summary>
        /// State, if applicable
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string State { get; set; } = default!;

        /// <summary>
        /// City
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string City { get; set; } = default!;

        /// <summary>
        /// First line for the address
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string StreetLine1 { get; set; } = default!;

        /// <summary>
        /// Second line for the address
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string StreetLine2 { get; set; } = default!;

        /// <summary>
        /// Address post code
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string PostCode { get; set; } = default!;
    }
}
