namespace EstateOwners.Domain
{
    public class ApprovedModel
    {
        public string DocumentTitle{ get; set; }

        public bool ApprovedByDocument { get; set; }

        public bool ApprovedByUser { get; set; }

        public bool ApprovedByMeet { get; set; }

        public bool Aproved => ApprovedByDocument | ApprovedByUser | ApprovedByMeet;
    }
}
