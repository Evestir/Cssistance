using CefSharp;
using Cssistance.src;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Cssistance
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();

        private string LastSavedURL;

        private const int DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1 = 19;
        private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
        private const int DWMWA_CAPTION_COLOR = 35;

        private static void SetTitleBarColor(IntPtr handle, int r, int g, int b)
        {
            [DllImport("dwmapi.dll")]
            static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, int[] attrValue, int attrSize);

            int[] colorstr = new int[] { 0x202020 };
            DwmSetWindowAttribute(handle, DWMWA_CAPTION_COLOR, colorstr, 4);
        }

        private static bool UseImmersiveDarkMode(IntPtr handle, bool enabled)
        {
            if (IsWindows10OrGreater(17763))
            {
                var attribute = DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1;
                if (IsWindows10OrGreater(18985))
                {
                    attribute = DWMWA_USE_IMMERSIVE_DARK_MODE;
                }

                bool value = enabled;
                [DllImport("dwmapi.dll")]
                static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, [In] ref bool attrValue, int attrSize);

                int useImmersiveDarkMode = enabled ? 1 : 0;
                if (DwmSetWindowAttribute(handle, (int)attribute, ref value, Marshal.SizeOf<bool>()) == 1) return true;
                else return false;
            }

            return false;
        }

        private static bool IsWindows10OrGreater(int build = -1)
        {
            return Environment.OSVersion.Version.Major >= 10 && Environment.OSVersion.Version.Build >= build;
        }

        public MainWindow()
        {
            InitializeComponent();
            AllocConsole();
            UseImmersiveDarkMode(new WindowInteropHelper(this).EnsureHandle(), true);
            BrowserIns = Browser;
            GetEnginesReady();
        }

        public void Notify(string message, int second)
        {
            Snackbar.MessageQueue?.Enqueue(message, null, null, null, false, true, TimeSpan.FromSeconds(second));
        }

        public static CefSharp.Wpf.HwndHost.ChromiumWebBrowser BrowserIns;

        private void ShowDevToolsBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Browser.IsBrowserInitialized)
            {
                Browser.ShowDevTools();
            }
        }

        private void TestBtn_Click(object sender, RoutedEventArgs e)
        {
            CheckEngines();
        }

        private void BrowserContainer_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("dsfdsfdsfd");

            if (Browser.Address != LastSavedURL)
            {
                Thread.Sleep(1000);

                BoardManager.SetMySide();
                LastSavedURL = Browser.Address;

                switch (BoardManager.GameStatus)
                {
                    case "White":
                        GameStatusLabel.Content = "White";
                        break;
                    case "Black":
                        GameStatusLabel.Content = "Black";
                        break;
                    case "GameNotInit":
                        GameStatusLabel.Content = "White";
                        break;
                }
            }
        }

        [DllImport("shlwapi.dll")]
        public static extern bool PathIsDirectoryEmpty(string pszPath);

        private void CheckEngines()
        {
            if (Directory.Exists(@"Engines\") && Directory.EnumerateFileSystemEntries(@"Engines\").Any())
            {
                GetEnginesReady();
            }
            else
            {
                Notify("⚠️ Engine Not Found", 1);
                Directory.CreateDirectory(@"Engines\");
            }
        }

        private void AnalyzeBtn_Click(object sender, RoutedEventArgs e)
        {
            Board.Coords = new SortedDictionary<int, int>();

            BoardManager.SetMySide();
            BoardManager.FindPieces();

            Board.CurrentFEN = Board.ToFen(Board.Coords);

            Console.WriteLine(Board.CurrentFEN);
            UCI UCIProc = new UCI();

            UCIProc.BestMove(3000, Board.CurrentFEN, "\\Engines\\"+ Engines.Engine);
        }

        private void EnginesChoice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EnginesChoice.SelectedItem != null)
            {
                Engines.Engine = EnginesChoice.SelectedItem.ToString();
            }
        }

        public void GetEnginesReady()
        {
            string EnginePath = @"Engines\";

            this.EnginesChoice.Items.Clear();

            string[] Cengines = Directory.GetFiles(EnginePath);
            Notify("ℹ️ Found Engine(s)", 1);
            foreach (string engine in Cengines)
            {
                Console.Write(engine);
                this.EnginesChoice.Items.Add(System.IO.Path.GetFileNameWithoutExtension(engine));
            }
            Console.Write('\n');
        }
    }
}
