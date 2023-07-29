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
            Thread BrowserUIDrawer = new Thread(IndicateMove);
            BrowserUIDrawer.Start();
        }

        private void IndicateMove()
        {
            while (true)
            {
                if (Board.BestMove != null)
                {
                    BoardManager.ClearIndications();
                    BoardManager.DrawIndication(Board.BestMove);
                    Console.WriteLine("Ok found bestmove injected js.");
                    Board.BestMove = null;
                    break;
                }
                Thread.Sleep(100);
            }
            return;
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
            string Output = outLine.Data;

            if (!string.IsNullOrEmpty(Output))
            {
                if (Output.Contains("bestmove"))
                {
                    Output = Output.Substring(Output.IndexOf(' ') + 1);
                    Output = Output.Substring(0, Output.IndexOf(' '));
                    Board.BestMove = Output;
                }

                Console.WriteLine(outLine.Data);
            }
        }
    }
}
