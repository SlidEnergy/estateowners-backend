using System;
using System.ComponentModel.DataAnnotations;

namespace EstateOwners.Domain
{
    public class TrusteeCar
    {
        public int TrusteeId { get; set; }

        [Required]
        public virtual Trustee Trustee { get; set; }

        public int CarId { get; set; }

        [Required]
        public virtual Car Car { get; set; }

		public TrusteeCar(int trusteeId, int carId)
		{
			if (trusteeId <= 0)
				throw new ArgumentOutOfRangeException(nameof(trusteeId));

			if (carId <= 0)
				throw new ArgumentOutOfRangeException(nameof(carId));

			TrusteeId = trusteeId;
			CarId = carId;
		}

		public TrusteeCar(Trustee trustee, Car car)
		{
			Trustee = trustee;
			Car = car;
		}

		public TrusteeCar(ApplicationUser user, Car car)
		{
			TrusteeId = user.TrusteeId;
			Car = car;
		}

		public TrusteeCar(ApplicationUser user, int carId) : this(user.TrusteeId, carId) { }
	}
}
