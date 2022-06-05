using System.ComponentModel;

namespace EstateOwners.Domain
{
    public enum EstateType
    {
        [Description("Апартамент")]
        Apartment,
        [Description("Парковочное место")]
        ParkingSpace,
        [Description("Кладовка")]
        Storeroom,
        [Description("Коммерческое помещение")]
        CommercialRoom
    }
}
