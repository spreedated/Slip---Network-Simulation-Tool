using System.Runtime.InteropServices;
using System.Windows;

// A namespace can only contain type declarations such as classes, structs, interfaces, and enums.
namespace Slip
{
    internal static partial class WinDivert
    {
        /*
         * In this case, since fields are already within an internal class,
         * it is already accessible within the same assembly and adding the
         * internal keyword is redundant.
         */
        [LibraryImport("kernel32.dll", EntryPoint = "SetDllDirectoryW", StringMarshalling = StringMarshalling.Utf16)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static partial bool SetDllDirectory(string lpPathName);

        [LibraryImport("WinDivert.dll", EntryPoint = "WinDivertHelperCompileFilter", SetLastError = true, StringMarshalling = StringMarshalling.Custom, StringMarshallingCustomType = typeof(System.Runtime.InteropServices.Marshalling.AnsiStringMarshaller))]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool WinDivertHelperCompileFilter([MarshalAs(UnmanagedType.LPStr)] string filter, WindivertLayer layer, nint obj, uint objLen, out nint errorStr, out uint errorPos);

        internal enum WindivertLayer
        {
            WindivertLayerNetwork = 0,
            WindivertLayerNetworkForward = 1
        }

        internal static bool CheckFilter(string filter)
        {
            WindivertLayer layer = WindivertLayer.WindivertLayerNetwork;
            nint obj = nint.Zero;
            uint objLen = 0;
            nint errorStr;
            uint errorPos;

            bool success = WinDivertHelperCompileFilter(filter, layer, obj, objLen, out errorStr, out errorPos);

            if (!success)
            {
                string errorString = Marshal.PtrToStringAnsi(errorStr);
                MessageBox.Show(errorString);
            }

            return success;
        }
    }
}