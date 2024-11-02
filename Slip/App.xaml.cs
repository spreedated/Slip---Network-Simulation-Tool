using Slip.Logic;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace Slip
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Task.Run(async () =>
            {
                await CheckAndDeployResource("Slip2.dll");
                await CheckAndDeployResource("PipeReader.dll");
                await CheckAndDeployResource("SlipClient.exe");
                await CheckAndDeployResource("libcrypto-3-x64.dll");
                await CheckAndDeployResource("WinDivert.dll");

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
                    using (Stream stream = typeof(App).Assembly.GetManifestResourceStream($"Slip.{resName}"))
                    {
                        using (FileStream fileStream = File.Create(filepath))
                        {
                            stream.CopyTo(fileStream);
                        }
                    }
                });
            }
        }
    }
}
