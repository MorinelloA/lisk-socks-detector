using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiskSocks
{

    public class Meta
    {
        public int offset { get; set; }
        public int limit { get; set; }
        public int count { get; set; }
    }

    public class Asset
    {
        public string data { get; set; }
        public List<string> votes { get; set; }
    }

    public class Transaction
    {
        public string id { get; set; }
        public int height { get; set; }
        public string blockId { get; set; }
        public int type { get; set; }
        public int timestamp { get; set; }
        public string senderPublicKey { get; set; }
        public string recipientPublicKey { get; set; }
        public string senderId { get; set; }
        public string recipientId { get; set; }
        public string amount { get; set; }
        public string fee { get; set; }
        public string signature { get; set; }
        public List<object> signatures { get; set; }
        public Asset asset { get; set; }
        public int confirmations { get; set; }
    }

    public class Links
    {
    }

    public class Transactions
    {
        public Meta meta { get; set; }
        public List<Transaction> data { get; set; }
        public Links links { get; set; }
    }
}
