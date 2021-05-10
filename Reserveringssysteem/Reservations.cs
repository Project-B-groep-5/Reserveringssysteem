using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using static Reserveringssysteem.Json;

namespace Reserveringssysteem
{
    public class Reservations
    {
        public static void ReservateTitle() // Call deze method om de onderstaande header te krijgen
        {
            Console.WriteLine(Logo.Reserveren); 
        }
        public static void sendEmail(string emailAddress, string reservationCode, string name, string time, string date) // Method om mail te sturen met de bevestigingscode, indien de klant dit graag wilt.
        {
            Console.WriteLine("De bevestigingsmail wordt nu verstuurd. Sluit dit menu nog niet af......");
            string mailMessage = @$"
                                Beste {name},

                                Hartelijk dank voor uw reservering bij Restaurant de Houten Vork op {time} om {date}.
                                Uw reserveringscode is : {reservationCode} , bewaar deze code goed. 

                                Tot dan!
                                ";
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("RestaurantProjectB@gmail.com", "Test12345XX"),
                EnableSsl = true,
            };

            smtpClient.Send("RestaurantProjectB@gmail.com", emailAddress, "Uw reservering is bevestigd!", mailMessage);
            Console.Clear();
            Console.WriteLine("Bevestiginsmail verstuurd. Vergeet niet uw spamfolder te bekijken als u geen bevestigingsmail heeft gehad.\n") ;
        }
        public static void Reservate()
        {
            string name;
            int size = 0;
            string date;
            string time = "" ;
            DateTime dDate;
            var datumVandaag = DateTime.UtcNow.ToString("dd-MM-yyyy");
            Reservation reservation;
            ReservateTitle();
            Console.WriteLine("Wat is uw naam?");
            name = Console.ReadLine();
            while (true)
            {

                if (name.Length == 0)
                {
                    Console.Clear();
                    ReservateTitle(); 
                    Console.WriteLine("Geen naam ingevuld. Probeer opnieuw : \n");
                    name = Console.ReadLine();
                }
                else
                {
                    Console.Clear();
                    break;
                }

            }
            ReservateTitle();
            Console.WriteLine("Voor hoeveel mensen wilt u een reservering maken?");
            while (size == 0)
            {
                var input = Console.ReadLine();
                try
                {
                    size = int.Parse(input);
                }
                catch
                {
                    Console.Clear();

                    ReservateTitle();


                    Console.WriteLine($"{Logo.Reserveren}\n{input} is geen correcte waarde.\nVul alstublieft een getal in.");

                }
            }
            Console.Clear();
            ReservateTitle();
            Console.WriteLine("Voor wanneer wilt u reserveren?");
            Console.WriteLine("Gebruik alstublieft het format: DD-MM-JJJJ");
            Console.WriteLine("De datum van vandaag is {0}", datumVandaag);
            date = Console.ReadLine();
            while (true)
            {
                if (!DateTime.TryParse(date, out dDate) || DateTime.Parse(date) < DateTime.Parse(datumVandaag) )
                {
                    Console.WriteLine("Opgegeven datum is niet gelijk aan het format of in het verleden. Het format is : DD-MM-JJJJ");
                    date = Console.ReadLine();
                }
                else
                {
                    String.Format("{0:dd/MM/yyyy}", dDate);
                    break;
                }
            }

            Console.Clear();
            var timeMenu = new SelectionMenu(new string[7] { "17:00", "17:30", "18:00", "18:30", "19:00", "19:30", "20:00" }, Logo.Reserveren, "\nHoe laat wilt u komen eten?\n");
            switch (timeMenu.Show())
            {
                case 0:
                    time = "17:00";
                    break; 
                case 1:
                    time = "17:30";
                    break;
                case 2:
                    time = "18:00";
                    break;
                case 3:
                    time = "18:30";
                    break;
                case 4:
                    time = "19:00";
                    break;
                case 5:
                    time = "19:30";
                    break;
                case 6:
                    time = "20:00";
                    break;
            }
            Console.Clear();
            reservation = new Reservation { Name = name, Date = date, Time = time, Size = size };
            ReservateTitle();
            Console.WriteLine("Om uw reservering te bevestigen hebben wij uw mail adres nodig.");
            Console.WriteLine("Naar welk mail adres mogen wij de reservering sturen? : \n");
            var emailAddress = Console.ReadLine();
            while (true)
            {
                if (emailAddress.Length == 0)
                {
                    Console.WriteLine("Geen email ingevuld. Probeer opnieuw : \n");
                    emailAddress = Console.ReadLine();
                }
                else
                    break;
            }
            Console.Clear();
                //Functie om mail te versturen
                sendEmail(emailAddress, reservation.ReservationId, name, date, time);
            Console.WriteLine($"Je hebt een reservering gemaakt op : {date} om : {time} uur!\nJe reserveringscode is : {reservation.ReservationId} \n\nDruk op 'enter' om terug te gaan");
        }
    }
}