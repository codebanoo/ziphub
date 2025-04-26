using FrameWork;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
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
using Wpf.DownloadZipFiles.VM;

namespace Wpf.DownloadZipFiles
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        AppConfigs appConfigs = new AppConfigs();
        List<string> dbNames = new List<string>();
        List<string> selectedDbNames = new List<string>();
        List<string> sourceNamesList = new List<string>();
        List<string> usersFilesList = new List<string>();

        DateTime todayEn = DateTime.Now.Date;

        string strToday = "";
        string centerDbBakUpFileName = "";

        private Thread threadDownloadBakUp;
        private bool threadDownloadBakUpRun;

        private bool chkUsersFile = false;
        private bool chkSourceFile = false;

        public MainWindow()
        {
            InitializeComponent();

            strToday = todayEn.ToString();
            centerDbBakUpFileName = "_backup_" + todayEn.Year.ToString() + "_" +
                ((todayEn.Month > 9) ? todayEn.Month.ToString() : ("0" + todayEn.Month)) + "_" +
                ((todayEn.Day > 9) ? todayEn.Day.ToString() : ("0" + todayEn.Day.ToString()));

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            appConfigs.Ip = config.AppSettings.Settings["Ip"].Value;
            appConfigs.Port = config.AppSettings.Settings["Port"].Value;
            appConfigs.DbNames = config.AppSettings.Settings["DbNames"].Value;
            appConfigs.DownloadRootPath = config.AppSettings.Settings["DownloadRootPath"].Value;
            appConfigs.SourceNames = config.AppSettings.Settings["SourceNames"].Value;
            appConfigs.UsersFiles = config.AppSettings.Settings["UsersFiles"].Value;
            appConfigs.DestinationPath = config.AppSettings.Settings["DestinationPath"].Value;
            appConfigs.DownloadZipStartTime = config.AppSettings.Settings["DownloadZipStartTime"].Value;
            appConfigs.Auto = bool.Parse(config.AppSettings.Settings["Auto"].Value);

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

            if (!string.IsNullOrEmpty(appConfigs.SourceNames))
            {
                sourceNamesList = appConfigs.SourceNames.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
            }

            if (!string.IsNullOrEmpty(appConfigs.UsersFiles))
            {
                usersFilesList = appConfigs.UsersFiles.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
            }

            txtDate.Text = PersianDate.DateNow;
            txtTime.Text = PersianDate.TimeNow;

            threadDownloadBakUpRun = true;

            if (!System.IO.Directory.Exists(appConfigs.DestinationPath))
            {
                System.IO.Directory.CreateDirectory(appConfigs.DestinationPath);
                System.Threading.Thread.Sleep(100);
            }

            if (appConfigs.Auto)
            {
                dbNames = appConfigs.DbNames.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();

                if (dbNames != null)
                    if (dbNames.Count > 0)
                    {
                        foreach (var dbName in dbNames)
                        {
                            selectedDbNames.Add(dbName);
                        }
                    }

                DisableControls();

                int hour = int.Parse(appConfigs.DownloadZipStartTime.Split(":")[0]);
                int minute = int.Parse(appConfigs.DownloadZipStartTime.Split(":")[1]);

                threadDownloadBakUp = new Thread(() => SetUpDownloadBakUp(new TimeSpan(hour, minute, 00/*hour, minute*/)));
                threadDownloadBakUp.IsBackground = true;
                threadDownloadBakUpRun = true;
                threadDownloadBakUp.Start();
            }
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

            //timerDownloadBakUp = null;

            try
            {
                if (threadDownloadBakUp != null)
                    //if (threadDbBakUp.IsAlive)
                    threadDownloadBakUpRun = false;
                //threadDbBakUp.Suspend();
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
                int hour = int.Parse(appConfigs.DownloadZipStartTime.Split(":")[0]);
                int minute = int.Parse(appConfigs.DownloadZipStartTime.Split(":")[1]);

                threadDownloadBakUp = new Thread(() => SetUpDownloadBakUp(new TimeSpan(hour, minute, 00/*hour, minute*/)));
                threadDownloadBakUp.IsBackground = true;
                threadDownloadBakUpRun = true;
                threadDownloadBakUp.Start();

                //if (lstBoxDbNames.SelectedItems != null)
                //    if (lstBoxDbNames.SelectedItems.Count > 0)
                //SetUpDownloadBakUp(new TimeSpan(hour, minute, 00));
            }
            else
            {
                //if (lstBoxDbNames.SelectedItems != null)
                //    if (lstBoxDbNames.SelectedItems.Count > 0)
                //    {
                DownloadBakUp();
                //}

                EnableControl();
                btnStart.IsEnabled = true;
                btnStop.IsEnabled = false;
            }
        }

        private void SetUpDownloadBakUp(TimeSpan alertTime)
        {
            while (threadDownloadBakUpRun)
            {
                DateTime current = DateTime.Now;
                TimeSpan timeOfDay = current.TimeOfDay;

                if (timeOfDay.Hours.Equals(alertTime.Hours) && timeOfDay.Minutes.Equals(alertTime.Minutes))
                {
                    DownloadBakUp();
                }

                System.Threading.Thread.Sleep(60 * 1000);
            }
            //DateTime current = DateTime.Now;
            //TimeSpan timeToGo = alertTime - current.TimeOfDay;
            //if (timeToGo < TimeSpan.Zero)
            //{
            //    return;//time already passed
            //}
            //this.timerDownloadBakUp = new System.Threading.Timer(x =>
            //{
            //    this.DownloadBakUp();
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
                lstBoxDbNames.IsEnabled = true;
            }
            catch (Exception exc)
            { }
        }

        private void ChangeCenterZipFileName()
        {
            try
            {
                todayEn = DateTime.Now.Date;
                strToday = todayEn.ToString();
                centerDbBakUpFileName = "_backup_" + todayEn.Year.ToString() + "_" +
                    ((todayEn.Month > 9) ? todayEn.Month.ToString() : ("0" + todayEn.Month)) + "_" +
                    ((todayEn.Day > 9) ? todayEn.Day.ToString() : ("0" + todayEn.Day.ToString()));

                if (!System.IO.Directory.Exists(appConfigs.DestinationPath + "\\" + PersianDate.DateNow.Replace("/", "_")))
                {
                    System.IO.Directory.CreateDirectory(appConfigs.DestinationPath + "\\" + PersianDate.DateNow.Replace("/", "_"));
                    System.Threading.Thread.Sleep(100);
                }
            }
            catch (Exception exc)
            { }
        }

        private void lstBoxDbNames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
                selectedDbNames.Add(e.AddedItems[0] as string);
            else
                if (e.RemovedItems.Count > 0)
                selectedDbNames.Remove(e.RemovedItems[0] as string);
        }

        private void DownloadBakUp()
        {
            ChangeCenterZipFileName();

            #region download db zip files

            try
            {
                if (selectedDbNames != null)
                    if (selectedDbNames.Count > 0)
                    {
                        try
                        {
                            foreach (var dbName in selectedDbNames/*lstBoxDbNames.SelectedItems*/)
                            {
                                string fileName = appConfigs.DestinationPath + "\\" +
                                    PersianDate.DateNow.Replace("/", "_") + "\\" +
                                    dbName + centerDbBakUpFileName + ".zip";

                                if (!System.IO.File.Exists(fileName))
                                {
                                    string address = appConfigs.Ip + ":" + appConfigs.Port + ("/" +
                                        appConfigs.DownloadRootPath + "/" + dbName + "/" + dbName + centerDbBakUpFileName + ".zip").Replace("//", "/");

                                    try
                                    {
                                        using (var client = new ExtendedWebClient(new Uri(address)))
                                        {
                                            client.DownloadFileAsync(new Uri(address), fileName);
                                        }
                                    }
                                    catch (Exception exc)
                                    {
                                        string message = "";

                                        message = exc.Message;

                                        if (exc.InnerException != null)
                                            message += " " + exc.InnerException.Message;

                                        MessageBox.Show(message);
                                    }
                                }
                            }
                        }
                        catch (Exception exc)
                        {
                            string message = "";

                            message = exc.Message;

                            if (exc.InnerException != null)
                                message += " " + exc.InnerException.Message;

                            MessageBox.Show(message);
                        }
                    }
            }
            catch (Exception exc)
            {
                string message = "";

                message = exc.Message;

                if (exc.InnerException != null)
                    message += " " + exc.InnerException.Message;

                MessageBox.Show(message);
            }

            #endregion

            #region download source zip files

            try
            {
                //if (chkSourceFiles.IsChecked.HasValue)
                //    if (chkSourceFiles.IsChecked.Value)
                if (chkSourceFile)
                {
                    if (sourceNamesList != null)
                        if (sourceNamesList.Count > 0)
                        {
                            foreach (var sourceName in sourceNamesList/*lstBoxDbNames.SelectedItems*/)
                            {
                                string fileName = appConfigs.DestinationPath + "\\" +
                                    PersianDate.DateNow.Replace("/", "_") + "\\" +
                                    sourceName + centerDbBakUpFileName + ".zip";

                                if (!System.IO.File.Exists(fileName))
                                {
                                    string address = appConfigs.Ip + ":" + appConfigs.Port + ("/" +
                                        appConfigs.DownloadRootPath + "/GitRepo/" + sourceName + centerDbBakUpFileName + ".zip").Replace("//", "/");

                                    try
                                    {
                                        using (var client = new ExtendedWebClient(new Uri(address)))
                                        {
                                            client.DownloadFileAsync(new Uri(address), fileName);
                                        }
                                    }
                                    catch (Exception exc)
                                    { }
                                }
                            }
                        }
                }
            }
            catch (Exception exc)
            {
                string message = "";

                message = exc.Message;

                if (exc.InnerException != null)
                    message += " " + exc.InnerException.Message;

                MessageBox.Show(message);
            }

            #endregion

            #region download user zip files

            try
            {
                if (chkUsersFile)
                //if (chkUsersFiles.IsChecked.HasValue)
                //    if (chkUsersFiles.IsChecked.Value)
                {
                    if (usersFilesList != null)
                        if (usersFilesList.Count > 0)
                        {
                            foreach (var userFile in usersFilesList/*lstBoxDbNames.SelectedItems*/)
                            {
                                string fileName = appConfigs.DestinationPath + "\\" +
                                    PersianDate.DateNow.Replace("/", "_") + "\\" +
                                    userFile + centerDbBakUpFileName + ".zip";

                                if (!System.IO.File.Exists(fileName))
                                {
                                    string address = appConfigs.Ip + ":" + appConfigs.Port + ("/" +
                                        appConfigs.DownloadRootPath + "/PropertiesFiles/" + userFile + centerDbBakUpFileName + ".zip").Replace("//", "/");

                                    try
                                    {
                                        using (var client = new ExtendedWebClient(new Uri(address)))
                                        {
                                            client.DownloadFileAsync(new Uri(address), fileName);
                                        }
                                    }
                                    catch (Exception exc)
                                    { }
                                }
                            }
                        }
                }
            }
            catch (Exception exc)
            {
                string message = "";

                message = exc.Message;

                if (exc.InnerException != null)
                    message += " " + exc.InnerException.Message;

                MessageBox.Show(message);
            }

            #endregion
        }
    }
}
