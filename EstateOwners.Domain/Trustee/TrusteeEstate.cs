using System;
using System.ComponentModel.DataAnnotations;

namespace EstateOwners.Domain
{
    public class TrusteeEstate
    {
        public int TrusteeId { get; set; }

        [Required]
        public virtual Trustee Trustee { get; set; }

        public int EstateId { get; set; }

        [Required]
        public virtual Estate Estate { get; set; }

		public TrusteeEstate(int trusteeId, int estateId)
		{
			if (trusteeId <= 0)
				throw new ArgumentOutOfRangeException(nameof(trusteeId));

			if (estateId <= 0)
				throw new ArgumentOutOfRangeException(nameof(estateId));

			TrusteeId = trusteeId;
			EstateId = estateId;
		}

		public TrusteeEstate(Trustee trustee, Estate estate) : this(trustee.Id, estate.Id) { }

		public TrusteeEstate(ApplicationUser user, Estate estate) : this(user.TrusteeId, estate.Id) { }

		public TrusteeEstate(ApplicationUser user, int estateId) : this(user.TrusteeId, estateId) { }
	}
}
