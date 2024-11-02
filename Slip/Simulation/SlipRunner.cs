using Slip.Console;
using Slip.Interfaces;
using Slip.Modules;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Controls;

namespace Slip.Simulation
{
    public class SlipRunner : IRunSlip
    {
        private static bool first = true;
        private static Thread _workerThread;
        private Label statusLabel;

        [DllImport("Slip2.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void startProgram([MarshalAs(UnmanagedType.LPStr)] string filterText, Config[] configs, int configCount);

        public SlipRunner(Label statusLabel)
        {
            this.statusLabel = statusLabel;
        }

        public void Run(string filter, Config[] configArray)
        {
            if (first)
            {
                // Open Console log
                if (!ConsoleController.OpenConsole()) return;
                first = false;
            }

            // Start new simulation process
            _workerThread = new Thread(() => startProgram(filter, configArray, configArray.Length));
            _workerThread.Start();

            statusLabel.Content = "ENABLED";
        }
    }
}