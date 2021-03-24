using System;
using System.IO;
using System.Text.Json;

namespace Reserveringssysteem
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            JsonSerialize(new Tafel[] {new Tafel(1, 6, new Customer("Oscar"), new Bill(0.00)), new Tafel(2, 4, new Customer("Arjen"), new Bill(100.0)) }, "tafels.json");
        }

        static void JsonSerialize(Object[] obj, string filename)
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
