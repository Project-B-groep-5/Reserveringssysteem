namespace Reserveringssysteem
{
    public class Customer
    {
        public string Name { get; set; }
        public string Id { get; set; }

        public Customer(string name, string id = null)
        {
            Id = id;
            Name = name;
        }
        private Customer()
        {

        }
    }
}