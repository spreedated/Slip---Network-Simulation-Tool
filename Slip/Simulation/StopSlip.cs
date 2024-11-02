﻿using Slip.Interfaces;
using System.Runtime.InteropServices;
using System.Threading;
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
            return true;
        }
    }
}