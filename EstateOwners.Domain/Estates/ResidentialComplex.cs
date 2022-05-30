using System;
using System.Collections.Generic;
using System.Text;

namespace EstateOwners.Domain
{
    public class ResidentialComplex
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Address { get; set; }

        public ResidentialComplex(string title, string address)
        {
            Title = title;
            Address = address;
        }
    }
}
