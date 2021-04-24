using System;
using System.Collections.Generic;
using static Reserveringssysteem.Json;

namespace Reserveringssysteem
{
    class ReservationCheck
    {
        
        public static List<Reservation> ReservationList;
        
        public static bool Check(string date, string time, int size)
        {
            ReservationList = Deserialize<List<Reservation>>("reservations.json");
            int capacity = 100;
            int occupied = 0;
            occupied += size;
            for (int i = 0; i < ReservationList.Count; i++)
            {
                if (ReservationList[i].Date == date && ReservationList[i].Time == time) 
                    occupied += ReservationList[i].Size;
            }
            if (occupied <= capacity) return true;
            else return false;
        }
    }
}
