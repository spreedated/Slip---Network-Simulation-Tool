using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Slip.Logic;
using System.Reflection;
using System.Threading.Tasks;

namespace Slip.ViewModels;

internal partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    private bool isLoading = true;

    [ObservableProperty]
    private string title = "Slip";

    [ObservableProperty]
    private bool isWindowVisible = true;

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
}