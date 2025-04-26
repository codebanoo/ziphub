using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf.CreateZipFromFiles.VM
{
    public class AppConfigs
    {
        public string DbNames { get; set; }

        public string DbBackUpPath { get; set; }

        public string DestinationPath { get; set; }

        public string MainUsersFilesPath { get; set; }

        public string DestinationUsersFilesPath { get; set; }

        public string MainSourcePath { get; set; }

        public string DestinationSourcePath { get; set; }

        public string CreateZipStartTime { get; set; }

        public string RemoveOldFilesStartTime { get; set; }
    }
}
