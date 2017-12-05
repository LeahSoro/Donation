using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimchaDonors.Data
{
   public class Simcha
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public int Id { get; set; }
        public int Count { get; set; }
        public Decimal Balance { get; set; }
    }
}
