using System;

namespace Reserveringssysteem
{
    class Program
    {
        static void Main(string[] args)
        {
<<<<<<< Updated upstream
            Console.WriteLine("Hello World!");
=======
            ReservationList = Deserialize<List<Reservation>>("reservations.json");
            if (state == null)
            {
            var introMenu = new SelectionMenu(new string[4] { "1] Reservering", "2] Bekijk de menukaart", "3] Informatie over ons", "[Voor Medewerkers]" });
                switch (introMenu.Show())
                {
                    case 0:
                        state = "Reservating";
                        break;
                    case 1:
                        state = "Menu";
                        break;
                    case 2:
                        state = "Information";
                        break;
                    case 3:
                        state = "Employee";
                        break;
                    default:
                        Console.WriteLine("Deze funcite is nog niet geimplementeerd.");
                        break;
                }
            }
            if (state == "Reservating")
            {
                var reservationMenu = new SelectionMenu(new string[2] { "1] Maak een reservering", "2] Annuleer een reservering" });
                reservationMenu.Show();
                Serialize(new List<Reservation>() { new Reservation() { Name = "Oscar", Size = 6, Date = "26-04-2021", Time = "18:00" } }, "reservations.json");
                Reservations.Reservate();
            }
            else if (state == "Menu")
            {
                Console.Write("Menukaart..");
                Console.ReadLine();
            }
            else if (state == "Information")
            {
                Console.WriteLine("Over ons..");
                Console.ReadLine();
            }
            else if (state == "Employee")
            {
                Console.WriteLine("Medewerker?");
                Console.ReadLine();
            }
>>>>>>> Stashed changes
        }
    }
}

