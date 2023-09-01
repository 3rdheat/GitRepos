using ConsoleApp5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp5.Contracts
{
    public interface IOcrService
    {
        string ExtractTextFromImg(string path);
    }
}
