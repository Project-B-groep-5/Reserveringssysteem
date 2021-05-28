using System;
using System.IO;

namespace Reserveringssysteem
{
	public class LogInEmployee
    {
		private static void Header() => Logo.PrintLogo(Logo.Inloggen);

		public static void LogIn()
        {
			Console.CursorVisible = true;
			string password;
            try
			{
				password = File.ReadAllText("password.txt");
			} 
			catch (FileNotFoundException)
            {
				EmployeeActions.ChangePassword();
				password = File.ReadAllText("password.txt");
			}

			Header();
			Console.WriteLine("Voer wachtwoord in:\nOf druk op 'enter' om terug te gaan.");
			string ingevoerdWachtwoord = Console.ReadLine();

			if (ingevoerdWachtwoord == "") Program.Main();
			else if (ingevoerdWachtwoord == password)
				EmployeeActions.MainMenu();
			else 
            {
				Header();
				Console.WriteLine("Incorrect wachtwoord.");
				Utils.Enter("om opnieuw te proberen");
				LogIn();
            }

        }
		

	}
	
}
	