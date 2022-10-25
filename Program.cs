using System.Diagnostics;
using System.Text.Json;

namespace AVRDudeWrapper
{
    class Program
    {
        private static IEnumerable<Preset>? _presets = new List<Preset>();

        static void Main(string[] args)
        {
            if (!ParseArguments(args, out string sourceFilename, out string? configFilename)) return;
            if (!LoadPresets(configFilename)) return;

            Preset? preset = null;

            Console.WriteLine("Enter preset name:");
            string presetName = Console.ReadLine();
            preset = _presets.FirstOrDefault(x => x.Name == presetName);

            if (preset == null) preset = _presets.OrderByDescending(x => x.LastUsed).First();
            else
            {
                Console.WriteLine("Enter port name:");

                string? portName = Console.ReadLine();
                if (portName is null or "") preset.Port = portName;
            }
            
            RunAVRDude(preset, sourceFilename);
            SavePresets(configFilename);
            
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        private static bool ParseArguments(string[] args, out string sourceFilename, out string? configFilename)
        {
            sourceFilename = "";
            configFilename = $"{Directory.GetCurrentDirectory()}\\presets.json";

            if (args.Length == 0)
            {
                PrintError("Too few arguments!");
                return false;
            }
            if(args.Length > 2)
            { 
                PrintError("Too many arguments!");
                return false;
            }

            sourceFilename = args[0];
            if (args.Length == 2 && args[1] != "") configFilename = args[1];

            return true;
        }

        private static void RunAVRDude(Preset preset, string filename)
        {
            preset.LastUsed = _presets.Max(x => x.LastUsed) + 1;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("avrdude.exe says:");
            Console.ResetColor();
            Process.Start("avrdude", new []{$"-C{GetConfigPath()}\\avrdude.conf", "-v", $"-p{preset.MCUName}", $"-c{preset.Programmer}", $"-P{preset.Port}", $"-b{preset.BaudRate}", "-D", $"-Uflash:w:{filename}:a"}).WaitForExit();
        }

        private static string GetConfigPath()
        {
            string[] path = Environment.GetEnvironmentVariable("PATH").Split(';');
            return path.FirstOrDefault(x => x.Contains("avrdude"));
        }
        
        private static void PrintError(string error)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(error);
            Console.ResetColor();
        }
        
        private static bool LoadPresets(string filename)
        {
            if (!File.Exists(filename))
            {
                PrintError($"File '{filename}' does not exist!");
                return false;
            }
            
            using (FileStream jsonStream = new FileStream(filename, FileMode.Open))
                _presets = (IEnumerable<Preset>)JsonSerializer.Deserialize(jsonStream, _presets.GetType());

            return true;
        }

        private static void SavePresets(string filename)
        {
            using (FileStream jsonStream = new FileStream(filename, FileMode.Truncate))
                JsonSerializer.Serialize(jsonStream, _presets, _presets.GetType());
        }
    }

    class Preset
    {
        public string Name { get; set; }
        public string MCUName { get; set; }
        public string Programmer { get; set; }
        public int BaudRate { get; set; }
        public int LastUsed { get; set; }
        public string Port { get; set; }

        public Preset(string name, string mcuName, string programmer, int baudRate, int lastUsed, string port)
        {
            Name = name;
            MCUName = mcuName;
            Programmer = programmer;
            BaudRate = baudRate;
            LastUsed = lastUsed;
            Port = port;
        }
    }
}