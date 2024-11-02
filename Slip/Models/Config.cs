using System.Text.Json.Serialization;

namespace Slip.Models;

internal class Config
{
    [JsonInclude, JsonPropertyName("show_command_prompt")]
    internal bool ShowCommandPrompt { get; set; } = true;
    [JsonInclude, JsonPropertyName("show_window")]
    internal bool ShowWindow { get; set; } = true;
}