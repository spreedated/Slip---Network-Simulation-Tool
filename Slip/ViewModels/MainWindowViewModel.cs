using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Slip.Logic;
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Slip.ViewModels;

internal partial class MainWindowViewModel : ObservableObject
{
    private readonly static ImageSource defaultTraybarIcon = new BitmapImage(new Uri(Constants.DEFAULT_TRAYBAR_ICON_URI));
    private readonly static ImageSource activeTraybarIcon = new BitmapImage(new Uri(Constants.ACTIVE_TRAYBAR_ICON_URI));

    [ObservableProperty]
    private bool isLoading = true;

    [ObservableProperty]
    private string title = "Slip";

    [ObservableProperty]
    private bool isWindowVisible = true;

    [ObservableProperty]
    private bool toggleShowCommandPrompt = Globals.Config.ShowCommandPrompt;

    [ObservableProperty]
    private ImageSource traybarIcon = defaultTraybarIcon;

    public MainWindowViewModel()
    {
        this.Title = $"Slip v{typeof(MainWindowViewModel).Assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version}";
        this.IsWindowVisible = Globals.Config.ShowWindow;
    }

    [RelayCommand]
    private void OnToggleWindow()
    {
        this.IsWindowVisible ^= true;
        Globals.Config.ShowWindow ^= true;

        Task.Run(Globals.SaveConfig);
    }

    [RelayCommand]
    private static void ChangeCommandPromptButton()
    {
        Globals.Config.ShowCommandPrompt ^= true;
        Task.Run(Globals.SaveConfig);
    }

    public void ToggleTaskbarIcon()
    {
        if (this.TraybarIcon.Equals(defaultTraybarIcon))
        {
            this.TraybarIcon = activeTraybarIcon;
            return;
        }

        this.TraybarIcon = defaultTraybarIcon;
    }
}