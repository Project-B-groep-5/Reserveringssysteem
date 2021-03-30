using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Reserveringssysteem
{
    public static class Json
    {
        private static readonly JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        public static void Serialize<T>(T obj, string filename) => File.WriteAllText(filename, JsonSerializer.Serialize<T>(obj, options));

        public static T Deserialize<T>(string filename) => JsonSerializer.Deserialize<T>(File.ReadAllText(filename), options);
    }
}
