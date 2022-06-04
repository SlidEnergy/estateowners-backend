using System;
using System.Collections.Generic;
using System.Text;

namespace EstateOwners.Domain
{
    public class Candidate
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public CandidateType Type { get; set; }

        public Candidate(string userId, CandidateType type)
        {
            UserId = userId;
            Type = type;
        }

        public override string ToString()
        {
            if(Type == CandidateType.Chairman)
                return $"Председатель {User}";
            else
                return $"Член совета дома {User}";
        }
    }
}
