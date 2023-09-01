using ConsoleApp5.Contracts;
using ConsoleApp5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp5.Services
{
    public class TerminalServices : ITerminalServices
    {
        public TerminalServices() { }

        public List<TerminalReceipt> GetDetails(string output)
        {
            throw new NotImplementedException();
        }
    }
}
