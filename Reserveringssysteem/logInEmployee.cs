using System;
namespace Reserveringssysteem
{
	class logInEmployee
    {
		string wachtwoord = "wachtwoord123";
		

		public static void Main()
        {
			
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
				Main();
            }

        }
		

	}
	
}
	