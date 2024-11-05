using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Logging;
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
        private readonly static LogEventLevel level = LogEventLevel.Verbose;

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

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.Verbose()
                .WriteTo.Debug(restrictedToMinimumLevel: level)
                .Enrich.WithProperty("application", typeof(App).Assembly.GetName().Name)
                .Enrich.WithProperty("version", typeof(App).Assembly.GetName().Version)
                .CreateLogger();

            Microsoft.Extensions.Logging.ILogger logger = new SerilogLoggerFactory().CreateLogger("application");

            logger.LogTrace("Application started");

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