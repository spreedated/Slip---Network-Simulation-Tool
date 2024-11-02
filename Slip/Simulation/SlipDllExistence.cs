using Slip.Interfaces;
using System.IO;

namespace Slip.Simulation
{
    public class SlipDllExistence : IFileExistence
    {
        private readonly string dllPath;

        public SlipDllExistence(string dllPath)
        {
            this.dllPath = dllPath;
        }

        public bool CheckFileExistence()
        {
            return File.Exists(dllPath);
        }
    }
}