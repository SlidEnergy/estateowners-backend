using System;

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
            var type = "";

            switch (Type)
            {
                case EstateType.Apartment:
                    type = "Апартамент";
                    break;
                case EstateType.CommercialRoom:
                    type = "Коммерческое помещение";
                    break;
                case EstateType.ParkingSpace:
                    type = "Парковочное место";
                    break;
                case EstateType.Storeroom:
                    type = "Кладовка";
                    break;
                default:
                    throw new Exception("Данный тип недвижимости не поддерживается: " + Type);
            }

            return $"{Building.ShortAddress}, {type} {Number}";
        }
    }
}
