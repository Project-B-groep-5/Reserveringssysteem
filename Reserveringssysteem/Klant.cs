namespace Reserveringssysteem
{
    public class Klant
    {
        public string Naam { get; set; }
        public string Id { get; set; }

        public Klant(string naam)
        {
            Id = naam + "_101";
            Naam = naam;
        }
    }
}