namespace Reserveringssysteem
{
    public class Tafel
    {
        public int TafelId { get; set; }
        public int Size { get; set; }
        public Customer Customer { get; set; }
        public Bill Bill { get; set; }

        public Tafel(int tafelId, int size, Customer customer, Bill bill)
        {
            TafelId = tafelId;
            Size = size;
            Customer = customer;
            Bill = bill;
        }
    }
}