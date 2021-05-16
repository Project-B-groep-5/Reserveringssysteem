namespace Reserveringssysteem
{
    class Restaurant
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Location Address { get; set; }
        public int Capacity { get; }
        public string[] OpeningHours { get; set; }
        public string[] ContactInformation { get; set; }

        public Restaurant(string name, Location address, int capacity, string[] openingHours, string[] contactInformation)
        {
            Name = name;
            Description =
                $@"Welkom bij {name}. 
Wij bezorgen u een lach met de lekkerste gerechten.
Wij bereiden de lekkerste vlees en vegetarische gerechten die u terug kunt vinden op de menukaart.";
            Address = address;
            Capacity = capacity;
            OpeningHours = openingHours;
            ContactInformation = contactInformation;
        }
    }
}
