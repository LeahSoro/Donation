using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimchaDonors.Data
{
    public class Contribution
    {
        public int id { get; set; }
        public int Contributorid { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
       
    }
}
