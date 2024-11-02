using CommunityToolkit.Mvvm.ComponentModel;
using System.Reflection;

namespace Slip.ViewModels
{
    internal partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool isLoading = true;

        [ObservableProperty]
        private string title = "Slip";

        public MainWindowViewModel()
        {
            this.Title = $"Slip v{typeof(MainWindowViewModel).Assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version}";
        }
    }
}
