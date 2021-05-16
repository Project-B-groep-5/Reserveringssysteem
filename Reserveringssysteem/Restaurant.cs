namespace Reserveringssysteem
{
    class Restaurant
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Location Address { get; set; }
        public string[] OpeningHours { get; set; }
        public string[] ContactInformation { get; set; }

        public Restaurant(string name, string description, Location address, string[] openingHours, string[] contactInformation)
        {
            Name = name;
            Description =
                $@"Welkom bij {name}. 
Wij bezorgen u een lach met de lekkerste gerechten.
Wij bereiden de lekkerste vlees en vegetarische gerechten die u terug kunt vinden op de menukaart.";
            Address = address;
            OpeningHours = openingHours;
            ContactInformation = contactInformation;
        }
    }
}
