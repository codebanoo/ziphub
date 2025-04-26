using APIs.TelegramBot.Models.Entities;
using AutoMapper;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.TelegramBot;

namespace APIs.TelegramBot.Models.Business
{
    public class TelegramBotApiBusiness : ITelegramBotApiBusiness, IDisposable
    {
        private TelegramBotApiContext telegramBotApiDb = new TelegramBotApiContext();

        private IMapper _mapper;

        private IHostEnvironment hostingEnvironment;

        public TelegramBotApiContext TelegramBotApiDb
        {
            get { return this.telegramBotApiDb; }
            set { }
            //private set { }
        }

        public void Dispose()
        {
            telegramBotApiDb.Dispose();
        }

        public TelegramBotApiBusiness(IMapper mapper,
            TelegramBotApiContext _telegramBotApiDb,
            IHostEnvironment _hostingEnvironment)
        {
            try
            {
                _mapper = mapper;

                telegramBotApiDb = _telegramBotApiDb;

                TelegramBotApiDb = _telegramBotApiDb;

                hostingEnvironment = _hostingEnvironment;
            }
            catch (Exception exc)
            {
            }
        }

        #region TelegramBot

        #region Methods for Work With TelegramGroupMessages

        public List<TelegramGroupMessagesVM> GetAllTelegramGroupMessagesList(
           ref int listCount,
           List<long> childsUsersIds)
        {
            List<TelegramGroupMessagesVM> telegramGroupMessagesVMList = new List<TelegramGroupMessagesVM>();

            try
            {
                var list = telegramBotApiDb.TelegramGroupMessages.AsQueryable();

                if (childsUsersIds != null)
                {
                    if (childsUsersIds.Count > 1)
                    {
                        list = list.Where(m => m.UserIdCreator.HasValue).Where(m => childsUsersIds.Contains(m.UserIdCreator.Value));
                    }
                    else
                    {
                        if (childsUsersIds.Count == 1)
                        {
                            if (childsUsersIds.FirstOrDefault() > 0)
                            {
                                list = list.Where(m => m.UserIdCreator.HasValue).Where(m => childsUsersIds.Contains(m.UserIdCreator.Value));
                            }
                        }
                    }
                }

                telegramGroupMessagesVMList = _mapper.Map<List<TelegramGroupMessages>, List<TelegramGroupMessagesVM>>(list.ToList());
            }
            catch (Exception exc)
            { }

            return telegramGroupMessagesVMList;
        }

        public List<TelegramGroupMessagesVM> GetAllUnreadedTelegramGroupMessages()
        {
            List<TelegramGroupMessagesVM> telegramGroupMessagesVMList = new List<TelegramGroupMessagesVM>();

            try
            {
                var list = telegramBotApiDb.TelegramGroupMessages.AsQueryable();

                list = list.Where(l => l.Transferred.Equals(false));

                List<TelegramGroupMessages> telegramGroupMessagesList = new List<TelegramGroupMessages>();
                telegramGroupMessagesList = list.ToList();

                telegramGroupMessagesVMList = _mapper.Map<List<TelegramGroupMessages>, List<TelegramGroupMessagesVM>>(list.ToList());

                foreach (var telegramGroupMessages in telegramGroupMessagesList)
                {
                    telegramGroupMessages.Transferred = true;
                }
                telegramBotApiDb.SaveChanges();
            }
            catch (Exception exc)
            { }

            return telegramGroupMessagesVMList;
        }

        public string GetFilePath(string fileName)
        {
            string filePath = "";

            try
            {
                //if (fileName.Equals("211bdc2f-7378-4a51-a21c-711f3a487b4a.jpg"))
                //{
                //    int i = 0;
                //}
                if (!string.IsNullOrEmpty(fileName))
                {
                    if (telegramBotApiDb.TelegramGroupMessages.Where(t => t.FileName.Equals(fileName)).Any())
                    {
                        var telegramGroupMessages = telegramBotApiDb.TelegramGroupMessages.Where(t => t.FileName.Equals(fileName)).FirstOrDefault();

                        switch (telegramGroupMessages.MessageType)
                        {
                            case "Photo":
                                filePath = "c:\\sites\\APIs.TelegramBot\\wwwroot\\Files\\TelegramGroupFiles\\" + telegramGroupMessages.SubFolderName + "\\images\\" + telegramGroupMessages.FileName;
                                break;
                            case "Video":
                                filePath = "c:\\sites\\APIs.TelegramBot\\wwwroot\\Files\\TelegramGroupFiles\\" + telegramGroupMessages.SubFolderName + "\\videos\\" + telegramGroupMessages.FileName;
                                break;
                            case "Voice":
                                filePath = "c:\\sites\\APIs.TelegramBot\\wwwroot\\Files\\TelegramGroupFiles\\" + telegramGroupMessages.SubFolderName + "\\voices\\" + telegramGroupMessages.FileName;
                                break;
                            case "Audio":
                                filePath = "c:\\sites\\APIs.TelegramBot\\wwwroot\\Files\\TelegramGroupFiles\\" + telegramGroupMessages.SubFolderName + "\\audios\\" + telegramGroupMessages.FileName;
                                break;
                            case "Document":
                                filePath = "c:\\sites\\APIs.TelegramBot\\wwwroot\\Files\\TelegramGroupFiles\\" + telegramGroupMessages.SubFolderName + "\\docs\\" + telegramGroupMessages.FileName;
                                break;
                        }
                    }
                }
            }
            catch (Exception exc)
            { }

            return filePath;
        }

        #endregion

        #endregion
    }
}
