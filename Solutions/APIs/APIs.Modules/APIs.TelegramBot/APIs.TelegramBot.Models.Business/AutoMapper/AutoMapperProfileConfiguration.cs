using APIs.TelegramBot.Models.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.TelegramBot;

namespace APIs.TelegramBot.Models.Business.AutoMapper
{
    public class AutoMapperProfileConfiguration : Profile
    {
        public AutoMapperProfileConfiguration()
        {
            #region TelegramBot

            CreateMap<TelegramGroupMessages, TelegramGroupMessagesVM>();
            CreateMap<TelegramGroupMessagesVM, TelegramGroupMessages>();

            #endregion
        }
    }
}
