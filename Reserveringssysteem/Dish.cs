using System;
using System.Collections.Generic;
using System.Text;

namespace Reserveringssysteem
{
    public class Dish
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public string[] Ingredients { get; set; }
        public string Type { get; set; }
        public string[] Tags { get; set; }

        public Dish(string name, double price, string[] ingredients, string type, string[] tags)
        {
            Name = name;
            Price = price;
            Ingredients = ingredients;
            Type = type;
            Tags = tags;
        }
    }
}
