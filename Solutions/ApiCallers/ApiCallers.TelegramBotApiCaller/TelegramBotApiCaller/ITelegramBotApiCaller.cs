using ApiCallers.BaseApiCaller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.PVM.TelegramBot;

namespace ApiCallers.TelegramBotApiCaller
{
    public interface ITelegramBotApiCaller
    {
        #region TelegramGroupMessagesManagement
        ResponseApiCaller GetAllUnreadedTelegramGroupMessages(GetAllUnreadedTelegramGroupMessagesPVM getAllUnreadedTelegramGroupMessagesPVM);
        #endregion
    }
}
