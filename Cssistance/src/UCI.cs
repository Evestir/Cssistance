using Microsoft.VisualBasic;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Cssistance.src.Piece;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace Cssistance.src
{
    internal class UCI
    {
        private Process stockfishProcess = null;
        private StreamWriter stockfishInput;
        private StreamReader stockfishOutput;

        public void BestMove(int thinkTime, string FEN, string EnginePath)
        {
            if (stockfishProcess == null) Start(EnginePath);

            // Important: Wait for the engine to be ready before sending commands
            SendCommand("ucinewgame");
            SendCommand("position fen " +FEN);
            SendCommand("go movetime 3000");
        }

        public void StartEngine(string enginePath)
        {
            stockfishProcess = new Process();
            stockfishProcess.StartInfo.FileName = enginePath;
            stockfishProcess.StartInfo.UseShellExecute = false;
            stockfishProcess.StartInfo.RedirectStandardInput = true;
            stockfishProcess.StartInfo.RedirectStandardOutput = true;
            stockfishProcess.StartInfo.RedirectStandardError = true; // Redirect standard error for error handling
            stockfishProcess.Start();

            stockfishInput = stockfishProcess.StandardInput;
            stockfishOutput = stockfishProcess.StandardOutput;

            // Important: Wait for the engine to be ready before sending commands
            SendCommand("uci");
            WaitForReady();
        }

        public void SendCommand(string command)
        {
            stockfishInput.WriteLine(command);
        }

        public void WaitForReady()
        {
            SendCommand("isready");
        }

        public void StopEngine()
        {
            stockfishInput.WriteLine("quit");
            stockfishProcess.WaitForExit();
        }


        // StackOf

        private Process engineProcess;

        private void Start(string pathToExe)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();

            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardInput = true;
            startInfo.FileName = pathToExe;

            try
            {
                stockfishProcess = Process.Start(startInfo);
                stockfishProcess.StandardInput.WriteLine("uci");
            }
            catch
            {
                Console.WriteLine("Error when starting Engine");
            }

            //Added these two lines so the method "engineOutputHandler" 
            //gets automatically called every time an output is received
            stockfishProcess.OutputDataReceived += engineOutputHandler;
            stockfishProcess.BeginOutputReadLine();

            stockfishInput = stockfishProcess.StandardInput;
            WaitForReady();
        }

        private void Update()
        {

            stockfishProcess.StandardInput.WriteLine("uci");

            //removed the entire while loop

        }

        //Added this method, which prints out the data received
        private void engineOutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            if (!string.IsNullOrEmpty(outLine.Data))
            {
                Console.WriteLine(outLine.Data);
            }
        }
    }
}
