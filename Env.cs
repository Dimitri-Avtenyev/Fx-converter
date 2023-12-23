namespace Fx_converter
{
    public static class Env
    {
        public static void Load(string path) {
            if (!File.Exists(path)) {
                return;
            }
            foreach (var line in File.ReadAllLines(path)) {
                var results = line.Split("=", StringSplitOptions.RemoveEmptyEntries);
                if (results.Length != 2) {
                    continue;
                }
                Environment.SetEnvironmentVariable(results[0], results[1]);
            }

        }
    }
}
