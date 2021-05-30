using System;
using System.Linq;
using static Reserveringssysteem.Json;

namespace Reserveringssysteem
{
    public class Reservation
    {
        public string ReservationId { get; set; }
        public string Name { get; set; }
        public string Date { get; set; }
        public string[] TimeSlot { get; set; }
        public int Size { get; set; }
        public string[] DiscountMenus { get; set; }
        public string Comments { get; set; }
        public string OrderStatus { get; set; }

        public Reservation()
        {
            ReservationId = GenerateReservationCode();
            OrderStatus = "ACCEPTED";
        }

        private static string GenerateReservationCode()
        {            
            int counter = 0;
            while (counter < 1000)
            {
                var code = new string(Enumerable.Repeat("ABCDEFGHIJKLMNPQRSTUVWXYZ123456789", 4).Select(s => s[new Random().Next(s.Length)]).ToArray());
                bool exceeded = true;
                if (ReservationList == null) return code;
                foreach (var reservation in ReservationList)
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
            ReservationList.Add(this);
            Serialize(ReservationList, "reservations.json");
        }
    }
}