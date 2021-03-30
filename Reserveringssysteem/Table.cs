namespace Reserveringssysteem
{
    public class Table
    {
        public string TableId { get; set; }
        public int Size { get; set; }
        public Customer Customer { get; set; }
        public Bill Bill { get; set; }

        public Table(string tableId, int size, Customer customer, Bill bill)
        {
            TableId = tableId;
            Size = size;
            Customer = customer;
            Bill = bill;
        }
        private Table()
        {

        }
    }
}