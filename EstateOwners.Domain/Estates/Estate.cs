using System;
using Lers.Utils;

namespace EstateOwners.Domain
{
    public class Estate
    {
        public int Id { get; set; }

        public EstateType Type { get; set; }

        public string Number { get; set; }

        public int BuildingId { get; set; }

        public virtual Building Building { get; set; }

        public float Area { get; set; }

        public Estate(EstateType type, int buildingId, string number)
        {
            Type = type;
            Number = number;
            BuildingId = buildingId;
        }

        public override string ToString()
        {
            return $"{Building.ShortAddress}, {Type.GetDescription().ToLower()} {Number}, площадью {Area}";
        }
    }
}
