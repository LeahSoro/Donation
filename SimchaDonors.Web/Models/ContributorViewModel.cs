using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SimchaDonors.Data;

namespace SimchaDonors.Web.Models
{
    public class ContributorViewModel
    {
        public IEnumerable<Contributor> contributors { get; set; }
        public Decimal TotalDonations { get; set; }
        public int count { get; set; }
        public int simchaid { get; set; }
        public List<DonateToSimcha> Donated { get; set; }
     }
}