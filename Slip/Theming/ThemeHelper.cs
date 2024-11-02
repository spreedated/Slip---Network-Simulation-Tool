using MaterialDesignThemes.Wpf;
using System.Runtime.Versioning;
using System.Windows.Media;

namespace Slip.Theming
{
    public static class ThemeHelper
    {
        [SupportedOSPlatform("windows7.0")]
        internal static void SetTheme(byte red, byte green, byte blue)
        {
            // Set the primary and secondary colors of the theme to a custom color
            Color customColor = Color.FromRgb(red, green, blue);

            // Create a helper object for managing the theme
            PaletteHelper themeManager = new();

            // Get the current theme
            ITheme theme = themeManager.GetTheme();

            // Set both the primary and secondary colors of the theme to the custom color
            theme.SetPrimaryColor(customColor);
            theme.SetSecondaryColor(customColor);

            // Update the theme with the new primary and secondary colors
            themeManager.SetTheme(theme);
        }
    }
}