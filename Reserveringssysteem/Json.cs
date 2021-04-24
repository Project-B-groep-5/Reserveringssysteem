using System.IO;
using Newtonsoft.Json;

namespace Reserveringssysteem
{
    public static class Json
    {
        public static void Serialize(object obj, string filename) => File.WriteAllText(filename, JsonConvert.SerializeObject(obj));

        public static T Deserialize<T>(string filename) => JsonConvert.DeserializeObject<T>(File.ReadAllText(filename));
    }
}
