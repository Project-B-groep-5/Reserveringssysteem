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
            SelectionMenu.Make(new[] { "Restaurant gegevens", "Tafels", "Gerechten / Dranken", "Voordeelmenus", "Wachtwoord \n", "Terug" }, new Action[] { ChangeRestaurantInfo, Table.TableManager, ChangeDish, ChangeMenus, ChangePassword, MainMenu }, Logo.Dashboard, "\nKies een optie om te wijzigen\n");
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
					"Naam",
					"Prijs"
				};
			if (_category.Contains("gerecht"))
				choices.Add("Ingredienten");
			choices[^1] += " \n";
			choices.Add("Verwijderen \n");
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
		private static string _ingredient;
        private static void ChangeIngredients()
        {
			Logo.PrintLogo(Logo.Dashboard);
			var choices = new List<string> { "Toevoegen \n" };
			foreach (var ingredient in _dish.Ingredients)
				choices.Add(ingredient);
			choices[^1] += " \n";
			choices.Add("Terug");
			var menu = new SelectionMenu(choices.ToArray(), Logo.Dashboard);
			var choice = menu.Show();
			if (choice == 0)
				AddIngredient();
			else if (choices[choice] == choices[^1])
				SelectProperty();
			else
            {
                _ingredient = choices[choice - 1];
                ChangeIngredientMenu();
            }
        }

        private static void ChangeIngredientMenu()
        {
            SelectionMenu.Make(new[] { "Wijzigen", "Verwijderen \n", "Terug" }, new Action[] { ChangeIngredient, RemoveIngredient, ChangeIngredients }, Logo.Dashboard);
        }

        private static void RemoveIngredient()
        {
			_dish.Ingredients.Remove(_ingredient);
			Serialize(DishList, "dishes.json");
			Logo.PrintLogo(Logo.Dashboard);
            Console.WriteLine($"{_ingredient} verwijdert.");
			Utils.Enter(ChangeIngredients);
		}

        private static void ChangeIngredient()
        {
            Console.WriteLine($"Waar wilt u {_ingredient} in veranderen?\nVoer de nieuwe naam in:");
			var name = Console.ReadLine();
			if (name == "")
			{
				SelectionMenu.Make(new[] { "Opnieuw proberen", "Terug" }, new Action[] { ChangeIngredient, ChangeIngredientMenu }, Logo.Dashboard, "U heeft niks ingevuld.\n\nMaak een keuze:\n");
				return;
			}
			if (Utils.Confirm(Logo.Dashboard, $"Weet u zeker dat u een ingrediënt met de naam: {name} wilt toevoegen?\n") == 0)
			{
				var ingredientInt = _dish.Ingredients.IndexOf(_ingredient);
				_dish.Ingredients[ingredientInt] = name;
				Serialize(DishList, "dishes.json");
				Logo.PrintLogo(Logo.Dashboard);
                Console.WriteLine($"{_ingredient} veranderd naar {name}.");
				Utils.Enter(ChangeIngredients);
				return;
			}
			ChangeIngredient();
		}

        private static void AddIngredient()
        {
            Console.WriteLine("Voer de naam van het ingrediënt in:");
			var name = Console.ReadLine();
			if (name == "")
			{
				SelectionMenu.Make(new[] { "Opnieuw proberen", "Terug" }, new Action[] { AddIngredient, ChangeIngredients }, Logo.Dashboard, "U heeft niks ingevuld.\n\nMaak een keuze:\n");
				return;
			}
			if (Utils.Confirm(Logo.Dashboard, $"Weet u zeker dat een ingrediënt met de naam: {name} wilt toevoegen?\n") == 0)
			{
				_dish.Ingredients.Add(name);
				Serialize(DishList, "dishes.json");
				Logo.PrintLogo(Logo.Dashboard);
                Console.WriteLine($"{name} toegevoegd.");
				Utils.Enter(ChangeIngredients);
				return;
			}
			AddIngredient();
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
        private static void ChangeMenus() // Om een voordeelmenu te veranderen / toe te voegen.
        {
            var choices = new string[VoordeelMenus.Count + 2]; 
            choices[0] = "Toevoegen \n";
            for (int i = 1; i < VoordeelMenus.Count + 1; i++) // Maakt array met alle namen van de voordeelmenus
            {
                choices[i] = VoordeelMenus[i - 1].Name;
                if (i == choices.Length - 2) choices[i] = choices[i] + " \n"; // 1 na laatste element met '\n' voor de terug knop.
            }
            choices[choices.Length - 1] = "Terug"; 

            var menusMenu = new SelectionMenu(choices, Logo.GerechtenMenus);
            var chosen = menusMenu.Show();

            if (chosen == choices.Length - 1) ChangeMenu(); // Gaat terug 
            else if (chosen == 0) AddMenu(); // Voegt een voordeelmenu toe
            else ChangeSpecificMenu(chosen); // Om een voordeelmenu aan te passen / te verwijderen.

        }

        private static void ChangeSpecificMenu(int chosenMenu)
        {
            string[] choices = new[] { "Naam", "Voorgerecht", "Hoofdgerecht", "Nagerecht", "Prijs \n", "Verwijderen \n", "Terug" };
            SelectionMenu menusMenu = new SelectionMenu(choices, Logo.GerechtenMenus, "\nKies wat u wilt aanpassen.\n");
            int chosen = menusMenu.Show();
            if (chosen == choices.Length - 1) ChangeMenus(); // Terug optie
            else if (chosen == choices.Length - 2) DeleteMenu(chosenMenu); // Bij optie verwijderen wordt deze gecalled.

            else if (chosen == 0) // Naam veranderen
            {
                string newName = ChangeInfoText("Voer de nieuwe naam in van het voordeelmenu.", Logo.GerechtenMenus);
                if (newName == "") ChangeSpecificMenu(chosenMenu);
                SelectionMenu.Make(new[] { "Ja", "Nee" }, new Action[] { null, ChangeMenus }, Logo.RestaurantGegevens, $"\nWeet u zeker dat u de naam van het voordeelmenu naar \"{newName}\" wilt veranderen?\n");
                VoordeelMenus[chosenMenu - 1].Name = newName; // -1 omdat de eerste optie "toevoegen" was.
            }
            else if (chosen == 1) // Voorgerecht veranderen
            {
                Dish newVoorGerecht = MakeDishMenu("Voorgerechten", ChangeMenus);
                SelectionMenu.Make(new[] { "Ja", "Nee" }, new Action[] { null, ChangeMenus }, Logo.RestaurantGegevens, $"\nWeet u zeker dat u de het voorgerecht van het voordeelmenu naar \"{newVoorGerecht.Name}\" wilt veranderen?\n");
                VoordeelMenus[chosenMenu - 1].VoorGerecht = newVoorGerecht;
            }
            else if (chosen == 2) // Hoofdgerecht veranderen
            {
                Dish newHoofdGerecht = MakeDishMenu("Hoofdgerechten", ChangeMenus);
                SelectionMenu.Make(new[] { "Ja", "Nee" }, new Action[] { null, ChangeMenus }, Logo.RestaurantGegevens, $"\nWeet u zeker dat u de het hoofdgerecht van het voordeelmenu naar \"{newHoofdGerecht.Name}\" wilt veranderen?\n");
                VoordeelMenus[chosenMenu - 1].HoofdGerecht = newHoofdGerecht;
            }
            else if (chosen == 3) // Nagerecht veranderen
            {
                Dish newNaGerecht = MakeDishMenu("Nagerechten", ChangeMenus);
                SelectionMenu.Make(new[] { "Ja", "Nee" }, new Action[] { null, ChangeMenus }, Logo.RestaurantGegevens, $"\nWeet u zeker dat u de het hoofdgerecht van het voordeelmenu naar \"{newNaGerecht.Name}\" wilt veranderen?\n");
                VoordeelMenus[chosenMenu - 1].NaGerecht = newNaGerecht;
            }
            else // Veranderen van de prijs
            {
                double newPrijs = PriceChange(ChangeMenus);
                SelectionMenu.Make(new[] { "Ja", "Nee" }, new Action[] { null, ChangeMenus }, Logo.RestaurantGegevens, $"\nWeet u zeker dat u de prijs van het voordeelmenu naar {newPrijs.ToString("#.00")} euro wilt veranderen?\n");
                VoordeelMenus[chosenMenu - 1].Prijs = newPrijs;
            }
            Serialize(VoordeelMenus, "voordeelmenu.json");
            ChangeInfoSucces("Voordeelmenu is succesvol aangepast", ChangeMenus);
        }

        private static void DeleteMenu(int chosenMenu) // Functie om een voordeelmenu te verwijderen.
        {
            SelectionMenu.Make(new[] { "Ja", "Nee" }, new Action[] { null, ChangeMenus }, Logo.RestaurantGegevens, $"\nWeet u zeker dat u het voordeelmenu: \"{VoordeelMenus[chosenMenu - 1].Name}\", wilt verwijderen?\n"); // check
            VoordeelMenus.Remove(VoordeelMenus[chosenMenu - 1]); // -1 omdat de eerste keuze in de array de optie "toevoegen" was.
            Serialize(VoordeelMenus, "voordeelmenu.json");
            ChangeInfoSucces("Voordeelmenu is succesvol verwijdert", ChangeMenus);
        }

        private static void AddMenu() // Voegt een voordeelmenu toe
        {
            string newName = ChangeInfoText("Voer de naam in van het voordeelmenu.", Logo.GerechtenMenus);
            if (newName == "") ChangeMenus();
            Dish newVoorGerecht = MakeDishMenu("Voorgerechten", AddMenu);
            Dish newHoofdGerecht = MakeDishMenu("Hoofdgerechten", AddMenu);
            Dish newNaGerecht = MakeDishMenu("Nagerechten", AddMenu);
            double newPrijs = PriceChange(AddMenu);
            SelectionMenu.Make(new[] { "Ja", "Nee" }, new Action[] { null, AddMenu }, Logo.GerechtenMenus, $"\nNaam: {newName}\n" + 
                $"Voorgerecht: {newVoorGerecht.Name}\n" +
                $"Hoofdgerecht: {newHoofdGerecht.Name}\n" +
                $"Nagerecht: {newNaGerecht.Name}\n" +
                $"Prijs: {Math.Round(newPrijs, 2).ToString("#.00")}\n" +
                $"Het voordeelmenu wordt toegevoegd. Wilt u doorgaan?\n"); // Vraag voor een check.
            VoordeelMenus.Add(new VoordeelMenu(newName, newVoorGerecht, newHoofdGerecht, newNaGerecht, newPrijs));
            Serialize(VoordeelMenus, "voordeelmenu.json"); // Slaat het op.
            ChangeInfoSucces("Voordeelmenu is succesvol opgeslagen.", ChangeMenus);
        }

        private static double PriceChange(Action func)
        {
            double newPrijs;
            string newPrijsStr;
            newPrijsStr = ChangeInfoText("Voer de prijs in van het voordeelmenu.", Logo.GerechtenMenus);
            while (true)
            {
                try
                {
                    if (newPrijsStr == "") func();
                    newPrijs = double.Parse(newPrijsStr, 0.00);
                    break;
                }
                catch // Gecalled wanneer input geen prijs is en vraagt het opnieuw.
                {
                    Logo.PrintLogo(Logo.GerechtenMenus);
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    newPrijsStr = ChangeInfoText("Ingevoerde waarde is geen correcte waarde.\nVoer de prijs in van het voordeelmenu.", Logo.GerechtenMenus);
                }
            }
            return newPrijs;
        } 
        private static Dish MakeDishMenu(string dishType, Action func) // Maakt array met gegeven gerecht type en returned het gekozen gerecht.
        {
            List<string> allDishes = new List<string>();
            for (int i = 0; i < DishList.Count; i++)
            {
                if (DishList[i].Type == dishType)
                    allDishes.Add(DishList[i].Name);
            }
            allDishes[allDishes.Count - 1] = allDishes[allDishes.Count - 1] + " \n";
            allDishes.Add("Terug");
            string[] allDishesArray = allDishes.ToArray();
            var dishesMenu = new SelectionMenu(allDishesArray, Logo.GerechtenMenus, $"\nKies het {dishType.ToLower().Remove(dishType.Length - 2)}.\n");
            var chosen = dishesMenu.Show();
            if (chosen == allDishesArray.Length - 1) func();
            if (chosen == allDishesArray.Length - 2)
            {
                foreach (Dish dish in DishList)
                {
                    if (allDishesArray[chosen] == dish.Name + " \n")
                        return dish;
                }
            }
            else
            {
                foreach (Dish dish in DishList)
                {
                    if (allDishesArray[chosen] == dish.Name)
                        return dish;
                }
            }
            return null;
        }

        private static void ChangeRestaurantInfo()
        {
            SelectionMenu.Make(new[] { "Naam van het restaurant", "Beschrijving", "Adres", "Openingstijden", "Contactinformatie \n", "Terug" }, new Action[] { ChangeRestaurantName, ChangeRestaurantDescription, ChangeRestaurantAddress, ChangeRestaurantHours, ChangeRestaurantContactInfo, ChangeMenu }, Logo.RestaurantGegevens, "\nKies een optie om te wijzigen\n");
        }

        private static void ChangeRestaurantName() // Verandert naam van het restaurant.
        {
            string newName = ChangeInfoText("Voer de nieuwe naam in voor het restaurant.", Logo.RestaurantGegevens);
            if (newName == "") ChangeRestaurantInfo();
            Console.Clear();
            SelectionMenu.Make(new[] { "Ja", "Nee" }, new Action[] { null, ChangeRestaurantName }, Logo.RestaurantGegevens, $"\nWeet u zeker dat u de naam van het restaurant naar \"{newName}\" wil veranderen?\n"); // Check
            Json.Restaurant.Name = newName;
            Serialize(Json.Restaurant, "restaurant.json"); // Slaat het op.
            ChangeInfoSucces($"Naam succesvol is veranderd naar \"{newName}\"", ChangeRestaurantInfo);
        }

        private static void ChangeRestaurantDescription() // Verandert beschrijving van het restaurant.
        {
            string newDescription = ChangeInfoText("Voer de nieuwe beschrijving van het restaurant in.", Logo.RestaurantGegevens);
            if (newDescription == "") ChangeRestaurantInfo();
            Console.Clear();
            SelectionMenu.Make(new[] { "Ja", "Nee" }, new Action[] { null, ChangeRestaurantDescription }, Logo.RestaurantGegevens, $"\nDe nieuwe beschrijving van het restaurant wordt:\n\n{newDescription}\n\nWilt u deze verandering toepassen?\n");
            Json.Restaurant.Description = newDescription;
            Serialize(Json.Restaurant, "restaurant.json");
            ChangeInfoSucces("Beschrijving is succesvol veranderd.", ChangeRestaurantInfo);
        }

        private static void ChangeRestaurantAddress() // Verandert het adres. (kan maar 1 element veranderen per keer, maar anders zou je alles moeten veranderen elke keer.)
        {
            var choices = new[] { "Straatnaam", "Huisnummer", "Postcode", "Stad \n", "Terug" };
            var menu = new SelectionMenu(choices, Logo.RestaurantGegevens); // Wordt gevraagd wat veranderd moet worden.
            var chosen = menu.Show();

            if (choices[chosen] == "Terug") ChangeRestaurantInfo(); 
            else if (choices[chosen] == "Straatnaam")
            {
                string newStreetName = ChangeInfoText("Voer het nieuwe adres in.", Logo.RestaurantGegevens);
                if (newStreetName == "") ChangeRestaurantAddress();
                SelectionMenu.Make(new[] { "Ja", "Nee" }, new Action[] { null, ChangeRestaurantAddress }, Logo.RestaurantGegevens, $"\nWeet u zeker dat u de straatnaam wil vernaderen naar { newStreetName}?\n");
                Json.Restaurant.Address.StreetName = newStreetName;
                Serialize(Json.Restaurant, "restaurant.json");
            }
            else if (choices[chosen] == "Huisnummer")
            {
                string newHouseNumber = ChangeInfoText("Voer het nieuwe huisnummer in.", Logo.RestaurantGegevens);
                if (newHouseNumber == "") ChangeRestaurantAddress();
                SelectionMenu.Make(new[] { "Ja", "Nee" }, new Action[] { null, ChangeRestaurantAddress }, Logo.RestaurantGegevens, $"\nWeet u zeker dat u het huisnummer wil veranderen naar {newHouseNumber}?\n"); 
                Json.Restaurant.Address.HouseNumber = newHouseNumber;
                Serialize(Json.Restaurant, "restaurant.json");
            }
            else if (choices[chosen] == "Postcode")
            {
                string newPostalCode = ChangeInfoText("Voer de nieuwe postcode in.", Logo.RestaurantGegevens);
                if (newPostalCode == "") ChangeRestaurantAddress();
                SelectionMenu.Make(new[] { "Ja", "Nee" }, new Action[] { null, ChangeRestaurantAddress }, Logo.RestaurantGegevens, $"\nWeet u zeker dat u de postcode wil veranderen naar {newPostalCode}?\n");
                Json.Restaurant.Address.PostalCode = newPostalCode;
                Serialize(Json.Restaurant, "restaurant.json");
            }
            else if (choices[chosen] == "Stad \n")
            {
                string newCity = ChangeInfoText("Voer de nieuwe stad in.", Logo.RestaurantGegevens);
                if (newCity == "") ChangeRestaurantAddress();
                SelectionMenu.Make(new[] { "Ja", "Nee" }, new Action[] { null, ChangeRestaurantAddress }, Logo.RestaurantGegevens, $"\nWeet u zeker dat u de stad wil veranderen naar {newCity}?\n");
                Json.Restaurant.Address.City = newCity;
                Serialize(Json.Restaurant, "restaurant.json");
            }
            ChangeInfoSucces("Adres van het restaurant is aangepast", ChangeRestaurantAddress);
        }

        private static void ChangeRestaurantHours() // Verandert openingstijden. 
        {
            var allDays = new[] { "Maandag", "Dinsdag", "Woensdag", "Donderdag", "Vrijdag", "Zaterdag", "Zondag \n", "Terug" };
            var day = new SelectionMenu(allDays, Logo.RestaurantGegevens, "\nKies van welke dag u de openingstijden wil aanpassen.\n");
            var chosenDay = day.Show();
            if (allDays[chosenDay] == "Terug") ChangeRestaurantInfo();

            var closedOrNot = new[] { "Ja", "Nee" }; // Of de dag gesloten moet zijn of niet.
            var closeCheck = new SelectionMenu(closedOrNot, Logo.RestaurantGegevens, "\nWilt u dat deze dag gesloten is?\n");
            var chosen = closeCheck.Show();
            if (closedOrNot[chosen] == "Ja") // Hier wordt de dag naar gesloten gezet.
            {
                SelectionMenu.Make(new[] { "Ja", "Nee" }, new Action[] { null, ChangeRestaurantHours }, Logo.RestaurantGegevens ,$"\nWeet u zeker dat u {(allDays[chosenDay] != "Zondag \n" ? allDays[chosenDay].ToLower() : allDays[chosenDay].Remove(allDays[chosenDay].Length - 2).ToLower())} op gesloten wil zetten?\n");
                Json.Restaurant.OpeningHours[chosenDay] = "Gesloten";
                Serialize(Json.Restaurant, "restaurant.json");
                ChangeInfoSucces($"{allDays[chosenDay]} is succesvol veranderd naar 'Gesloten'.", ChangeRestaurantHours);
            }

            TimeSpan check;
            Console.CursorVisible = true;
            string newOpeningHour = ChangeInfoText("Voer de openingstijd in.\nHet format is '00:00'\nVoorbeeld: 17:00", Logo.RestaurantGegevens); // Openingstijd gedeelte.
            while (true)
            {
                if (newOpeningHour == "") ChangeRestaurantHours(); // Gaat terug naar vorig menu.
                else if (TimeSpan.TryParse(newOpeningHour, out check)) break; // Bij goede format breaked hij en gaat hij verder.
                Logo.PrintLogo(Logo.RestaurantGegevens);
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine("Ingevoerde waarde is niet in het correcte format of het is geen echte tijd.\nVoer opnieuw de openingstijd in.\nHet format is '00:00'.\nVoorbeeld: 17:00\nOf druk op 'enter' om terug te gaan\n");
                Console.ForegroundColor = ConsoleColor.White;
                Console.CursorVisible = true;
                newOpeningHour = Console.ReadLine();
            }
            TimeSpan check2; 
            string newClosingHour = ChangeInfoText("Voer de sluitingstijd in.\nHet format is '00:00'\nVoorbeeld: 22:00", Logo.RestaurantGegevens); // Sluitingstijd gedeelte.
            while (true)
            {
                if (newClosingHour == "") ChangeRestaurantInfo(); // Gaat terug naar vorig menu.
                else if (TimeSpan.TryParse(newClosingHour, out check2)) break; // Bij goede format breaked hij en gaat hij verder.
                Logo.PrintLogo(Logo.RestaurantGegevens);
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine("Ingevoerde waarde is niet in het correcte format of het is geen echte tijd.\nVoer opnieuw de sluitingstijd in.\nHet format is '00:00'.\nVoorbeeld: 22:00\nOf druk op 'enter' om terug te gaan\n");
                Console.ForegroundColor = ConsoleColor.White;
                Console.CursorVisible = true;
                newClosingHour = Console.ReadLine();
            }
            SelectionMenu.Make(new[] { "Ja", "Nee" }, new Action[] { null, ChangeRestaurantHours }, Logo.RestaurantGegevens, $"De nieuwe openings- en sluitingstijden op {allDays[chosenDay].ToLower()} worden:\n{newOpeningHour}-{newClosingHour}\nWilt u deze verandering toepassen?\n"); // check 
            Json.Restaurant.OpeningHours[chosenDay] = $"{newOpeningHour}-{newClosingHour}";
            Serialize(Json.Restaurant, "restaurant.json"); // Slaat het op
            ChangeInfoSucces("Openings- en sluitings tijden zijn veranderd.", ChangeRestaurantHours);
        }

        private static void ChangeRestaurantContactInfo() // Voor het veranderen van contactinformatie.
        {
            var choices = new[] { "Emailadres", "Telefoonnummer \n", "Terug" }; 
            var menu = new SelectionMenu(choices, Logo.RestaurantGegevens, "\nKies wat u wilt aanpassen\n"); // Wordt gevraagd welke optie aangepast moet worden.
            var chosen = menu.Show();
            if (choices[chosen] == "Terug") ChangeRestaurantInfo();

            if (choices[chosen] == "Emailadres")
            {
                string newEmail = ChangeInfoText("Vul het nieuwe emailadres in.", Logo.RestaurantGegevens);
                if (newEmail == "") ChangeRestaurantContactInfo();
                SelectionMenu.Make(new[] { "Ja", "Nee" }, new Action[] { null, ChangeRestaurantContactInfo }, Logo.RestaurantGegevens, $"\nWeet u zeker dat u het emailadres wilt veranderen naar {newEmail}?\n");
                Json.Restaurant.ContactInformation[0] = newEmail;
                Serialize(Json.Restaurant, "restaurant.json");
            }
            else if (choices[chosen] == "Telefoonnummer \n")
            {
                string newPhoneNumber = ChangeInfoText("Vul het nieuwe telefoonnummer in.", Logo.RestaurantGegevens);
                while (true)
                {
                    if (newPhoneNumber == "") ChangeRestaurantContactInfo();

                    // Check om te kijken of de input wel een telefoon nummer zou kunnen zijn. (correct aantal getallen)
                    newPhoneNumber = newPhoneNumber.Trim().Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", ""); // Haalt gebruikelijke characters eruit
                    if (newPhoneNumber.Length != 10) // Als de lengte niet 10 is, is het geen telefoonnummer.
                    {
                        newPhoneNumber = ChangeInfoText("Gegeven waarde is geen telefoonnummer.\nVul het nieuwe telefoonnummer in.", Logo.RestaurantGegevens);
                        continue;
                    }
                    try { int.Parse(newPhoneNumber); break; } // Als hij dit kan, betekent het dat er alleen nog maar getallen in zitten. (het kan dan een telefoonnmmr. zijn)
                    catch { newPhoneNumber = ChangeInfoText("Gegeven waarde is geen telefoonnummer.\nVul het nieuwe telefoonnummer in.", Logo.RestaurantGegevens); }
                }
                SelectionMenu.Make(new[] { "Ja", "Nee" }, new Action[] { null, ChangeRestaurantContactInfo }, Logo.RestaurantGegevens, $"\nWeet u zeker dat u het telefoonnummer wilt veranderen naar {newPhoneNumber}?");
                Json.Restaurant.ContactInformation[1] = newPhoneNumber;
                Serialize(Json.Restaurant, "restaurant.json");
            }
            ChangeInfoSucces("Contactinformatie is succesvol aangepast", ChangeRestaurantContactInfo);
        }

        private static string ChangeInfoText(string text, string logo) // Functie om makkelijk wat te printen en kleuren te veranderen. Returned meteen een string als input van de gebruiker.
        {
            Logo.PrintLogo(logo);
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine(text + "\nOf druk op 'enter' om terug te gaan.\n");
            Console.CursorVisible = true;
            Console.ForegroundColor = ConsoleColor.White;
            return Console.ReadLine();
        }
        private static void ChangeInfoSucces(string text, Action func) // Functie om te laten zien dat het succesvol opgeslagen is en gaat naar de gegeven functie.
        {
            Logo.PrintLogo(Logo.RestaurantGegevens);
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine(text + "\nDruk op 'enter' om door te gaan");
            Console.ReadLine();
            func();
        }

        public static void ChangePassword()
        {
            string password;
            while (true)
            {
                Logo.PrintLogo(Logo.Wachtwoord);
                Console.CursorVisible = true;
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine("Voer het nieuwe wachtwoord in.\nOf druk op 'enter' om terug te gaan.\n");
                Console.ForegroundColor = ConsoleColor.White;
                string newPassword = Console.ReadLine();
                if (newPassword == "") ChangeMenu();
                Logo.PrintLogo(Logo.Wachtwoord);
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine("Herhaal het nieuwe wachtwoord:");
                Console.ForegroundColor = ConsoleColor.White;
                if (Console.ReadLine() == newPassword)
                {
                    password = newPassword;
                    break;
                }
                else
                {
                    Logo.PrintLogo(Logo.Wachtwoord);
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine("De ingevoerde wachtwoorden komen niet overeen.");
                    Utils.Enter("om opnieuw te proberen");
                }
            }
            File.WriteAllText("password.txt", password);
            ChangeMenu();
        }
    }
}
