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

namespace InfoLoggerWpf
{
    public partial class MainWindow : Window
    {
        public static string path = "C:\\work\\InfoLoggerLog.csv";
        public static string intoLables;
        public static int stringLength;
        public static string[] labels = new string[5];
        private static string compNameFromReg()
        {
            RegistryKey regedit = Registry.CurrentUser.OpenSubKey("Volatile Environment", true);
            string currentLoggedUserName = regedit.GetValue("USERNAME").ToString();

            if (currentLoggedUserName == "")
            {
                //Console.WriteLine("No user name!");
                return "N/A";
            }
            else
                return currentLoggedUserName;
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

        public static void sendDataToLabel()
        {
            intoLables = readFromFile();
            stringLength = readFromFile().Length;
            int n =0;
             
             for (int i = 0; i < stringLength; i++)
             {
                 if (intoLables[i] != ',')
                     labels[n] += intoLables[i];
                 else
                     n++;
             }
        }

        public MainWindow()
        {
            InitializeComponent();
            Left = System.Windows.SystemParameters.WorkArea.Width - Width;
            Top = System.Windows.SystemParameters.WorkArea.Height - Height;
            //Console.ReadKey();

            string machineName = Environment.MachineName;
            string userName = Environment.UserName;
            //Console.WriteLine("Current logged user: ");
            // Console.WriteLine(machineName + " " + userName);

            //Currently logged user
            string loggedUser = compNameFromReg();

            createFile();
            readFromFile();
            writeToFile(shiftListAndReturnResults(convertDataToList(), loggedUser));
            sendDataToLabel();

        }

        private void Label_Loaded(object sender, RoutedEventArgs e)
        {
            var label1 = sender as Label;
            label1.Content = labels[0];

            var label2 = sender as Label;
            label2.Content = labels[1];

            var label3 = sender as Label;
            label3.Content = labels[2];

            var label4 = sender as Label;
            label4.Content = labels[3];

            //var label5 = sender as Label;
           // label5.Content = labels[5];
        }
    }
}