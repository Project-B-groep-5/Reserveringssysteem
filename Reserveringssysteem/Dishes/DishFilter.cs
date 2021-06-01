using System;
using System.Linq;
using static Reserveringssysteem.Json;

namespace Reserveringssysteem
{
    public class DishFilter
    {
        private static void Search(string keyWord)
        {
            int count = 0;
            foreach (Dish dish in DishList)
            {
                bool printDish = false;
                if (!(dish.Name.ToLower().Contains(keyWord.ToLower()) || dish.Type.ToLower().Contains(keyWord.ToLower())))
                {
                    foreach (var tag in dish.Tags)
                        if (tag.ToLower().Contains(keyWord.ToLower()))
                        { 
                            printDish = true; 
                            break; 
                        }
                    if (!printDish)
                        foreach (var ingredient in dish.Ingredients)
                            if (ingredient.ToLower().Contains(keyWord.ToLower()))
                            {
                                printDish = true;
                                break;
                            }   
                }
                else
                    printDish = true;
                
                if (printDish)
                {
                    printDishProperty("\nNaam: ", dish.Name);
                    printDishProperty("Prijs: ", $"€{dish.Price:0.00}");
                    if (dish.Ingredients.Count > 0)
                        printDishProperty("Ingredienten: ", string.Join(", ", dish.Ingredients));
                    printDishProperty("Type: ", dish.Type);
                    if (dish.Allergie.Length > 0)
                        printDishProperty("Allergieën: ", string.Join(", ", dish.Allergie));
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("__________________________________________________________");
                    count++;
                }
            }

            if (count < 1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nNiks gevonden.");
            }

            static void printDishProperty(string propertyName, string propertyValue)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(propertyName);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(propertyValue);
            }
        }

        public static void Search()
        {
            Logo.PrintLogo(Logo.MenuKaart);
            string keyWord = Utils.Input("Voer een term in:");
            Search(keyWord);
            Utils.Enter(Program.DishMenu);
        }
        public static void SecondMenuScreen()
        {
            var categories = new[] { "Voorgerechten", "Hoofdgerechten", "Nagerechten", "Dranken" };
            var titles = categories.ToList<string>();
            titles[^1] += " \n";
            titles.Add("Terug");
            var menu = new SelectionMenu(titles.ToArray(), Logo.MenuKaart);
            var choice = menu.Show();
            if (choice == titles.Count -1) Program.DishMenu();
            else
            {
                Logo.PrintLogo(Logo.MenuKaart);
                Search(categories[choice]);
                Utils.Enter(SecondMenuScreen);
            }
        }
        public static void ThirdMenuScreen()
        {
            var categories = new[] { "Vegetarisch", "Veganistisch", "Glutenvrij", "Lactosevrij" };
            var titles = categories.ToList<string>();
            titles[^1] += " \n";
            titles.Add("Terug");
            var menu = new SelectionMenu(titles.ToArray(), Logo.MenuKaart);
            var choice = menu.Show();
            if (choice == titles.Count - 1) Program.DishMenu();
            else
            {
                Logo.PrintLogo(Logo.MenuKaart);
                Search(categories[choice]);
                Utils.Enter(ThirdMenuScreen);
            }
        }
    }
}
