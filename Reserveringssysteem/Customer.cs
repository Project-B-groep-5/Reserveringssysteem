namespace Reserveringssysteem
{
    public class Customer
    {
        public string Name { get; set; }
        public string Id { get; set; }

        public Customer(string name, string id = null)
        {
            if (id == null)
            {
                Id = name + "_101";
            }
            else
            {
                Id = id;
            }
            Name = name;
        }
    }
}