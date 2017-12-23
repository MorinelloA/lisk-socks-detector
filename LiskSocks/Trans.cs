using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiskSocks
{

    public class Trans
    {
        public bool success { get; set; }
        public List<Transaction> transactions { get; set; }
        public long count { get; set; }
    }
}
