using System;
namespace Reserveringssysteem
{
	class logInEmployee
    {
		public static void logIn()
        {
			string wachtwoord = "wachtwoord123";
			Console.WriteLine("Voer wachtwoord in:");
			string ingevoerdWachtwoord = Console.ReadLine();
			Console.Clear();
			if (ingevoerdWachtwoord == wachtwoord)
            {
				Console.WriteLine("Naar medewerkers omgeving.");
            }
			else if (ingevoerdWachtwoord != wachtwoord)
            {
				Console.WriteLine("Probeer opnieuw.");
				logIn();
            }

        }
		public static void Main()
        {
			logIn();
        }

	}
	
}
	