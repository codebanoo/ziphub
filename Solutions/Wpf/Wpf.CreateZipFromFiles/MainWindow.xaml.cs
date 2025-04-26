using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wpf.CreateZipFromFiles.VM;
using FrameWork;
using System.Text.RegularExpressions;
using System.IO;
using Path = System.IO.Path;
using System.Threading;
using System.Timers;

namespace Wpf.CreateZipFromFiles
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        AppConfigs appConfigs = new AppConfigs();
        List<string> dbNames = new List<string>();
        List<string> selectedDbNames = new List<string>();
        List<string> mainUsersFilesPath = new List<string>();
        List<string> mainSourcePath = new List<string>();

        DateTime todayEn = DateTime.Now.Date;

        string strToday = "";
        string centerDbBakUpFileName = "";

        //private System.Threading.Timer timerDbBakUp;
        //private System.Threading.Timer timerBackUpUsersFiles;
        //private System.Threading.Timer timerBackUpSourceControl;

        private bool chkUsersFile = false;
        private bool chkSourceFile = false;

        private Thread threadDbBakUp;
        private Thread threadBackUpUsersFiles;
        private Thread threadBackUpSourceControl;

        private bool threadDbBakUpRun;
        private bool threadBackUpUsersFilesRun;
        private bool threadBackUpSourceControlRun;

        public MainWindow()
        {
            InitializeComponent();

            strToday = todayEn.ToString();
            centerDbBakUpFileName = "_backup_" + todayEn.Year.ToString() + "_" +
                ((todayEn.Month > 9) ? todayEn.Month.ToString() : ("0" + todayEn.Month)) + "_" +
                ((todayEn.Day > 9) ? todayEn.Day.ToString() : ("0" + todayEn.Day.ToString()));

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            appConfigs.DbNames = config.AppSettings.Settings["DbNames"].Value;
            appConfigs.DbBackUpPath = config.AppSettings.Settings["DbBackUpPath"].Value;
            appConfigs.DestinationPath = config.AppSettings.Settings["DestinationPath"].Value;
            appConfigs.MainUsersFilesPath = config.AppSettings.Settings["MainUsersFilesPath"].Value;
            appConfigs.DestinationUsersFilesPath = config.AppSettings.Settings["DestinationUsersFilesPath"].Value;
            appConfigs.MainSourcePath = config.AppSettings.Settings["MainSourcePath"].Value;
            appConfigs.DestinationSourcePath = config.AppSettings.Settings["DestinationSourcePath"].Value;
            appConfigs.CreateZipStartTime = config.AppSettings.Settings["CreateZipStartTime"].Value;
            appConfigs.RemoveOldFilesStartTime = config.AppSettings.Settings["RemoveOldFilesStartTime"].Value;

            if (!string.IsNullOrEmpty(appConfigs.DbNames))
            {
                dbNames = appConfigs.DbNames.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();

                if (dbNames != null)
                    if (dbNames.Count > 0)
                    {
                        foreach (var dbName in dbNames)
                        {
                            lstBoxDbNames.Items.Add(dbName);
                        }
                    }
            }

            if (!string.IsNullOrEmpty(appConfigs.MainUsersFilesPath))
                mainUsersFilesPath = appConfigs.MainUsersFilesPath.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();

            if (!string.IsNullOrEmpty(appConfigs.MainSourcePath))
                mainSourcePath = appConfigs.MainSourcePath.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();

            txtDate.Text = PersianDate.DateNow;
            txtTime.Text = PersianDate.TimeNow;

            threadDbBakUpRun = true;
            threadBackUpUsersFilesRun = true;
            threadBackUpSourceControlRun = true;
        }

        private void cmbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBoxItem = e.AddedItems[0] as ComboBoxItem;
            if (comboBoxItem.Content != null)
            {
                if (comboBoxItem.Content.Equals("همیشه"))
                {
                    stkDate.Visibility = Visibility.Visible;
                    btnStop.Visibility = Visibility.Visible;
                }
                else
                {
                    stkDate.Visibility = Visibility.Hidden;
                    btnStop.Visibility = Visibility.Hidden;
                }
            }
        }

        private void txtDate_LostFocus(object sender, RoutedEventArgs e)
        {
            string text = txtDate.Text.Trim();

            if (text.Contains("/"))
                text = text.Replace("/", "");

            txtDate.Text = FormatStringDate(text);
        }

        private string FormatStringDate(string text/*int date*/)
        {
            //string text = txtDate.Text.Trim();

            if (text.Contains("/"))
                text = text.Replace("/", "");

            if (text.Length != 8) { txtDate.Clear(); return ""; }
            if (Convert.ToInt32(text.Substring(4, 2)) > 12 || Convert.ToInt32(text.Substring(4, 2)) < 1) { txtDate.Clear(); return ""; }
            if (Convert.ToInt32(text.Substring(6, 2)) > 31 || Convert.ToInt32(text.Substring(6, 2)) < 1) { txtDate.Clear(); return ""; }
            return text.Substring(0, 4) + "/" + text.Substring(4, 2) + "/" + text.Substring(6, 2);
        }

        private void txtTime_LostFocus(object sender, RoutedEventArgs e)
        {
            string text = txtTime.Text.Trim();

            if (text.Contains(":"))
                text = text.Replace(":", "");

            txtTime.Text = FormatStringTime(text);
        }

        private string FormatStringTime(string text)
        {
            if (text.Contains(":"))
                text = text.Replace(":", "");

            if (text.Length != 4) { txtTime.Clear(); return ""; }
            if (Convert.ToInt32(text.Substring(0, 2)) > 24 || Convert.ToInt32(text.Substring(0, 2)) < 0) { txtTime.Clear(); return ""; }
            if (Convert.ToInt32(text.Substring(2, 2)) > 59 || Convert.ToInt32(text.Substring(2, 2)) < 0) { txtTime.Clear(); return ""; }
            return text.Substring(0, 2) + ":" + text.Substring(2, 2);
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            EnableControl();

            //timerDbBakUp = null;
            //timerBackUpUsersFiles = null;
            //timerBackUpSourceControl = null;

            try
            {
                if (threadDbBakUp != null)
                    //if (threadDbBakUp.IsAlive)
                        threadDbBakUpRun = false;
                        //threadDbBakUp.Suspend();
            }
            catch (Exception exc)
            { }

            try
            {
                if (threadBackUpUsersFiles != null)
                    //if (threadBackUpUsersFiles.IsAlive)
                        threadBackUpUsersFilesRun = false;
                        //threadBackUpUsersFiles.Abort();
            }
            catch (Exception exc)
            { }

            try
            {
                if (threadBackUpSourceControl != null)
                    //if (threadBackUpSourceControl.IsAlive)
                        threadBackUpSourceControlRun = false;
                        //threadBackUpSourceControl.Abort();
            }
            catch (Exception exc)
            { }

            btnStart.IsEnabled = true;
            btnStop.IsEnabled = false;
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            //if (chkSourceFiles.IsChecked.HasValue)
            //    if (chkSourceFiles.IsChecked.Value)
            //        chkSourceFile = true;

            //if (chkUsersFiles.IsChecked.HasValue)
            //    if (chkUsersFiles.IsChecked.Value)
            //        chkUsersFile = true;

            if (chkPropertiesFiles.IsChecked.HasValue)
                if (chkPropertiesFiles.IsChecked.Value)
                    chkUsersFile = true;

            DisableControls();

            btnStart.IsEnabled = false;
            btnStop.IsEnabled = true;

            if ((cmbType.SelectedItem as ComboBoxItem).Content.Equals("همیشه"))
            {
                int hour = int.Parse(appConfigs.CreateZipStartTime.Split(":")[0]);
                int minute = int.Parse(appConfigs.CreateZipStartTime.Split(":")[1]);

                if (lstBoxDbNames.SelectedItems != null)
                    if (lstBoxDbNames.SelectedItems.Count > 0)
                    {
                        threadDbBakUp = new Thread(() => SetUpTimerDbBakUp(new TimeSpan(hour, minute, 00/*hour, minute*/)));
                        threadDbBakUp.IsBackground = true;
                        threadDbBakUpRun = true;
                        threadDbBakUp.Start();

                        //SetUpTimerDbBakUp(new TimeSpan(hour, minute, 00));
                    }

                //if (chkUsersFiles.IsChecked.HasValue)
                //    if (chkUsersFiles.IsChecked.Value)
                //    {
                //        threadBackUpUsersFiles = new Thread(() => SetUpTimerBackUpUsersFiles(new TimeSpan(hour, minute, 00/*hour, minute*/)));
                //        threadBackUpUsersFiles.IsBackground = true;
                //        threadBackUpUsersFilesRun = true;
                //        threadBackUpUsersFiles.Start();

                //        //SetUpTimerBackUpUsersFiles(new TimeSpan(hour, minute, 00));
                //    }

                //if (chkSourceFiles.IsChecked.HasValue)
                //    if (chkSourceFiles.IsChecked.Value)
                //    {
                //        threadBackUpSourceControl = new Thread(() => SetUpTimerBackUpSourceControl(new TimeSpan(hour, minute, 00/*hour, minute*/)));
                //        threadBackUpSourceControl.IsBackground = true;
                //        threadBackUpSourceControlRun = true;
                //        threadBackUpSourceControl.Start();

                //        //SetUpTimerBackUpSourceControl(new TimeSpan(hour, minute, 00));
                //    }

                if (chkPropertiesFiles.IsChecked.HasValue)
                    if (chkPropertiesFiles.IsChecked.Value)
                    {
                        threadBackUpUsersFiles = new Thread(() => SetUpTimerBackUpPropertiesFiles(new TimeSpan(hour, minute, 00/*hour, minute*/)));
                        threadBackUpUsersFiles.IsBackground = true;
                        threadBackUpUsersFilesRun = true;
                        threadBackUpUsersFiles.Start();

                        //SetUpTimerBackUpUsersFiles(new TimeSpan(hour, minute, 00));
                    }
            }
            else
            {
                if (lstBoxDbNames.SelectedItems != null)
                    if (lstBoxDbNames.SelectedItems.Count > 0)
                    {
                        CreateBackUpFromDbs();

                        //if (chkRemoveOldFiles.IsChecked.HasValue)
                        //    if (chkRemoveOldFiles.IsChecked.Value)
                        //        RemoveOldBackUpDbFiles();
                    }

                if (chkPropertiesFiles.IsChecked.HasValue)
                    if (chkPropertiesFiles.IsChecked.Value)
                    {
                        CreateBackUpFromUsersFiles();

                        //if (chkRemoveOldFiles.IsChecked.HasValue)
                        //    if (chkRemoveOldFiles.IsChecked.Value)
                        //        RemoveOldBackUsersFiles();
                    }

                //if (chkUsersFiles.IsChecked.HasValue)
                //    if (chkUsersFiles.IsChecked.Value)
                //    {
                //        CreateBackUpFromUsersFiles();

                //        if (chkRemoveOldFiles.IsChecked.HasValue)
                //            if (chkRemoveOldFiles.IsChecked.Value)
                //                RemoveOldBackUsersFiles();
                //    }

                //if (chkSourceFiles.IsChecked.HasValue)
                //    if (chkSourceFiles.IsChecked.Value)
                //    {
                //        CreateBackUpFromSourceControl();

                //        if (chkRemoveOldFiles.IsChecked.HasValue)
                //            if (chkRemoveOldFiles.IsChecked.Value)
                //                RemoveOldSourceControl();
                //    }

                EnableControl();
                btnStart.IsEnabled = true;
                btnStop.IsEnabled = false;
            }
        }

        private void SetUpTimerDbBakUp(TimeSpan alertTime)
        {
            while (threadDbBakUpRun)
            {
                DateTime current = DateTime.Now;
                TimeSpan timeOfDay = current.TimeOfDay;

                if (timeOfDay.Hours.Equals(alertTime.Hours) && timeOfDay.Minutes.Equals(alertTime.Minutes))
                {
                    CreateBackUpFromDbs();
                }

                System.Threading.Thread.Sleep(60 * 1000);
            }
            //aTimer = new System.Timers.Timer(6000);
            //// Hook up the Elapsed event for the timer. 
            //aTimer.Elapsed += MyMethod;
            //aTimer.AutoReset = true;
            //aTimer.Enabled = true;


            //DateTime current = DateTime.Now;
            //TimeSpan timeToGo = alertTime - current.TimeOfDay;
            //if (timeToGo < TimeSpan.Zero)
            //{
            //    return;//time already passed
            //}
            //this.timerDbBakUp = new System.Threading.Timer(x =>
            //{
            //    MyMethod();//this.CreateBackUpFromDbs();
            //}, null, timeToGo, Timeout.InfiniteTimeSpan);

            //var startTimeSpan = TimeSpan.Zero;
            //var periodTimeSpan = TimeSpan.FromMinutes(0.1);

            //var timer = new System.Threading.Timer((e) =>
            //{
            //    MyMethod();
            //}, null, startTimeSpan, periodTimeSpan);
        }

        //private static void MyMethod(Object source, ElapsedEventArgs e)
        //{
        //    MessageBox.Show("ok");
        //}

        //private void MyMethod()
        //{
        //    MessageBox.Show("ok");
        //}

        private void SetUpTimerBackUpPropertiesFiles(TimeSpan alertTime)
        {
            while (threadBackUpUsersFilesRun)
            {
                DateTime current = DateTime.Now;
                TimeSpan timeOfDay = current.TimeOfDay;

                if (timeOfDay.Hours.Equals(alertTime.Hours) && timeOfDay.Minutes.Equals(alertTime.Minutes))
                {
                    CreateBackUpFromUsersFiles();
                }

                System.Threading.Thread.Sleep(60 * 1000);
            }

            //DateTime current = DateTime.Now;
            //TimeSpan timeToGo = alertTime - current.TimeOfDay;
            //if (timeToGo < TimeSpan.Zero)
            //{
            //    return;//time already passed
            //}
            //this.timerBackUpUsersFiles = new System.Threading.Timer(x =>
            //{
            //    this.CreateBackUpFromUsersFiles();
            //}, null, timeToGo, Timeout.InfiniteTimeSpan);
        }

        //private void SetUpTimerBackUpUsersFiles(TimeSpan alertTime)
        //{
        //    while (threadBackUpUsersFilesRun)
        //    {
        //        DateTime current = DateTime.Now;
        //        TimeSpan timeOfDay = current.TimeOfDay;

        //        if (timeOfDay.Hours.Equals(alertTime.Hours) && timeOfDay.Minutes.Equals(alertTime.Minutes))
        //        {
        //            CreateBackUpFromUsersFiles();
        //        }

        //        System.Threading.Thread.Sleep(60 * 1000);
        //    }

        //    //DateTime current = DateTime.Now;
        //    //TimeSpan timeToGo = alertTime - current.TimeOfDay;
        //    //if (timeToGo < TimeSpan.Zero)
        //    //{
        //    //    return;//time already passed
        //    //}
        //    //this.timerBackUpUsersFiles = new System.Threading.Timer(x =>
        //    //{
        //    //    this.CreateBackUpFromUsersFiles();
        //    //}, null, timeToGo, Timeout.InfiniteTimeSpan);
        //}

        private void SetUpTimerBackUpSourceControl(TimeSpan alertTime)
        {
            while (threadBackUpSourceControlRun)
            {
                DateTime current = DateTime.Now;
                TimeSpan timeOfDay = current.TimeOfDay;

                if (timeOfDay.Hours.Equals(alertTime.Hours) && timeOfDay.Minutes.Equals(alertTime.Minutes))
                {
                    CreateBackUpFromSourceControl();
                }

                System.Threading.Thread.Sleep(60 * 1000);
            }

            //DateTime current = DateTime.Now;
            //TimeSpan timeToGo = alertTime - current.TimeOfDay;
            //if (timeToGo < TimeSpan.Zero)
            //{
            //    return;//time already passed
            //}
            //this.timerBackUpSourceControl = new System.Threading.Timer(x =>
            //{
            //    this.CreateBackUpFromSourceControl();
            //}, null, timeToGo, Timeout.InfiniteTimeSpan);
        }

        private void DisableControls()
        {
            try
            {
                cmbType.IsEnabled = false;
                chkPropertiesFiles.IsEnabled = false;
                //chkUsersFiles.IsEnabled = false;
                //chkSourceFiles.IsEnabled = false;
                //chkRemoveOldFiles.IsEnabled = false;
                lstBoxDbNames.IsEnabled = false;
            }
            catch (Exception exc)
            { }
        }

        public void EnableControl()
        {
            try
            {
                cmbType.IsEnabled = true;
                chkPropertiesFiles.IsEnabled = true;
                //chkUsersFiles.IsEnabled = true;
                //chkSourceFiles.IsEnabled = true;
                //chkRemoveOldFiles.IsEnabled = true;
                lstBoxDbNames.IsEnabled = true;
            }
            catch (Exception exc)
            { }
        }

        private void ChangeCenterDbBakUpFileName()
        {
            try
            {
                todayEn = DateTime.Now.Date;
                strToday = todayEn.ToString();
                centerDbBakUpFileName = "_backup_" + todayEn.Year.ToString() + "_" +
                    ((todayEn.Month > 9) ? todayEn.Month.ToString() : ("0" + todayEn.Month)) + "_" +
                    ((todayEn.Day > 9) ? todayEn.Day.ToString() : ("0" + todayEn.Day.ToString()));
            }
            catch (Exception exc)
            { }
        }

        private void CreateBackUpFromDbs()
        {
            ChangeCenterDbBakUpFileName();

            if (selectedDbNames != null)
                if (selectedDbNames.Count > 0)
                {
                    foreach (var dbName in selectedDbNames/*lstBoxDbNames.SelectedItems*/)
                    {
                        string directorydbName = appConfigs.DbBackUpPath + "\\" + dbName;

                        if (!System.IO.File.Exists(directorydbName + "\\" + dbName + centerDbBakUpFileName + ".zip"))
                        {
                            string dbFileNameStart = dbName + centerDbBakUpFileName + "_";

                            var mainDbFilePath = Directory.GetFiles(directorydbName).
                                Where(path => path.Contains(dbFileNameStart)).FirstOrDefault();

                            if (!string.IsNullOrEmpty(mainDbFilePath))
                                //ZipsManagement.CreateFromFile(mainDbFilePath, directorydbName + "\\" + dbName + centerDbBakUpFileName + ".zip");
                                ZipsManagement.CreateFromFileWithExt(directorydbName + "\\" + dbName + centerDbBakUpFileName + ".zip",
                                    Path.GetFileName(mainDbFilePath),
                                    mainDbFilePath);
                        }
                    }
                }
        }

        private void RemoveOldBackUpDbFiles()
        {

        }

        private void CreateBackUpFromUsersFiles()
        {
            ChangeCenterDbBakUpFileName();

            foreach (var usersFilesPath in mainUsersFilesPath)
            {
                string folderName = usersFilesPath.Substring(usersFilesPath.LastIndexOf("\\") + 1);
                string fileName = folderName + centerDbBakUpFileName + ".zip";

                ZipsManagement.CreateFromDirectory(usersFilesPath, appConfigs.DestinationUsersFilesPath + "\\" + fileName);
            }
        }

        private void RemoveOldBackUsersFiles()
        {

        }

        private void CreateBackUpFromSourceControl()
        {
            ChangeCenterDbBakUpFileName();

            foreach (var sourcePath in mainSourcePath)
            {
                string folderName = sourcePath.Substring(sourcePath.LastIndexOf("\\") + 1);
                string fileName = folderName + centerDbBakUpFileName + ".zip";

                ZipsManagement.CreateFromDirectory(sourcePath, appConfigs.DestinationSourcePath + "\\" + fileName);
            }
        }

        private void RemoveOldSourceControl()
        {

        }

        private void lstBoxDbNames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
                selectedDbNames.Add(e.AddedItems[0] as string);
            else
                if (e.RemovedItems.Count > 0)
                selectedDbNames.Remove(e.RemovedItems[0] as string);
        }
    }
}
