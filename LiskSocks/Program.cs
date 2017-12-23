using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LiskSocks
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Make sure all .txt files associated with this program are closed at this time. Otherwise, the program will crash");
            //seconds range to determine who is a sock
            bool moveOn = true;
            int range = 300;
            do
            {
                moveOn = true;
                Console.WriteLine("Enter the range in seconds");
                try
                {
                    range = Convert.ToInt32(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("You must enter a whole number");
                    moveOn = false;
                }
            } while (!moveOn);
            int offset = 0;

            //string publicNode = "https://wallet.lisknode.io/";
            string publicNode = "https://wallet.mylisk.com/";

            Dictionary<string, string> addresses = new Dictionary<string, string>();

            var lines = File.ReadLines("addresses.txt");
            foreach (var line in lines)
            {
                string[] temp = line.Split(' ');
                addresses.Add(temp[1].Trim(), temp[0].Trim());
            }
            List<string> transactions = new List<string>();
            List<Transaction> votingTrans = new List<Transaction>();

            Console.WriteLine("Addresses gathered");
            Console.WriteLine("Gathering transactions for each address");

            //foreach (string address in addresses)
            foreach (KeyValuePair<string, string> address in addresses)
            {
                Console.WriteLine("Starting " + address.Value + ": " + address.Key);
                offset = 0;
                bool isDone = false;
                do
                {
                    string query = "api/transactions?&senderId=" + address.Key + "&limit=1000&offset=" + offset + "&orderBy=timestamp:desc";
                    
                    //Without sorting, duplicate transactions are gathered
                    //string query = "api/transactions?&senderId=" + address.Key + "&limit=1000&offset=" + offset;

                    using (WebClient wc = new WebClient())
                    {
                        string temp = wc.DownloadString(publicNode + query);
                        transactions.Add(temp);
                        Trans tempTransaction = JsonConvert.DeserializeObject<Trans>(temp);
                        if (tempTransaction.transactions.Count >= 1000)
                        {
                            offset += 1000;
                        }
                        else
                        {
                            isDone = true;
                            Console.WriteLine(address.Value + ": " + address.Key + " - DONE");
                        }
                    }
                } while (!isDone); //transaction = 1000
            }
            Console.WriteLine("Done with all addresses");
            File.WriteAllText("SameVotes.txt", string.Empty);
            File.WriteAllText("DifferentVotes.txt", string.Empty);

            foreach (string str in transactions)
            {

                Trans result = JsonConvert.DeserializeObject<Trans>(str);
                foreach(Transaction tran in result.transactions)
                {
                    if(tran.type == 3)
                    {
                        votingTrans.Add(tran);
                    }
                }
                // Open the file to read from.
                //string readText = File.ReadAllText(path);
            }

            Console.WriteLine("Transactions seperated by type");
            Console.WriteLine("Looking for possible socks");

            for(int i = 0; i < votingTrans.Count; i++)
            {
                for (int j = 0; j < votingTrans.Count; j++)
                {
                    if(i < j)
                    {
                        if(votingTrans[i].timestamp - votingTrans[j].timestamp <= range && votingTrans[i].timestamp - votingTrans[j].timestamp > 0)
                        {
                            if (votingTrans[i].senderId != votingTrans[j].senderId)
                            {
                                if (addresses[votingTrans[i].senderId] != addresses[votingTrans[j].senderId])
                                {
                                    //api / transactions / get ? id = 2715735863990084187
                                    string query1 = "api/transactions/get?id=" + votingTrans[i].id;
                                    string query2 = "api/transactions/get?id=" + votingTrans[j].id;

                                    //Without sorting, duplicate transactions are gathered
                                    //string query = "api/transactions?&senderId=" + address.Key + "&limit=1000&offset=" + offset;

                                    using (WebClient wc = new WebClient())
                                    {
                                        string temp1 = wc.DownloadString(publicNode + query1);
                                        string temp2 = wc.DownloadString(publicNode + query2);

                                        VoteTransaction tempTransaction1 = JsonConvert.DeserializeObject<VoteTransaction>(temp1);
                                        VoteTransaction tempTransaction2 = JsonConvert.DeserializeObject<VoteTransaction>(temp2);

                                        //ints1.All(ints2.Contains) && ints1.Count == ints2.Count;
                                        if (tempTransaction1.transaction.votes.added.All(tempTransaction2.transaction.votes.added.Contains) && tempTransaction1.transaction.votes.added.Count == tempTransaction2.transaction.votes.added.Count && tempTransaction1.transaction.votes.added.Count > 0)
                                        {
                                            Console.WriteLine("Same vote match found - " + addresses[votingTrans[i].senderId] + " & " + addresses[votingTrans[j].senderId]);
                                            File.AppendAllText("SameVotes.txt", addresses[votingTrans[i].senderId] + ": " + votingTrans[i].id + " & " + addresses[votingTrans[j].senderId] + ": " + votingTrans[j].id + Environment.NewLine);
                                        }
                                        else
                                        {
                                            Console.WriteLine("Different vote match found - " + addresses[votingTrans[i].senderId] + " & " + addresses[votingTrans[j].senderId]);
                                            File.AppendAllText("DifferentVotes.txt", addresses[votingTrans[i].senderId] + ": " + votingTrans[i].id + " & " + addresses[votingTrans[j].senderId] + ": " + votingTrans[j].id + Environment.NewLine);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            Console.WriteLine("COMPLETE. Check .txt files for data");
            Console.WriteLine("Press enter to close...");
            Console.ReadLine();
        }
    }
}