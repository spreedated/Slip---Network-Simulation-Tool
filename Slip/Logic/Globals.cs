using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using Slip.Models;

namespace Slip.Logic;

internal static class Globals
{
    internal static bool ResourceDeployed { get; set; }
    internal static string ConfigPath { get; } = Path.Combine(AppContext.BaseDirectory, "Config.cfg");
    internal static Config Config { get; private set; }

    internal static async Task LoadConfig()
    {
        if (File.Exists(ConfigPath))
        {
            try
            {
                string json = await File.ReadAllTextAsync(ConfigPath);
                Config = JsonSerializer.Deserialize<Config>(json);

                return;
            }
            catch (Exception ex)
            {
                Debug.Print("Config got depressions like the dev while writing this.");
                Debug.Print(ex.ToString());
            }
        }

        Config = new();

        await SaveConfig();
    }

    internal static async Task SaveConfig()
    {
        try
        {
            await File.WriteAllTextAsync(ConfigPath, JsonSerializer.Serialize(Config));
        }
        catch (Exception ex)
        {
            Debug.Print("Config could not be saved. So don't dream it's over!");
            Debug.Print(ex.ToString());
        }
    }
}