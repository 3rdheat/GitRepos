using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp5.Models
{
    public class TerminalReceipt
    {
        public string MerchantID { get; set; }
        public string CardType { get; set; }
        public int CardNumber { get; set; }
        public DateTime TranDateTime { get; set;}
        public decimal Amount { get; set; }
    }
}
