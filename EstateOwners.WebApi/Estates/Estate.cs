using EstateOwners.Domain;

namespace EstateOwners.WebApi.Dto
{
    public class Estate
    {
        public int Id { get; set; }

        public EstateType Type { get; set; }

        public string Number { get; set; }

        public int BuildingId { get; set; }
        public string Building { get; set; }

        public float Area { get; set; }
    }
}
