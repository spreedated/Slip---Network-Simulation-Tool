﻿using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using Slip.Logic;

namespace Slip.Console
{
    public static class ConsoleController
    {
        private static Process _consoleProcess;
        public static Process ConsoleProcess
        {
            get { return _consoleProcess; }
            set { _consoleProcess = value; }
        }

        internal static bool OpenConsole()
        {
            string exeName = "SlipClient.exe";
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string exePath = Path.Combine(currentDirectory, exeName);

            if (File.Exists(exePath))
            {
                // Check if PipeReader DLL exist on the directory before running the SlipClient console
                string dllPath = Path.Combine(Path.GetDirectoryName(AppContext.BaseDirectory), "PipeReader.dll");

                if (!File.Exists(dllPath))
                {
                    // File not found
                    MessageBox.Show("PipeReader dll not found in the current directory.");
                    return false;
                }

                ProcessStartInfo psi = new(exePath)
                {
                    UseShellExecute = true,
                    WindowStyle = Globals.Config.ShowCommandPrompt ? ProcessWindowStyle.Normal : ProcessWindowStyle.Hidden
                };

                ConsoleProcess = Process.Start(psi);
                ConsoleProcess.EnableRaisingEvents = true;

                // Make sure that the C# ReadFromPipe() code is running
                // and waiting for a connection to the named pipe
                // before the C++ DLL tries to connect to it.
                Thread.Sleep(500);
                return true;
            }

            // The exe was not found at the expected path.
            // Display an error message and return fail.
            MessageBox.Show("SlipClient was not found in the current directory." +
                            " Cannot start the program.");
            return false;
        }
    }
}