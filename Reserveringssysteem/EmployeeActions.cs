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
			if (option < dishNames.Count -1 && option > 0)
			{
				_dish = dishes[option -1];
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
				case "Prijs": case "Prijs \n":
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
			while(true)
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
            SelectionMenu.Make(new[] { "Naam van het restaurant", "Beschrijving", "Adress", "Capaciteit", "Openingstijden", "Contactinformatie\n", "Terug"}, new Action[] { ChangeRestaurantName, ChangeRestaurantDescription, ChangeRestaurantAddress, ChangeRestaurantHours, ChangeRestaurantContactInfo, MainMenu }, Logo.RestaurantGegevens, "\nKies een optie om te wijzigen\n");
			//Serialize(new Restaurant("De houten vork", new Location("Wijhaven", "107", "3011 WN", "Rotterdam"), 100, new string[] { "10:00-23:00", "10:00-23:00", "10:00-23:00", "10:00-23:00", "10:00-23:00", "10:00-23:00", "Gesloten" }, new string[] { "oscar.vugt@gmail.com", "06-12932305" }), "restaurant.json");
		}

		private static void ChangeRestaurantName()
        {
			Logo.PrintLogo(Logo.RestaurantGegevens);
            Console.WriteLine("Voer de nieuwe naam in voor het restaurant:\nOf druk op 'enter' om terug te gaan");
			Console.CursorVisible = true;
			string newName = Console.ReadLine();
			if (newName == "") MainMenu();
			Console.Clear();
			SelectionMenu.Make(new[] { "Ja", "Nee" }, new Action[] { null, ChangeRestaurantName }, Logo.RestaurantGegevens, $"Weet u zeker dat u de naam van het restaurant naar \"{newName}\" wil veranderen?");
        }

		private static void ChangeRestaurantDescription()
        {

        }

		private static void ChangeRestaurantAddress()
        {

        }

		private static void ChangeRestaurantHours()
        {

        }

		private static void ChangeRestaurantContactInfo()
        {

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
