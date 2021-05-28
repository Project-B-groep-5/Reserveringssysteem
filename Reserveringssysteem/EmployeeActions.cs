using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using static Reserveringssysteem.Json;

namespace Reserveringssysteem
{
    class EmployeeActions
    {
        public static void MainMenu()
        {
            SelectionMenu.Make(new[] { "Reserveringen weergeven", "Gegevens wijzigen \n", "Terug" }, new Action[] { OverviewReservations.Overview, ChangeMenu, Program.Main }, Logo.Dashboard);
        }
        public static void ChangeMenu()
        {
            SelectionMenu.Make(new[] { "Restaurant gegevens", "Tafels", "Gerechten / Dranken", "Menus", "Wachtwoord \n", "Terug" }, new Action[] { ChangeRestaurantInfo, Table.TableManager, ChangeDish, ChangeMenus, ChangePassword, MainMenu }, Logo.Dashboard, "\nKies een optie om te wijzigen\n");
        }
        private static string _category;
        private static Dish _dish;

        private static void ChangeDish()
        {
            SelectCategory();
        }
        private static void SelectCategory()
        {
            var categories = new[] { "Voorgerechten", "Hoofdgerechten", "Nagerechten", "Koude dranken", "Warme dranken", "Alcoholische dranken" };
            var menu = new SelectionMenu(new[] { "Voorgerechten", "Hoofdgerechten", "Nagerechten", "Koude dranken", "Warme dranken", "Alcoholische dranken \n", "Terug" }, Logo.Dashboard);
            var option = menu.Show();
            if (option < categories.Length)
            {
                _category = categories[option];
                SelectDish();
            }
            else
                ChangeMenu();
        }
        private static void SelectDish()
        {
            var dishes = new List<Dish>();
            var dishNames = new List<string>
                {
                    "Toevoegen \n"
                };
            foreach (var dish in DishList)
            {
                if (dish.Type == _category)
                {
                    dishes.Add(dish);
                    dishNames.Add(dish.Name);
                }
            }
            dishNames[^1] = dishNames[^1] + " \n";
            dishNames.Add("Terug");
            var dishMenu = new SelectionMenu(dishNames.ToArray(), Logo.Dashboard);
            var option = dishMenu.Show();
            if (option < dishNames.Count - 1 && option > 0)
            {
                _dish = dishes[option - 1];
                SelectProperty();
            }
            else if (option == 0)
            {
                AddDish();
            }
            else
                SelectCategory();
        }
        private static void SelectProperty()
        {
            var choices = new List<string>
                {
                    "Verwijderen \n",
                    "Naam",
                    "Prijs"
                };
            if (_category.Contains("gerecht"))
                choices.Add("Ingredienten");
            choices[^1] += " \n";
            choices.Add("Terug");

            var choiceMenu = new SelectionMenu(choices.ToArray(), Logo.Dashboard);
            switch (choices[choiceMenu.Show()])
            {
                case "Naam":
                    Logo.PrintLogo(Logo.Dashboard);
                    var naam = _dish.Name;
                    Console.WriteLine($"De nieuwe naam voor {naam} is: \n");
                    _dish.Name = Console.ReadLine();
                    Console.WriteLine($"{naam} veranderd in {_dish.Name}");
                    break;
                case "Prijs":
                case "Prijs \n":
                    var prijs = _dish.Price;
                    while (true)
                    {
                        string input = "";
                        try
                        {
                            Logo.PrintLogo(Logo.Dashboard);
                            Console.WriteLine($"De nieuwe prijs voor {_dish.Name} is: \n");
                            input = Console.ReadLine();
                            _dish.Price = double.Parse(input);
                            Console.WriteLine($"Prijs voor {_dish.Name} veranderd van {prijs} naar {_dish.Price}");
                            break;
                        }
                        catch
                        {
                            Logo.PrintLogo(Logo.Dashboard);
                            Console.WriteLine($"{input} is geen correcte waarde voor een prijs.\nPrijzen zijn bijvoorbeeld genoteerd als volgt: 19.5 of 19");
                            Utils.Enter("om opnieuw te proberen");
                        }
                    }
                    break;
                case "Ingredienten":
                case "Ingredienten \n":
                    ChangeIngredients();
                    break;
                case "Verwijderen \n":
                    DishList.Remove(_dish);
                    Console.WriteLine($"{_dish.Name} verwijderd.");
                    break;
                default:
                    SelectDish();
                    return;
            }
            Serialize(DishList, "dishes.json");
            Utils.Enter(ChangeDish);
        }
        private static void ChangeIngredients()
        {
            Logo.PrintLogo(Logo.Dashboard);
            var choices = new List<string> { "Toevoegen \n" };
            foreach (var ingredient in _dish.Ingredients)
                choices.Add(ingredient);
            choices[^1] += " \n";
            choices.Add("Verwijderen");
        }
        private static void AddDish()
        {
            Logo.PrintLogo(Logo.Dashboard);
            Console.WriteLine($"Wat wordt de naam voor dit item?\n");
            var naam = Console.ReadLine();
            double prijs;
            while (true)
            {
                string input = "";
                try
                {
                    Logo.PrintLogo(Logo.Dashboard);
                    Console.WriteLine($"Wat wordt de prijs voor {naam}?\n");
                    input = Console.ReadLine();
                    prijs = double.Parse(input);
                    break;
                }
                catch
                {
                    Logo.PrintLogo(Logo.Dashboard);
                    Console.WriteLine($"{input} is geen correcte waarde voor een prijs.\nPrijzen zijn bijvoorbeeld genoteerd als volgt: 19.5 of 19");
                    Utils.Enter("om opnieuw te proberen");
                }
            }
            DishList.Add(new Dish(naam, prijs, null, _category, null));
            Serialize(DishList, "dishes.json");
            Console.WriteLine($"{naam} toegevoegd aan {_category}.");
        }
        private static void ChangeMenus()
        {

        }
        private static void ChangeRestaurantInfo()
        {
            SelectionMenu.Make(new[] { "Naam van het restaurant", "Beschrijving", "Adress", "Capaciteit", "Openingstijden", "Contactinformatie\n", "Terug" }, new Action[] { ChangeRestaurantName, ChangeRestaurantDescription, ChangeRestaurantAddress, ChangeRestaurantCapacity, ChangeRestaurantHours, ChangeRestaurantContactInfo, ChangeMenu }, Logo.RestaurantGegevens, "\nKies een optie om te wijzigen\n");
        }

        private static void ChangeRestaurantName()
        {
            string newName = ChangeInfoText("Voer de nieuwe naam in voor het restaurant.");
            if (newName == "") ChangeRestaurantInfo();
            Console.Clear();
            SelectionMenu.Make(new[] { "Ja", "Nee" }, new Action[] { null, ChangeRestaurantName }, Logo.RestaurantGegevens, $"\nWeet u zeker dat u de naam van het restaurant naar \"{newName}\" wil veranderen?\n");
            Json.Restaurant.Name = newName;
            Serialize(Json.Restaurant, "restaurant.json");
            ChangeInfoSucces($"Naam succesvol is veranderd naar \"{newName}\"");
        }

        private static void ChangeRestaurantDescription()
        {
            string newDescription = ChangeInfoText("Voer de nieuwe beschrijving van het restaurant in.");
            if (newDescription == "") ChangeRestaurantInfo();
            Console.Clear();
            SelectionMenu.Make(new[] { "Ja", "Nee" }, new Action[] { null, ChangeRestaurantDescription }, Logo.RestaurantGegevens, $"\nDe nieuwe beschrijving van het restaurant wordt:\n\n{newDescription}\n\nWilt u deze verandering toepassen?\n");
            Json.Restaurant.Description = newDescription;
            Serialize(Json.Restaurant, "restaurant.json");
            ChangeInfoSucces("Beschrijving is succesvol veranderd.");
        }

        private static void ChangeRestaurantAddress()
        {

        }

        private static void ChangeRestaurantCapacity()
        {
            /*            try 
                        {	
                            int newCapacity = int.Parse(ChangeInfoText("Voer de nieuwe capaciteit van het restaurant in."));
                        }
                        catch
                        {
                            Console.WriteLine("De ingevoerde waarde is geen getal. Probeer het opnieuw.");
                            ChangeRestaurantCapacity();
                        }*/
        }

        private static void ChangeRestaurantHours()
        {
            var allDays = new[] { "Maandag", "Dinsdag", "Woensdag", "Donderdag", "Vrijdag", "Zaterdag", "Zondag\n", "Terug" };
            var day = new SelectionMenu(allDays, Logo.RestaurantGegevens, "\nKies van welke dag u de openingstijden wil aanpassen.\n");
            var chosenDay = day.Show();
            if (allDays[chosenDay] == "Terug") ChangeRestaurantInfo();
            Console.Clear();

            var closedOrNot = new[] { "Ja", "Nee" };
            var closeCheck = new SelectionMenu(closedOrNot, Logo.RestaurantGegevens, "\nWilt u dat deze dag gesloten is?\n");
            var chosen = closeCheck.Show();
            if (closedOrNot[chosen] == "Ja")
            {
                SelectionMenu.Make(new[] { "Ja", "Nee" }, new Action[] { null, ChangeRestaurantHours }, Logo.RestaurantGegevens ,$"\nWeet u zeker dat u {allDays[chosenDay].ToLower()} op gesloten wil zetten?\n");
                Json.Restaurant.OpeningHours[chosenDay] = "Gesloten";
                Serialize(Json.Restaurant, "restaurant.json");
                ChangeInfoSucces($"{allDays[chosenDay]} is succesvol veranderd naar 'Gesloten'.");
            }

            TimeSpan check;
            Console.CursorVisible = true;
            string newOpeningHour = ChangeInfoText("Voer de openingstijd in.\nHet format is '00:00'\nVoorbeeld: 17:00");
            while (true)
            {
                if (newOpeningHour == "") ChangeRestaurantHours();
                else if (TimeSpan.TryParse(newOpeningHour, out check)) break;
                Logo.PrintLogo(Logo.RestaurantGegevens);
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine("Ingevoerde waarde is niet in het correcte format of het is geen echte tijd.\nVoer opnieuw de openingstijd in.\nHet format is '00:00'.\nVoorbeeld: 17:00\nOf druk op 'enter' om terug te gaan\n");
                Console.ForegroundColor = ConsoleColor.White;
                Console.CursorVisible = true;
                newOpeningHour = Console.ReadLine();
            }
            TimeSpan check2;
            string newClosingHour = ChangeInfoText("Voer de sluitingstijd in.\nHet format is '00:00'\nVoorbeeld: 22:00");
            while (true)
            {
                if (newClosingHour == "") ChangeRestaurantInfo();
                else if (TimeSpan.TryParse(newClosingHour, out check2)) break;
                Logo.PrintLogo(Logo.RestaurantGegevens);
                Logo.PrintLogo(Logo.RestaurantGegevens);
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine("Ingevoerde waarde is niet in het correcte format of het is geen echte tijd.\nVoer opnieuw de sluitingstijd in.\nHet format is '00:00'.\nVoorbeeld: 22:00\nOf druk op 'enter' om terug te gaan\n");
                Console.ForegroundColor = ConsoleColor.White;
                Console.CursorVisible = true;
                newClosingHour = Console.ReadLine();
            }
            SelectionMenu.Make(new[] { "Ja", "Nee" }, new Action[] { null, ChangeRestaurantHours }, Logo.RestaurantGegevens, $"De nieuwe openings- en sluitingstijden op {allDays[chosenDay].ToLower()} worden:\n{newOpeningHour}-{newClosingHour}\nWilt u deze verandering toepassen?\n");
            Json.Restaurant.OpeningHours[chosenDay] = $"{newOpeningHour}-{newClosingHour}";
            Serialize(Json.Restaurant, "restaurant.json");
            ChangeInfoSucces("Openings- en sluitings tijden zijn veranderd.");
        }

        private static void ChangeRestaurantContactInfo()
        {

        }

        private static string ChangeInfoText(string text)
        {
            Logo.PrintLogo(Logo.RestaurantGegevens);
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine(text + "\nOf druk op 'enter' om terug te gaan.\n");
            Console.CursorVisible = true;
            Console.ForegroundColor = ConsoleColor.White;
            return Console.ReadLine();
        }
        private static void ChangeInfoSucces(string text)
        {
            Logo.PrintLogo(Logo.RestaurantGegevens);
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine(text + "\nDruk op 'enter' om door te gaan");
            Console.Read();
            ChangeRestaurantInfo();
        }

        public static void ChangePassword()
        {
            string password;
            while (true)
            {
                Logo.PrintLogo(Logo.Wachtwoord);
                Console.CursorVisible = true;
                Console.WriteLine("Nieuw wachtwoord:");
                var newPassword = Console.ReadLine();
                Logo.PrintLogo(Logo.Wachtwoord);
                Console.WriteLine("Herhaal het nieuwe wachtwoord:");
                if (Console.ReadLine() == newPassword)
                {
                    password = newPassword;
                    break;
                }
                else
                {
                    Logo.PrintLogo(Logo.Wachtwoord);
                    Console.WriteLine("De ingevoerde wachtwoorden komen niet overeen.");
                    Utils.Enter("om opnieuw te proberen");
                }
            }
            File.WriteAllText("password.txt", password);
        }
    }
}
