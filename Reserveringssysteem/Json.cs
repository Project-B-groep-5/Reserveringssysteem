using System.IO;
using System.Text.Json;

namespace Reserveringssysteem
{
    public static class Json
    {
        public static void JsonSerialize(object[] obj, string filename)
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            var jsonString = JsonSerializer.Serialize(obj, options);
            File.WriteAllText(filename, jsonString);
        }
    }
}
