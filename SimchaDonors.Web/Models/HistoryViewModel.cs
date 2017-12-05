using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SimchaDonors.Data;

namespace SimchaDonors.Web.Models
{
    public class HistoryViewModel
    {
        //public IEnumerable<Contributor> contributors {get; set;}
        public IEnumerable<Contribution> contributions { get; set; }
        public Contributor contributor { get; set; }
    }
}