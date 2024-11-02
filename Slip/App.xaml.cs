using Slip.Logic;
using Slip.Views;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace Slip
{
    /// <summary>Interaction logic for App.xaml</summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Task.Run(Globals.LoadConfig).ConfigureAwait(false).GetAwaiter().GetResult();
            Task.Run(async () =>
            {
                await CheckAndDeployResource("Slip2.dll");
                await CheckAndDeployResource("PipeReader.dll");
                await CheckAndDeployResource("SlipClient.exe");
                await CheckAndDeployResource("libcrypto-3-x64.dll");
                await CheckAndDeployResource("WinDivert.dll");
                await CheckAndDeployResource("WinDivert.lib");
                await CheckAndDeployResource("WinDivert64.sys");

                Globals.ResourceDeployed = true;
            });

            base.OnStartup(e);
        }

        private static async Task CheckAndDeployResource(string resName)
        {
            string filepath = Path.Combine(AppContext.BaseDirectory, resName);

            if (!File.Exists(filepath))
            {
                await Task.Run(() =>
                {
                    using Stream stream = typeof(App).Assembly.GetManifestResourceStream($"Slip.{resName}");
                    using FileStream fileStream = File.Create(filepath);
                    stream.CopyTo(fileStream);
                });
            }
        }

        private void Application_Startup(object sender, StartupEventArgs e) => this.MainWindow = new MainWindow();
    }
}