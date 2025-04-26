using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.TelegramBot;

namespace APIs.TelegramBot.Models.Business
{
    public interface ITelegramBotApiBusiness
    {
        TelegramBotApiContext TelegramBotApiDb { get; set; }

        #region TelegramBot

        #region Methods for Work With TelegramGroupMessages

        List<TelegramGroupMessagesVM> GetAllTelegramGroupMessagesList(
           ref int listCount,
           List<long> childsUsersIds);

        List<TelegramGroupMessagesVM> GetAllUnreadedTelegramGroupMessages();

        string GetFilePath(string fileName);

        #endregion

        #endregion
    }
}
