using System;
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

        public static void Serialize(object[] obj, string filename)
        {
            var jsonString = JsonSerializer.Serialize(obj, options);
            File.WriteAllText(filename, jsonString);
        }

        public static Table[] DeserializeTable()
        {
            var json = File.ReadAllText("tables.json");
            using (JsonDocument document = JsonDocument.Parse(json))
            {
                JsonElement tafelArrayElement = document.RootElement;
                var Arr = new Table[tafelArrayElement.GetArrayLength()];
                var i = 0;
                foreach (var tafel in tafelArrayElement.EnumerateArray())
                {
                    if (tafel.TryGetProperty("TableId", out JsonElement tableIdElement) &&
                        tafel.TryGetProperty("Size", out JsonElement sizeElement) &&
                        tafel.TryGetProperty("Customer", out JsonElement customerElement) &&
                            customerElement.TryGetProperty("Name", out JsonElement nameElement) &&
                            customerElement.TryGetProperty("Id", out JsonElement idElement) &&
                        tafel.TryGetProperty("Bill", out JsonElement billElement) &&
                            billElement.TryGetProperty("ToPay", out JsonElement toPayElement))
                    {
                        string tableId = tableIdElement.GetString();
                        int size = sizeElement.GetInt32();
                        Customer customer = new Customer(nameElement.GetString(), idElement.GetString());
                        Bill bill = new Bill(toPayElement.GetDouble());
                        Arr[i++] = new Table(tableId, size, customer, bill);
                    }
                }
                return Arr;
            }
        }
    }
}
