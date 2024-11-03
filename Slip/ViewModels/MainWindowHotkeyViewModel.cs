using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Slip.Logic;
using Slip.Views;
using System.Threading.Tasks;

namespace Slip.ViewModels
{
    internal partial class MainWindowHotkeyViewModel : ObservableObject
    {
        [ObservableProperty]
        internal Hotkey startStopHotkey;

        [ObservableProperty]
        private MainWindow mainWindowInstance;

        [ObservableProperty]
        private bool isHotkeyAssigning;

        partial void OnIsHotkeyAssigningChanged(bool value)
        {
            if (value)
            {
                this.StartStopHotkey.Clear();
            }
        }

        [ObservableProperty]
        private string hotkey;

        [ObservableProperty]
        private string hotkeyButtonText = "Assign";

        [ObservableProperty]
        private bool isHotkeyButtonEnabled = true;

        [ObservableProperty]
        private bool isStartStopHotkeyEnabled;

        #region Constructor
        public MainWindowHotkeyViewModel()
        {
            this.StartStopHotkey = new("StartStop", () => this.MainWindowInstance.UiEventHandlers._buttonEventHandlers.StartButton_OnClick(this, null));

            Task.Run(async () =>
            {
                while (!Globals.IsConfigLoaded)
                {
                    await Task.Delay(500);
                }

                if (Globals.Config.StartStopHotkeyKey != default)
                {
                    this.StartStopHotkey.AssignHotkeyCombination(Globals.Config.StartStopHotkeyKey, Globals.Config.StartStopHotkeyModifierKeys);
                    this.FinishHotkeyAssigning(false);
                }
            });
        }
        #endregion

        partial void OnIsStartStopHotkeyEnabledChanged(bool value)
        {
            if (value)
            {
                this.StartStopHotkey.Activate();
                Globals.Config.IsStartStopHotkeyEnabled = true;
                Task.Run(Globals.SaveConfig);

                return;
            }

            this.StartStopHotkey.Deactivate();
            Globals.Config.IsStartStopHotkeyEnabled = false;
            Task.Run(Globals.SaveConfig);
        }

        [RelayCommand]
        private void Assign()
        {
            this.StartStopHotkey.Clear();
            this.Hotkey = null;
            this.OnPropertyChanged(nameof(this.StartStopHotkey));

            this.IsHotkeyAssigning = true;
            this.HotkeyButtonText = "Press keys";
        }

        [RelayCommand]
        private void Unassign()
        {
            this.StartStopHotkey.Clear();
            this.Hotkey = null;
            this.OnPropertyChanged(nameof(this.StartStopHotkey));

            this.SaveStartStopHotkeyToConfig();
        }

        public void FinishHotkeyAssigning(bool autoEnable = true)
        {
            this.IsHotkeyAssigning = false;
            this.HotkeyButtonText = "Assign";
            if (autoEnable)
            {
                this.IsStartStopHotkeyEnabled = false;
                this.IsStartStopHotkeyEnabled = true;
                this.OnPropertyChanged(nameof(this.StartStopHotkey));
            }
            this.Hotkey = this.StartStopHotkey.ToString();

            this.SaveStartStopHotkeyToConfig();
        }

        private void SaveStartStopHotkeyToConfig()
        {
            Globals.Config.StartStopHotkeyKey = this.StartStopHotkey.HotkeyKey;
            Globals.Config.StartStopHotkeyModifierKeys = this.StartStopHotkey.Modifierkeys;
            Globals.Config.IsStartStopHotkeyEnabled = this.StartStopHotkey.IsEnabled;

            Task.Run(Globals.SaveConfig);
        }

        public void RefreshHotkeyString()
        {
            this.Hotkey = this.StartStopHotkey.ToString();
        }
    }
}
