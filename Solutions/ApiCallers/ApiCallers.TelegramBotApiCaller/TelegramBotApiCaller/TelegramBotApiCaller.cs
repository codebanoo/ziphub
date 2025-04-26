using ApiCallers.BaseApiCaller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.PVM.TelegramBot;

namespace ApiCallers.TelegramBotApiCaller
{
    public class TelegramBotApiCaller : BaseCaller, ITelegramBotApiCaller
    {
        public TelegramBotApiCaller() : base()
        {
        }

        public TelegramBotApiCaller(string _serviceUrl) : base(_serviceUrl)
        {
            serviceUrl = _serviceUrl;
        }

        public TelegramBotApiCaller(string _serviceUrl,
            string _accessToken) :
            base(_serviceUrl,
                _accessToken)
        {
            serviceUrl = _serviceUrl;
        }

        #region TelegramGroupMessagesManagement
        public ResponseApiCaller GetAllUnreadedTelegramGroupMessages(GetAllUnreadedTelegramGroupMessagesPVM getAllUnreadedTelegramGroupMessagesPVM)
        {
            return GetRecords(getAllUnreadedTelegramGroupMessagesPVM);
        }
        #endregion
    }
}
