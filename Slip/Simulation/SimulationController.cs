using Slip.Interfaces;
using Slip.Modules;
using Slip.Views;
using System.IO;

namespace Slip.Simulation
{
    internal class SimulationController
    {
        private MainWindow _mainWindow;
        private readonly SlipRunner _slipRunner;
        private readonly IStartSimulation _startSimulation;
        private readonly IStopSimulation _stopSimulation;

        internal SimulationController(IModuleConfigManager moduleConfigManager, MainWindow _mainWindow)
        {
            this._mainWindow = _mainWindow;

            IFileExistence fileExistence = new SlipDllExistence(Path.Combine
            (Path.GetDirectoryName(System.AppContext.BaseDirectory),
                "Slip2.dll"));

            _slipRunner = new SlipRunner(_mainWindow.statusLabel);
            _startSimulation = new StartSlip(fileExistence, moduleConfigManager, _slipRunner);
            _stopSimulation = new StopSlip(_mainWindow.startButton);
        }

        public bool StartSimulation(string filter, ref bool stopped)
        {
            if (!_startSimulation.Start(filter)) return false;

            _mainWindow.startButton.Content = "Stop";
            stopped = false;
            return true;

        }

        public bool StopSimulation()
        {
            _mainWindow.statusLabel.Content = "DISABLED";
            return _stopSimulation.Stop();
        }

        public bool ResetSimulation()
        {
            _mainWindow.statusLabel.Content = "DISABLED";
            bool stopped = _stopSimulation.Stop();
            SlipRunner.Reset();
            return stopped;
        }
    }
}