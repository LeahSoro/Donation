using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimchaDonors.Data
{
   public class DonateToSimcha
    {
        public int Simchaid { get; set; }
        public int Contributorid { get; set; }
        public Decimal Amount { get; set; }
       
        public bool Include { get; set; }
    }
}
