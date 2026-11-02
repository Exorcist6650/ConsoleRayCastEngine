
using System.Text.Json;

namespace ConsoleRaycastEngine
{
    public static class ConfigurationReader
    {
        private const string ConfigPath = "config.json";
        public static string ReadValue(string value)
        {
            // Read as file
            var json = File.ReadAllText(ConfigPath);

            // Open json document
            using var document = JsonDocument.Parse(json);

            var root = document.RootElement;

            if (!root.TryGetProperty(value, out var output))
                throw new NullReferenceException("Parametr is not exist");

            return output.ToString();
        }

    };
}
