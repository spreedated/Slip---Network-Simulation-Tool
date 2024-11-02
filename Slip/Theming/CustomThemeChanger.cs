using System.Runtime.Versioning;

namespace Slip.Theming
{
    public class CustomThemeChanger : IThemeChanger
    {
        private readonly byte red, green, blue;

        public CustomThemeChanger(byte red, byte green, byte blue)
        {
            this.red = red;
            this.green = green;
            this.blue = blue;
        }

        [SupportedOSPlatform("windows7.0")]
        public void ChangeTheme()
        {
            ThemeHelper.SetTheme(red, green, blue);
        }
    }
}