using System.Collections.Generic;

namespace Reserveringssysteem
{
    public class Dish
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public List<string> Ingredients { get; set; }
        public string Type { get; set; }
        public string[] Tags { get; set; }

        public string Allergie { get; set; }

        public Dish(string name, double price, string[] ingredients, string type, string[] tags, string allergie)
        {
            Name = name;
            Price = price;
            Ingredients = ingredients;
            Type = type;
            Tags = tags;
            Allergie = allergie;
        }
    }
}
