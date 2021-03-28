namespace Reserveringssysteem
{
    class Restaurant
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string[] Adress { get; set; }
        public string[] OpeningHours { get; set; }
        public string[] ContactInformation { get; set; }

        public Restaurant(string name, string description, string[] adress, string[] openingHours, string[] contactInformation)
        {
            Name = name;
            Description = description;
            Adress = adress;
            OpeningHours = openingHours;
            ContactInformation = contactInformation;
        }
    }
}
