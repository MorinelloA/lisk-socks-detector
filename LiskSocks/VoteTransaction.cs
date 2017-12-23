using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiskSocks
{
    public class Votes
    {
        public List<string> added { get; set; }
        public List<string> deleted { get; set; }
    }

    public class VTransaction
    {
        public string id { get; set; }
        public int height { get; set; }
        public string blockId { get; set; }
        public int type { get; set; }
        public int timestamp { get; set; }
        public string senderPublicKey { get; set; }
        public string senderId { get; set; }
        public string recipientId { get; set; }
        public string recipientPublicKey { get; set; }
        public int amount { get; set; }
        public int fee { get; set; }
        public string signature { get; set; }
        public List<object> signatures { get; set; }
        public int confirmations { get; set; }
        public Asset asset { get; set; }
        public Votes votes { get; set; }
    }

    public class VoteTransaction
    {
        public bool success { get; set; }
        public VTransaction transaction { get; set; }
    }
}
