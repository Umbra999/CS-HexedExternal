using System.Diagnostics;

namespace Hexed.Wrappers
{
    internal class GameHelper
    {
        public static Process GetGameProcess()
        {
            Process[] processList = Process.GetProcessesByName("csgo");
            if (processList != null && processList.Length > 0)
            {
                var matchingProcesses = processList.Where(p => p.Threads.Count > 0);
                if (matchingProcesses != null && matchingProcesses.ToArray().Length > 0) return matchingProcesses.First();
            }

            return null;
        }

        public static bool IsModuleLoaded(Process p, string moduleName)
        {
            var q = from m in p.Modules.OfType<ProcessModule>() select m;

            return q.Any(pm => pm.ModuleName == moduleName && (int)pm.BaseAddress != 0);
        }

        public static bool IsKeyDown(int key)
        {
            return (NativeMethods.GetAsyncKeyState(key) & 0x8000) != 0;
        }
    }
}
