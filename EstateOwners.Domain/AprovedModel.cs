namespace EstateOwners.Domain
{
    public class ApprovedModel
    {
        public bool ApprovedByDocument { get; set; }
        public string DocumentTitle{ get; set; }

        public bool ApprovedByUser { get; set; }
        public string ApproverUserName { get; set; }
        public string ApproveCircumstances { get; set; }

        public bool Aproved => ApprovedByDocument | ApprovedByUser;
    }
}
