using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Slip.Logic;
using Slip.Utils;
using Slip.Views;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace Slip.ViewModels;

internal partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    private bool isLoading = true;

    [ObservableProperty]
    private string title = "Slip";

    [ObservableProperty]
    private bool isWindowVisible = true;

    [ObservableProperty]
    private bool toggleShowCommandPrompt = Globals.Config.ShowCommandPrompt;

    public MainWindowViewModel()
    {
        this.Title = $"Slip v{typeof(MainWindowViewModel).Assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version}";
        this.IsWindowVisible = Globals.Config.ShowWindow;
    }

    [RelayCommand]
    private void OnToggleWindow() {
        this.IsWindowVisible ^= true;
        Globals.Config.ShowWindow ^= true;

        Task.Run(Globals.SaveConfig);
    }

    [RelayCommand]
    private void ChangeCommandPromptButton()
    {
        Globals.Config.ShowCommandPrompt ^= true;
        Task.Run(Globals.SaveConfig);
    }
}