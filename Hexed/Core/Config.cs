using Hexed.Wrappers;

namespace Hexed.Core
{
    internal class Config
    {
        public static IniFile Ini;

        public static bool ESP = true;
        public static bool BHop = true;
        public static bool Radar = true;

        public static void LoadConfig()
        {
            if (!File.Exists("Config.ini")) File.Create("Config.ini");

            Ini = new IniFile("Config.ini");

            //ESP = Ini.GetBool("Modules", "ESP");
            //BHop = Ini.GetBool("Modules", "BHop");
            //Radar = Ini.GetBool("Modules", "Radar");
        }
    }
}
