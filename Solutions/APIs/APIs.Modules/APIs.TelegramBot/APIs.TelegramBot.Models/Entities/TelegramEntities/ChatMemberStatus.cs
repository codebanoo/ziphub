﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIs.TelegramBot.Model.Entities
{
    /// <summary>
    /// ChatMember status
    /// </summary>
    [JsonConverter(typeof(ChatMemberStatusConverter))]
    public enum ChatMemberStatus
    {
        /// <summary>
        /// Creator of the <see cref="Chat"/>
        /// </summary>
        Creator = 1,

        /// <summary>
        /// Administrator of the <see cref="Chat"/>
        /// </summary>
        Administrator,

        /// <summary>
        /// Normal member of the <see cref="Chat"/>
        /// </summary>
        Member,

        /// <summary>
        /// A <see cref="User"/> who left the <see cref="Chat"/>
        /// </summary>
        Left,

        /// <summary>
        /// A <see cref="User"/> who was kicked from the <see cref="Chat"/>
        /// </summary>
        Kicked,

        /// <summary>
        /// A <see cref="User"/> who is restricted in the <see cref="Chat"/>
        /// </summary>
        Restricted
    }
}
