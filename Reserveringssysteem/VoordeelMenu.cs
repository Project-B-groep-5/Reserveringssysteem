using System;
using System.Collections.Generic;
using System.Text;

namespace Reserveringssysteem
{
    public class VoordeelMenu
    {
        public Dish VoorGerecht { get; set; }
        public Dish HoofdGerecht { get; set; }
        public Dish NaGerecht { get; set; }
        public double Prijs { get; set; }

        public VoordeelMenu(Dish voorGerecht, Dish hoofdGerecht, Dish naGerecht, double prijs)
        {
            VoorGerecht = voorGerecht;
            HoofdGerecht = hoofdGerecht;
            NaGerecht = naGerecht;
            Prijs = prijs;
        }
    }
}
