using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf.DownloadZipFiles.VM
{
    public class AppConfigs
    {
        public string Ip { get; set; }

        public string Port { get; set; }

        public string DbNames { get; set; }

        public string DownloadRootPath { get; set; }

        public string SourceNames { get; set; }

        public string UsersFiles { get; set; }

        public string DestinationPath { get; set; }

        public string DownloadZipStartTime { get; set; }

        public bool Auto { get; set; }
    }
}
