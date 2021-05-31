using System;
using System.IO;
using System.Collections.Generic;
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
                    ChangeName();
                    break;
                case "Prijs":
                case "Prijs \n":
                    ChangePrice();
                    break;
                case "Ingredienten":
                case "Ingredienten \n":
                    ChangeIngredients();
                    break;
                case "Verwijderen \n":
                    RemoveDish();
					break;
				default:
					SelectDish();
					return;
			}
			Serialize(DishList, "dishes.json");
		}

        private static void RemoveDish()
        {
            if (Utils.Confirm(Logo.Dashboard, $"Weet u zeker dat u een gerecht met de naam: {_dish.Name} wilt verwijderen?\n"))
            {
                DishList.Remove(_dish);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(_dish.Name);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(" verwijderd.");
                Utils.Enter(ChangeDish);
                return;
            }
            ChangeDish();
        }

        private static void ChangeName()
        {
            Logo.PrintLogo(Logo.Dashboard);
            var naam = _dish.Name;
            Console.Write("De nieuwe naam voor");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(naam);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine();
            var input = Utils.Input();
            if (input != "")
            {
                _dish.Name = input;
                Logo.PrintLogo(Logo.Dashboard);
                Console.Write(naam);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("veranderd in: ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(_dish.Name);
                Utils.Enter("om verder te gaan.");
                return;
            }
            Utils.NoInput(ChangeName, ChangeDish, Logo.Dashboard);
        }

        private static void ChangePrice()
        {
            var prijs = _dish.Price;
            while (true)
            {
                string input = "";
                try
                {
                    Logo.PrintLogo(Logo.Dashboard);
                    Console.WriteLine($"De nieuwe prijs voor {_dish.Name} is: \n");
                    input = Console.ReadLine();
                    if (input == "")
                    {
                        Utils.NoInput(ChangePrice, ChangeDish, Logo.Dashboard);
                        return;
                    }
                    var newPrice = double.Parse(input);
                    if (newPrice > 0)
                    {
                        _dish.Price = newPrice;
                        Console.WriteLine($"Prijs voor {_dish.Name} veranderd van {prijs} naar {_dish.Price}");
                        Utils.Enter("om verder te gaan.");
                        return;
                    }
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nDe prijs kan niet kleiner dan of nul zijn.");
                    Utils.Enter("om opnieuw te proberen");
                }
                catch
                {
                    Logo.PrintLogo(Logo.Dashboard);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(input);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(" is geen correcte waarde voor een prijs.\nPrijzen zijn bijvoorbeeld genoteerd als volgt: ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("19.5 of 19");
                    Utils.Enter("om opnieuw te proberen");
                }
            }
        }

        private static string _ingredient;
        private static void ChangeIngredients()
        {
			Logo.PrintLogo(Logo.Dashboard);
			var choices = new List<string> { "Toevoegen \n" };
			foreach (var ingredient in _dish.Ingredients)
				choices.Add(ingredient);
			if (!choices[^1].Contains("\n"))
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
            if (Utils.Confirm(Logo.Dashboard, $"Weet u zeker dat u een ingrediënt met de naam: {_ingredient} wilt verwijderen?\n"))
            {
                _dish.Ingredients.Remove(_ingredient);
                Serialize(DishList, "dishes.json");
                Logo.PrintLogo(Logo.Dashboard);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(_ingredient);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(" verwijderd.");
                Utils.Enter(ChangeIngredients);
                return;
            }
            ChangeIngredients();
		}

        private static void ChangeIngredient()
        {
            Console.WriteLine($"Waar wilt u {_ingredient} in veranderen?\nVoer de nieuwe naam in:");
			var name = Console.ReadLine();
			if (name == "")
			{
                Utils.NoInput(ChangeIngredient, ChangeIngredientMenu, Logo.Dashboard);
				return;
			}
			if (Utils.Confirm(Logo.Dashboard, $"Weet u zeker dat u {_ingredient} wilt veranderen in {name}?\n"))
			{
				var ingredientInt = _dish.Ingredients.IndexOf(_ingredient);
				_dish.Ingredients[ingredientInt] = name;
				Serialize(DishList, "dishes.json");
				Logo.PrintLogo(Logo.Dashboard);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(_ingredient);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(" veranderd naar ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(name);
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
                Utils.NoInput(AddIngredient, ChangeIngredients, Logo.Dashboard);
                return;
			}
			if (Utils.Confirm(Logo.Dashboard, $"Weet u zeker dat een ingrediënt met de naam: {name} wilt toevoegen?\n"))
			{
				_dish.Ingredients.Add(name);
				Serialize(DishList, "dishes.json");
				Logo.PrintLogo(Logo.Dashboard);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(name);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(" toegevoegd.");
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
            _dish = new Dish(naam, prijs, null, _category, null, null);
            DishList.Add(_dish);
            Serialize(DishList, "dishes.json");
            Console.WriteLine($"{naam} toegevoegd aan {_category}.");
        }
        private static VoordeelMenu _menu;
        private static void ChangeMenus() // Om een voordeelmenu te veranderen / toe te voegen.
        {
            var choices = new List<string>
            {
                "Toevoegen \n"
            };
            foreach (var voordeelmenu in VoordeelMenus) // Maakt array met alle namen van de voordeelmenus
            {
                choices.Add(voordeelmenu.Name);
            }
            if (!choices[^1].Contains("\n"))
                choices[^1] += " \n";
            choices.Add("Terug"); 

            var menusMenu = new SelectionMenu(choices.ToArray(), Logo.GerechtenMenus);
            var chosen = menusMenu.Show();

            if (chosen == choices.Count - 1) ChangeMenu(); // Gaat terug 
            else if (chosen == 0) AddMenu(); // Voegt een voordeelmenu toe
            else
            {
                _menu = VoordeelMenus[chosen -1];
                ChangeSpecificMenu(); // Om een voordeelmenu aan te passen / te verwijderen.
            }

        }

        private static void ChangeSpecificMenu()
        {
            var choices = new[] { "Naam", "Voorgerecht", "Hoofdgerecht", "Nagerecht", "Prijs \n", "Verwijderen \n", "Terug" };
            var menu = new SelectionMenu(choices, Logo.GerechtenMenus, "Kies wat u wilt aanpassen.\n");
            var choice = menu.Show();
            if (choice == 0)
            {
                ChangeMenuName();
            }
            else if (choice < 4)
            {
                ChangeMenuByType(choices[choice]);
            }
            else if (choice == 4)
            {
                ChangeMenuPrice();
            }
            else if (choice == 5)
            {
                DeleteMenu();
            }
            else
            {
                ChangeMenus();
                return;
            }
            Serialize(VoordeelMenus, "voordeelmenu.json");
            ChangeInfoSucces("Voordeelmenu is succesvol aangepast", ChangeMenus);
        }

        private static void ChangeMenuPrice()
        {
            double newPrice = PriceChange(ChangeSpecificMenu);
            if(Utils.Confirm(Logo.GerechtenMenus, $"Weet u zeker dat u de prijs van het voordeelmenu naar €{newPrice:0.00} wilt veranderen?\n"))
            {
                _menu.Prijs = newPrice;
                return;
            }
            ChangeMenuPrice();
        }

        private static void ChangeMenuByType(string type)
        {
            Dish dish = MakeDishMenu(type, ChangeSpecificMenu);
            if (dish != null)
            {
                if (Utils.Confirm(Logo.GerechtenMenus, $"Weet u zeker dat u het {type.ToLower()} van het voordeelmenu wilt veranderen naar {dish.Name}?\n"))
                {
                    switch(type)
                    {
                        case "Voorgerecht":
                            _menu.VoorGerecht = dish;
                            return;
                        case "Hoofdgerecht":
                            _menu.HoofdGerecht = dish;
                            return;
                        case "Nagerecht":
                            _menu.NaGerecht = dish;
                            return;
                    }
                }
                ChangeMenuByType(type);
            }
        }

        private static void ChangeMenuName()
        {
            string newName = ChangeInfoText("Voer de nieuwe naam in van het voordeelmenu.", Logo.GerechtenMenus);
            if (newName == "")
            {
                Utils.NoInput(ChangeMenuName, ChangeSpecificMenu, Logo.GerechtenMenus);
                return;
            }
            if (Utils.Confirm(Logo.GerechtenMenus, $"Weet u zeker dat u de naam van het voordeelmenu naar {newName} wilt veranderen?\n"))
            {
                Logo.PrintLogo(Logo.Dashboard);
                Console.Write(_menu.Name);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("veranderd in: ");
                Console.ForegroundColor = ConsoleColor.White;
                _menu.Name = newName;
                Console.Write(_menu.Name);
                return;
            }
            ChangeMenuName();
        }

        private static void DeleteMenu() // Functie om een voordeelmenu te verwijderen.
        {
            SelectionMenu.Make(new[] { "Ja", "Nee" }, new Action[] { null, ChangeMenus }, Logo.GerechtenMenus, $"\nWeet u zeker dat u het voordeelmenu: \"{_menu.Name}\", wilt verwijderen?\n"); // check
            VoordeelMenus.Remove(_menu); // -1 omdat de eerste keuze in de array de optie "toevoegen" was.
            Serialize(VoordeelMenus, "voordeelmenu.json");
            ChangeInfoSucces("Voordeelmenu is succesvol verwijderd", ChangeMenus);
        }

        private static void AddMenu() // Voegt een voordeelmenu toe
        {
            string newName = ChangeInfoText("Voer de naam in van het voordeelmenu.", Logo.GerechtenMenus);
            if (newName == "") ChangeMenus();
            Dish newVoorGerecht = MakeDishMenu("Voorgerechten", AddMenu);
            Dish newHoofdGerecht = MakeDishMenu("Hoofdgerechten", AddMenu);
            Dish newNaGerecht = MakeDishMenu("Nagerechten", AddMenu);
            double newPrijs = PriceChange(AddMenu);
            if (Utils.Confirm(Logo.GerechtenMenus, $"\nNaam: {newName}\n" +
                $"Voorgerecht: {newVoorGerecht.Name}\n" +
                $"Hoofdgerecht: {newHoofdGerecht.Name}\n" +
                $"Nagerecht: {newNaGerecht.Name}\n" +
                $"Prijs: €{newPrijs:0.00}\n" +
                $"Het voordeelmenu wordt toegevoegd. Wilt u doorgaan?\n"))
            {
                VoordeelMenus.Add(new VoordeelMenu(newName, newVoorGerecht, newHoofdGerecht, newNaGerecht, newPrijs));
                Serialize(VoordeelMenus, "voordeelmenu.json"); // Slaat het op.
                ChangeInfoSucces("Voordeelmenu is succesvol opgeslagen.", ChangeMenus);
                return;
            }
            AddMenu();            
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
                    if (newPrijsStr == "")
                    {
                        var menu = new SelectionMenu(new[] { "Opnieuw proberen", "Terug" }, Logo.GerechtenMenus, "Niks ingevult.\n\nKies een optie:\n");
                        if (menu.Show() == 0)
                            PriceChange(func);
                        else
                            func();
                        return 0;
                    }
                    newPrijs = double.Parse(newPrijsStr, 0.00);
                    break;
                }
                catch // Gecalled wanneer input geen prijs is en vraagt het opnieuw.
                {
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
            allDishes[^1] += " \n";
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
            if (newName == "")
            {
                Utils.NoInput(ChangeRestaurantName, ChangeRestaurantInfo, Logo.RestaurantGegevens);
                return;
            }
            if(Utils.Confirm(Logo.RestaurantGegevens, $"\nWeet u zeker dat u de naam van het restaurant naar \"{newName}\" wil veranderen?\n"))
            {
                Json.Restaurant.Name = newName;
                Serialize(Json.Restaurant, "restaurant.json"); // Slaat het op.
                ChangeInfoSucces($"Naam succesvol is veranderd naar \"{newName}\"", ChangeRestaurantInfo);
                return;
            }
            ChangeRestaurantName();
        }

        private static void ChangeRestaurantDescription() // Verandert beschrijving van het restaurant.
        {
            string newDescription = ChangeInfoText("Voer de nieuwe beschrijving van het restaurant in.", Logo.RestaurantGegevens);
            if (newDescription == "")
            {
                Utils.NoInput(ChangeRestaurantDescription, ChangeRestaurantInfo, Logo.RestaurantGegevens);
                return;
            }
            Console.Clear();
            if (Utils.Confirm(Logo.RestaurantGegevens, $"\nDe nieuwe beschrijving van het restaurant wordt:\n\n{newDescription}\n\nWilt u deze verandering toepassen?\n"))
            {
                Json.Restaurant.Description = newDescription;
                Serialize(Json.Restaurant, "restaurant.json");
                ChangeInfoSucces("Beschrijving is succesvol veranderd.", ChangeRestaurantInfo);
                return;
            }
            ChangeRestaurantDescription();
        }

        private static void ChangeRestaurantAddress() // Verandert het adres. (kan maar 1 element veranderen per keer, maar anders zou je alles moeten veranderen elke keer.)
        {
            var choices = new[] { "Straatnaam", "Huisnummer", "Postcode", "Stad \n", "Terug" };
            var menu = new SelectionMenu(choices, Logo.RestaurantGegevens); // Wordt gevraagd wat veranderd moet worden.
            var chosen = menu.Show();

            if (choices[chosen] == "Terug") ChangeRestaurantInfo(); 
            else if (choices[chosen] == "Straatnaam")
            {
                ChangeStreet();
            }
            else if (choices[chosen] == "Huisnummer")
            {
                ChangeNumber();
            }
            else if (choices[chosen] == "Postcode")
            {
                ChangePostalCode();
            }
            else if (choices[chosen] == "Stad \n")
            {
                ChangeCity();
            }
            Serialize(Json.Restaurant, "restaurant.json");
            ChangeInfoSucces("Adres van het restaurant is aangepast", ChangeRestaurantAddress);
        }

        private static void ChangeCity()
        {
            string newCity = ChangeInfoText("Voer de nieuwe stad in.", Logo.RestaurantGegevens);
            if (newCity == "")
            {
                Utils.NoInput(ChangeCity, ChangeRestaurantAddress, Logo.RestaurantGegevens);
                return;
            }
            if (Utils.Confirm(Logo.RestaurantGegevens, $"\nWeet u zeker dat u de stad wil veranderen naar {newCity}?\n"))
            {
                Json.Restaurant.Address.City = newCity;
                return;
            }
            ChangeCity();
        }

        private static void ChangePostalCode()
        {
            string newPostalCode = ChangeInfoText("Voer de nieuwe postcode in.", Logo.RestaurantGegevens);
            if (newPostalCode == "")
            {
                Utils.NoInput(ChangePostalCode, ChangeRestaurantAddress, Logo.RestaurantGegevens);
                return;
            }
            if (Utils.Confirm(Logo.RestaurantGegevens, $"\nWeet u zeker dat u de postcode wil veranderen naar {newPostalCode}?\n"))
            {
                Json.Restaurant.Address.PostalCode = newPostalCode;
                return;
            }
            ChangePostalCode();
        }

        private static void ChangeNumber()
        {
            string newHouseNumber = ChangeInfoText("Voer het nieuwe huisnummer in.", Logo.RestaurantGegevens);
            if (newHouseNumber == "")
            {
                Utils.NoInput(ChangeNumber, ChangeRestaurantAddress, Logo.RestaurantGegevens);
                return;
            }
            if (Utils.Confirm(Logo.RestaurantGegevens, $"\nWeet u zeker dat u het huisnummer wil veranderen naar {newHouseNumber}?\n"))
            {
                Json.Restaurant.Address.HouseNumber = newHouseNumber;
                return;
            }
            ChangeNumber();
        }

        private static void ChangeStreet()
        {
            string newStreetName = ChangeInfoText("Voer het nieuwe adres in.", Logo.RestaurantGegevens);
            if (newStreetName == "")
            {
                Utils.NoInput(ChangeStreet, ChangeRestaurantAddress, Logo.RestaurantGegevens);
                return;
            }
            if (Utils.Confirm(Logo.RestaurantGegevens, $"\nWeet u zeker dat u de straatnaam wil vernaderen naar { newStreetName}?\n"))
            {
                Json.Restaurant.Address.StreetName = newStreetName;
                return;
            }
            ChangeStreet();
        }

        private static int _day;
        private static string[] _days = new[] { "Maandag", "Dinsdag", "Woensdag", "Donderdag", "Vrijdag", "Zaterdag", "Zondag \n", "Terug" };
        private static void ChangeRestaurantHours() // Verandert openingstijden. 
        {
            var day = new SelectionMenu(_days, Logo.RestaurantGegevens, "\nKies van welke dag u de openingstijden wil aanpassen.\n");
            _day = day.Show();
            if (_days[_day] == "Terug") ChangeRestaurantInfo();
            ChangeRestaurantHourMethod();
        }
        private static void ChangeRestaurantHourMethod()
        {
            var closedOrNot = new[] { "Ja", "Nee\n", "Terug" }; // Of de dag gesloten moet zijn of niet.
            var closeCheck = new SelectionMenu(closedOrNot, Logo.RestaurantGegevens, "\nWilt u dat deze dag gesloten is?\n");
            var chosen = closeCheck.Show();
            if (closedOrNot[chosen] == "Terug")
            {
                ChangeRestaurantHours();
                return;
            }
            if (closedOrNot[chosen] == "Ja") // Hier wordt de dag naar gesloten gezet.
            {
                if (Utils.Confirm(Logo.RestaurantGegevens, $"\nWeet u zeker dat u {(_days[_day] != "Zondag \n" ? _days[_day].ToLower() : _days[_day].Remove(_days[_day].Length - 2).ToLower())} op gesloten wil zetten?\n"))
                {
                    Json.Restaurant.OpeningHours[_day] = "Gesloten";
                    Serialize(Json.Restaurant, "restaurant.json");
                    ChangeInfoSucces($"{_days[_day]} is succesvol veranderd naar 'Gesloten'.", ChangeRestaurantHours);
                    return;
                }
            }

            Console.CursorVisible = true;
            ChangeRestaurantHoursOpening();
        }

        private static string _openingHour;
        private static void ChangeRestaurantHoursOpening()
        {
            _openingHour = ChangeInfoText("Voer de openingstijd in.\nHet format is '00:00'\nVoorbeeld: 17:00", Logo.RestaurantGegevens); // Openingstijd gedeelte.
            while (true)
            {
                if (_openingHour == "")
                {
                    Utils.NoInput(ChangeRestaurantHoursOpening, ChangeRestaurantHours, Logo.RestaurantGegevens);
                    return;
                }
                else if (TimeSpan.TryParse(_openingHour, out TimeSpan check))    // Bij goede format breaked hij en gaat hij verder.
                {
                    ChangeRestaurantHoursClosing();
                    return;
                } 
                Logo.PrintLogo(Logo.RestaurantGegevens);
                _openingHour = Utils.Input("Ingevoerde waarde is niet in het correcte format of het is geen echte tijd.\nVoer opnieuw de openingstijd in.\nHet format is '00:00'.\nVoorbeeld: 17:00\n");
            }
        }

        private static void ChangeRestaurantHoursClosing()
        {
            string newClosingHour = ChangeInfoText("Voer de sluitingstijd in.\nHet format is '00:00'\nVoorbeeld: 22:00", Logo.RestaurantGegevens); // Sluitingstijd gedeelte.
            while (true)
            {
                if (newClosingHour == "") 
                {
                    Utils.NoInput(ChangeRestaurantHoursClosing, ChangeRestaurantHoursOpening, Logo.RestaurantGegevens);
                }
                else if (TimeSpan.TryParse(newClosingHour, out TimeSpan check2)) break; // Bij goede format breaked hij en gaat hij verder.
                Logo.PrintLogo(Logo.RestaurantGegevens);
                newClosingHour = Utils.Input("Ingevoerde waarde is niet in het correcte format of het is geen echte tijd.\nVoer opnieuw de sluitingstijd in.\nHet format is '00:00'.\nVoorbeeld: 22:00\n");
            }
            if (Utils.Confirm(Logo.RestaurantGegevens, $"De nieuwe openings- en sluitingstijden op {_days[_day].ToLower()} worden:\n{_openingHour}-{newClosingHour}\nWilt u deze verandering toepassen?\n"))
            {
                Json.Restaurant.OpeningHours[_day] = $"{_openingHour}-{newClosingHour}";
                Serialize(Json.Restaurant, "restaurant.json"); // Slaat het op
                ChangeInfoSucces("Openings- en sluitings tijden zijn veranderd.", ChangeRestaurantHours);
                return;
            }
            ChangeRestaurantHoursClosing();
        }

        private static void ChangeRestaurantContactInfo() // Voor het veranderen van contactinformatie.
        {
            var choices = new[] { "Emailadres", "Telefoonnummer \n", "Terug" }; 
            var menu = new SelectionMenu(choices, Logo.RestaurantGegevens, "\nKies wat u wilt aanpassen\n"); // Wordt gevraagd welke optie aangepast moet worden.
            var chosen = menu.Show();
            if (choices[chosen] == "Terug")
            {
                ChangeRestaurantInfo();
                return;
            }
            if (choices[chosen] == "Emailadres")
            {
                ChangeEmail();
            }
            else if (choices[chosen] == "Telefoonnummer \n")
            {
                ChangePhone();
            }
            Serialize(Json.Restaurant, "restaurant.json");
            ChangeInfoSucces("Contactinformatie is succesvol aangepast", ChangeRestaurantContactInfo);
        }

        private static void ChangePhone()
        {
            string newPhoneNumber = ChangeInfoText("Vul het nieuwe telefoonnummer in.", Logo.RestaurantGegevens);
            while (true)
            {
                if (newPhoneNumber == "")
                {
                    Utils.NoInput(ChangePhone, ChangeRestaurantContactInfo, Logo.RestaurantGegevens);
                    return;
                }

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
            if (Utils.Confirm(Logo.RestaurantGegevens, $"\nWeet u zeker dat u het telefoonnummer wilt veranderen naar {newPhoneNumber}?"))
            {
                Json.Restaurant.ContactInformation[1] = newPhoneNumber;
                return;
            }
            ChangePhone();
        }

        private static void ChangeEmail()
        {
            string newEmail = ChangeInfoText("Vul het nieuwe emailadres in.", Logo.RestaurantGegevens);
            if (newEmail == "")
            {
                Utils.NoInput(ChangeEmail, ChangeRestaurantContactInfo, Logo.RestaurantGegevens);
                return;
            }
            if (Utils.Confirm(Logo.RestaurantGegevens, $"\nWeet u zeker dat u het emailadres wilt veranderen naar {newEmail}?\n"))
            {
                Json.Restaurant.ContactInformation[0] = newEmail;
                return;
            }
            ChangeEmail();
        }

        private static string ChangeInfoText(string text, string logo) // Functie om makkelijk wat te printen en kleuren te veranderen. Returned meteen een string als input van de gebruiker.
        {
            Logo.PrintLogo(logo);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(text);
            Console.CursorVisible = true;
            Console.ForegroundColor = ConsoleColor.White;
            return Console.ReadLine();
        }
        private static void ChangeInfoSucces(string text, Action func) // Functie om te laten zien dat het succesvol opgeslagen is en gaat naar de gegeven functie.
        {
            Logo.PrintLogo(Logo.RestaurantGegevens);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(text);
            Utils.Enter("om door te gaan");
            func();
        }

        public static void ChangePassword()
        {
            string password;
            while (true)
            {
                Logo.PrintLogo(Logo.Wachtwoord);
                Console.CursorVisible = true;
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Voer het nieuwe wachtwoord in.\nOf druk op 'enter' om terug te gaan.\n");
                Console.ForegroundColor = ConsoleColor.White;
                string newPassword = Console.ReadLine();
                if (newPassword == "")
                {
                    Utils.NoInput(ChangePassword, ChangeMenu, Logo.Wachtwoord);
                    return;
                }
                Logo.PrintLogo(Logo.Wachtwoord);
                Console.ForegroundColor = ConsoleColor.Cyan;
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
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("De ingevoerde wachtwoorden komen niet overeen.");
                    Utils.Enter("om opnieuw te proberen");
                }
            }
            File.WriteAllText("password.txt", password);
            ChangeMenu();
        }
    }
}
