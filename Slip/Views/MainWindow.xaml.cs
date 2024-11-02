using NHotkey.Wpf;
using Slip.EventHandlers;
using Slip.Logic;
using Slip.Modules;
using Slip.Simulation;
using Slip.Theming;
using Slip.ViewModels;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Slip.Views
{
    public partial class MainWindow
    {
        private ModuleConfigManager _moduleConfigManager;

        internal UiEventHandlers UiEventHandlers { get; set; }
        internal ModuleConfigManager ModuleConfigManager => _moduleConfigManager;
        internal SimulationController SimulationController { get; private set; }

        public MainWindow()
        {
            InitializeComponent();

            Task.Run(async () =>
            {
                while (!Globals.ResourceDeployed)
                {
                    await Task.Delay(1000);
                }

                this.Dispatcher.Invoke(() =>
                {
                    this.Initialize();
                    ((MainWindowViewModel)this.DataContext).IsLoading = false;
                });
            });

            ((MainWindowHotkeyViewModel)this.HotkeyTab.DataContext).MainWindowInstance = this;
            Application.Current.Exit += this.Window_Closing;
        }

        private void Initialize()
        {
            IThemeChanger themeChanger;

            themeChanger = new LightThemeChanger();
            themeChanger.ChangeTheme();

            IBackgroundChanger backgroundChanger = new BackgroundChanger(tabControl, "#ECECEC");
            backgroundChanger.ChangeBackground();

            // 255, 155, 0
            themeChanger = new CustomThemeChanger(165, 165, 165);
            themeChanger.ChangeTheme();

            WinDivert.SetDllDirectory(Directory.GetCurrentDirectory()); // output directory

            UiEventHandlers = new UiEventHandlers(this);
            _moduleConfigManager = new ModuleConfigManager(this);
            SimulationController = new SimulationController(_moduleConfigManager, this);
        }

        private void Window_Closing(object sender, ExitEventArgs e)
        {
            HotkeyManager.Current.Remove("StartStop");
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            MainWindowHotkeyViewModel hvm = (MainWindowHotkeyViewModel)this.HotkeyTab.DataContext;

            if (!hvm.IsHotkeyAssigning)
            {
                return;
            }

            if (e.Key == Key.Return || e.Key == Key.Enter || e.Key == Key.Escape)
            {
                hvm.FinishHotkeyAssigning();
                return;
            }

            ((MainWindowHotkeyViewModel)this.HotkeyTab.DataContext).StartStopHotkey.AppendKeyToHotkeyCombination(e.Key);
            ((MainWindowHotkeyViewModel)this.HotkeyTab.DataContext).RefreshHotkeyString();
        }
    }
}
