using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Slip.Logic;
using Slip.Views;

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
        }
        #endregion

        partial void OnIsStartStopHotkeyEnabledChanged(bool value)
        {
            if (value)
            {
                this.StartStopHotkey.Activate();
                return;
            }

            this.StartStopHotkey.Deactivate();
        }

        [RelayCommand]
        private void Assign()
        {
            this.StartStopHotkey.Clear();
            this.IsHotkeyAssigning = true;
            this.HotkeyButtonText = "Press keys";
        }

        [RelayCommand]
        private void Unassign()
        {
            this.StartStopHotkey.Clear();
            this.Hotkey = null;
            this.OnPropertyChanged(nameof(this.StartStopHotkey));
        }

        public void FinishHotkeyAssigning()
        {
            this.IsHotkeyAssigning = false;
            this.HotkeyButtonText = "Assign";
            this.IsStartStopHotkeyEnabled = false;
            this.IsStartStopHotkeyEnabled = true;
            this.Hotkey = this.StartStopHotkey.ToString();
            this.OnPropertyChanged(nameof(this.StartStopHotkey));
        }

        public void RefreshHotkeyString()
        {
            this.Hotkey = this.StartStopHotkey.ToString();
        }
    }
}
