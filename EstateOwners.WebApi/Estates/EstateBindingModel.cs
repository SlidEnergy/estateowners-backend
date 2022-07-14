using EstateOwners.Domain;

namespace EstateOwners.WebApi.Dto
{
    public class EstateBindingModel
    {
        public EstateType Type { get; set; }

        public string Number { get; set; }

        public int BuildingId { get; set; }

        public float Area { get; set; }
    }
}
