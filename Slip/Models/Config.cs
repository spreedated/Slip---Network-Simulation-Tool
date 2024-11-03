using System.Text.Json.Serialization;
using System.Windows.Input;

namespace Slip.Models;

internal class Config
{
    [JsonInclude, JsonPropertyName("show_command_prompt")]
    internal bool ShowCommandPrompt { get; set; } = true;
    [JsonInclude, JsonPropertyName("show_window")]
    internal bool ShowWindow { get; set; } = true;
    [JsonInclude, JsonPropertyName("startStopHotkeyKey")]
    internal Key StartStopHotkeyKey { get; set; }
    [JsonInclude, JsonPropertyName("startStopHotkeyModifierKeys")]
    internal ModifierKeys StartStopHotkeyModifierKeys { get; set; }
    [JsonInclude, JsonPropertyName("isStartStopHotkeyEnabled")]
    internal bool IsStartStopHotkeyEnabled { get; set; }
}