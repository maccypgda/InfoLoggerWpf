using System;
using System.Collections.Generic;
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
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Diagnostics;
using System.Threading;
using Microsoft.Win32;
using System.Reflection;
using System.Globalization;


namespace InfoLoggerWpf
{
    public partial class MainWindow : Window
    {
        public static string path = "C:\\work\\InfoLoggerLog.csv";

        private static string compNameFromReg()
        {
            string keyPath = null;
            String registryKey = @"Volatile Environment";

            RegistryKey tempKey = null;

            using (Microsoft.Win32.RegistryKey key = Registry.CurrentUser.OpenSubKey(registryKey))
            {
                foreach (string subKeyName in key.GetSubKeyNames())
                {
                    using (tempKey = key.OpenSubKey(subKeyName))
                    {
                        keyPath = tempKey.Name.Remove(0, 18);
                    }
                }
            }

            //RegistryKey regedit = Registry.CurrentUser.OpenSubKey("Volatile Environment\\1", true);
            RegistryKey regedit = Registry.CurrentUser.OpenSubKey(keyPath, true);
            string currentLoggedUserName = regedit.GetValue("CLIENTNAME").ToString();

            if (currentLoggedUserName == "")
            {
                //Console.WriteLine("No user name!");
                return "N/A";
            }
            else
                return currentLoggedUserName +" "+ DateTime.Now;

        }
        public static void checkDirectory()
        {
            string dirPath = @"C:\work";
            if (Directory.Exists(dirPath))
            {
                return;
            }
            else
                Directory.CreateDirectory(dirPath);
        }

        public static void writeToFile(string dataToWrite)
        {
            StreamWriter sw = new StreamWriter(path);
            sw.Write(dataToWrite);
            sw.Close();
        }

        public static string readFromFile()
        {
            StreamReader sr = new StreamReader(path);
            string fileContents = sr.ReadLine();
            sr.Close();

            return fileContents;
        }

        public static void createFile()
        {
            string initialDataToWrite = "N/A,N/A,N/A,N/A,N/A";

            if (!File.Exists(path))
            {
                File.Create(path).Close();
                writeToFile(initialDataToWrite);
            }
        }

        public static List<string> convertDataToList()
        {
            string fileData = readFromFile();
            List<string> userList = new List<string>();
            // add try//catch exc
            var userEntries = fileData.Split(',');

            foreach (string user in userEntries)
            {
                userList.Add(user);
            }
            return userList;
        }

        public static string shiftListAndReturnResults(List<string> userList, string newUser)
        {
            userList.RemoveAt(0);
            userList.Add(newUser);

            return String.Join(",", userList);
        }

        public MainWindow()
        {
            try
            {
            InitializeComponent();
            Left = System.Windows.SystemParameters.WorkArea.Width - Width;
            Top = System.Windows.SystemParameters.WorkArea.Height - Height;

            string machineName = Environment.MachineName;
            string userName = Environment.UserName;

            //Currently logged user
            string loggedUser = compNameFromReg();

            //chech if dir exist
            checkDirectory();

            createFile();
            readFromFile();
            writeToFile(shiftListAndReturnResults(convertDataToList(), loggedUser));

            textBlock.Text = convertDataToList()[0];
            textBlock1.Text = convertDataToList()[1];
            textBlock2.Text = convertDataToList()[2];
            textBlock3.Text = convertDataToList()[3];
            textBlock4.Text = convertDataToList()[4];     
            }

            catch(Exception ex)
            {
               File.WriteAllText(@"C:\work\log.txt", ex.ToString());
            }
                   
        }
    }
}