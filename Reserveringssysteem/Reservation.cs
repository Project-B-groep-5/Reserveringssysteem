using System;
using System.Collections.Generic;
using System.Linq;
using static Reserveringssysteem.Json;

namespace Reserveringssysteem
{
    public class Reservation
    {
        public string ReservationId { get; set; }
        public string Name { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public int Size { get; set; }
        public bool TookDiscountMenu { get; set; }
        public string Comments { get; set; }
        public string OrderStatus { get; set; }

        public Reservation()
        {
            ReservationId = GenerateReservationCode();
            OrderStatus = "ACCEPTED";
        }

        public static string GenerateReservationCode()
        {            
            int counter = 0;
            while (counter < 1000)
            {
                var code = new string(Enumerable.Repeat("ABCDEFGHIJKLMNPQRSTUVWXYZ123456789", 4).Select(s => s[new Random().Next(s.Length)]).ToArray());
                bool exceeded = true;
                if (Program.ReservationList == null) return code;
                foreach (var reservation in Program.ReservationList)
                {
                    if (reservation.ReservationId == code)
                    {
                        exceeded = false;
                        break;
                    }
                }
                if (exceeded) return code;
                counter++;
            }
            return null;
        }

        public void Save()
        {
            var reservations = Deserialize<List<Reservation>>("reservations.json");
            reservations.Add(this);
            Serialize(reservations, "reservations.json");
        }
    }
}