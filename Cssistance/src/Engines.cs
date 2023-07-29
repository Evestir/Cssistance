using CefSharp.DevTools.CSS;
using CefSharp.DevTools.IO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Shapes;
using System.Windows.Threading;
using static System.Net.WebRequestMethods;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Cssistance.src
{
    internal static class Engines
    {
        public static string Engine = null;
        public static async Task DownloadEngine()   
        {
            string StockFish = @"https://www.mediafire.com/file/h299r1npubspsph/stockfish-windows-x86-64-avx2.exe/file";
            string StockFishAnon = @"https://cdn-101.anonfiles.com/G5HcV245ze/938b4231-1690621718/stockfish-windows-x86-64-avx2.exe";
            var fileStream = new System.IO.FileStream(@"Engines\StockFish.exe", System.IO.FileMode.Create);

            using (var httpClient = new HttpClient())
            {
                try
                {
                    using (var response = await httpClient.GetAsync(StockFishAnon))
                    {
                        response.EnsureSuccessStatusCode();

                        using (var contentStream = await response.Content.ReadAsStreamAsync())
                        {
                            await contentStream.CopyToAsync(fileStream);
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    // Handle any exceptions that might occur during the download process
                    Console.WriteLine($"Error downloading file: {ex.Message}");
                }
            }
        }
    }
}
