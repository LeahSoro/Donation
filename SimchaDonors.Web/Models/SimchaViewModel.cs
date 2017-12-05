using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SimchaDonors.Data;

namespace SimchaDonors.Web.Models
{
    public class SimchaViewModel
    {
        public IEnumerable<Simcha> simchas { get; set; }
        public int ContributorCount { get; set; }
        
    }
}