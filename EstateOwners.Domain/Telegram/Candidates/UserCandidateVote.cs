using System;
using System.Collections.Generic;
using System.Text;

namespace EstateOwners.Domain.Candidates
{
    public class UserCandidateVote
    {
        public string UserId { get; set; }
        public int CandidateId { get; set; }

        public UserCandidateVote(string userId, int candidateId)
        {
            UserId = userId;
            CandidateId = candidateId;
        }
    }
}
