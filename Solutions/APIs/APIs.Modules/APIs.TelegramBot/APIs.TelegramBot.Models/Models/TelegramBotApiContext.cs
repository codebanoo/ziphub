using APIs.TelegramBot.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace APIs.TelegramBot.Models
{
    public partial class TelegramBotApiContext : DbContext
    {
        public TelegramBotApiContext()
        {
        }

        public TelegramBotApiContext(DbContextOptions<TelegramBotApiContext> options)
            : base(options)
        {
            Database.SetCommandTimeout(int.MaxValue);
        }

        #region TelegramBot

        public virtual DbSet<TelegramGroupMessages> TelegramGroupMessages { get; set; }

        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                        .AddJsonFile("appsettings.json")
                        .Build();

            optionsBuilder.UseSqlServer(configuration.GetConnectionString("TelegramBotConnection"));
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("TelegramBotConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region TelegramBot

            modelBuilder.Entity<TelegramGroupMessages>(entity =>
            {
                entity.HasKey(e => e.TelegramGroupMessageId);
            });

            #endregion
        }
    }
}
