using Slip.Interfaces;
using Slip.Logic;
using Slip.ViewModels;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Slip.Simulation
{
    public class StopSlip : IStopSimulation
    {
        private Button stateButton;

        public StopSlip(Button stateButton)
        {
            this.stateButton = stateButton;
        }

        [DllImport("Slip2.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void stopProgram();

        public bool Stop()
        {
            stopProgram(); // call the function dll
            Thread.Sleep(250);
            stateButton.Content = "Start";
            ((MainWindowViewModel)Application.Current.MainWindow.DataContext).ToggleTaskbarIcon();

            if (!Globals.Config.ShowCommandPrompt)
            {
                Task.Run(async () =>
                {
                    await Task.Delay(2000);
                    Process.GetProcessesByName("SlipClient")[0].Kill();
                });
            }

            return true;
        }
    }
}