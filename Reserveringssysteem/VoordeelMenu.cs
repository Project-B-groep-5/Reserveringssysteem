namespace Reserveringssysteem
{
    public class VoordeelMenu
    {
        public string Name { get; set; }
        public Dish VoorGerecht { get; set; }
        public Dish HoofdGerecht { get; set; }
        public Dish NaGerecht { get; set; }
        public double Prijs { get; set; }

        public VoordeelMenu(string name, Dish voorGerecht, Dish hoofdGerecht, Dish naGerecht, double prijs)
        {
            Name = name;
            VoorGerecht = voorGerecht;
            HoofdGerecht = hoofdGerecht;
            NaGerecht = naGerecht;
            Prijs = prijs;
        }
    }
}
