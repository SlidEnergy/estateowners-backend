namespace EstateOwners.Domain
{
    public class Building
    {
        public int Id { get; set; }

        public int ResidentialComplexId { get; set; }

        public string ShortAddress { get; set; }

        public string Address { get; set; }

        public Building(int residentialComplexId, string shortAddress, string address)
        {
            ResidentialComplexId = residentialComplexId;
            ShortAddress = shortAddress;
            Address = address;
        }
    }
}
