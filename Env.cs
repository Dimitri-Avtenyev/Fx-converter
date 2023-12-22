namespace Fx_converter
{
    public static class Env
    {
        public static void Load(string path) {
            if(!File.Exists(path)) {
                return;
            }
            foreach (var line in File.ReadAllLines(path)) {
                var lines = line.Split("=", StringSplitOptions.RemoveEmptyEntries);
                if (lines.Length != 2) {
                    continue;
                }
                Environment.SetEnvironmentVariable(lines[0], lines[1]);
            }
        }
    }
}
