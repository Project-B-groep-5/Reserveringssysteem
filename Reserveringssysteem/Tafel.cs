namespace Reserveringssysteem
{
    public class Tafel
    {
        public int TafelId { get; set; }
        public int Size { get; set; }
        public Klant Customer { get; set; }
        public Rekening Bill { get; set; }

        public Tafel(int tafelId, int size, Klant customer, Rekening bill)
        {
            TafelId = tafelId;
            Size = size;
            Customer = customer;
            Bill = bill;
        }
    }
}