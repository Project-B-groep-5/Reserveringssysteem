namespace Reserveringssysteem
{
    public class Customer
    {
        public string Name { get; set; }
        public string Id { get; set; }

        public Customer(string name)
        {
            Id = name + "_101";
            Name = name;
        }
    }
}