using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiskSocks
{
    public class Asset
    {
    }

    public class Transaction
    {
        public string id { get; set; }
        public long height { get; set; }
        public string blockId { get; set; }
        public int type { get; set; }
        public long timestamp { get; set; }
        public string senderPublicKey { get; set; }
        public string senderId { get; set; }
        public string recipientId { get; set; }
        public string recipientPublicKey { get; set; }
        public object amount { get; set; }
        public long fee { get; set; }
        public string signature { get; set; }
        public List<object> signatures { get; set; }
        public long confirmations { get; set; }
        public Asset asset { get; set; }
    }
}
