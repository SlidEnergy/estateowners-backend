namespace EstateOwners.Domain
{
    public class Car
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Number { get; set; }

        public string Color { get; set; }

        public Car()
        {

        }

        public Car(string number)
        {
            Number = number;
        }

        public override string ToString()
        {
            return $"{Title} ({Color}) - {Number}";
        }
    }
}
