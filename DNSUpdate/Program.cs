using PH.ConsoleHelper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PH.DNSUpdate
{
    class Program
    {

        static DNSUpdater DNSUpdater { get; set; } = new DNSUpdater();

        static int Main(string[] args)
        {
            using var console = new ConsoleLogger("dnsUpdater.log");
            Console.WriteLine("Checking current IP ...");
            Task task = IPChecker.UpdateIP();
            while (!task.IsCompleted) { }
            Console.WriteLine("IP updated. Current IP: {0}", IPChecker.IP);

            Console.WriteLine();
            Console.WriteLine("Fetching DNS info from DNS provider ...");

            if (DNSUpdater.GetDNSInfo())
            {
                PrintRecords(DNSUpdater.Records);
                if (DNSUpdater.Records.Count == 0)
                {
                    Console.WriteLine("No DNS record finded");
                }
                else if (DNSUpdater.Records.Count > 1)
                {
                    Console.WriteLine("Auto-selected DNS id: {0}, base on the perfered domain setting: \"{1}\" in dnsUpdater.conf", DNSUpdater.Profile.DnsID, DNSUpdater.Profile.PreferedDomain);
                }
                else
                {
                    Console.WriteLine("Auto-selected DNS id: {0}", DNSUpdater.Profile.DnsID);
                }
            }
            else
            {
                Console.WriteLine("Fail to fetch DNS record, program exiting...");
                return 1;
            }

            if (DNSUpdater.UpdateDNS(IPChecker.IP))
            {
                Console.WriteLine("DNS record updated, Current record:");
                var list = new List<DnsRecord>();
                list.Add(DNSUpdater.UpdatedRecord);
                PrintRecords(list);
            }
            else
            {
                Console.WriteLine("Fail to update DNS record, program exiting...");
                return 1;
            }

            
            return 0;
        }

        public static void PrintRecords(List<DnsRecord> recoeds)
        {
            ConsoleHelper.PrintLine();
            ConsoleHelper.PrintRow("Type","Name","Content","ID");
            foreach (var recoed in recoeds)
            {
                ConsoleHelper.PrintRow(recoed.Type, recoed.Name, recoed.Content, recoed.ID);
            }
            ConsoleHelper.PrintLine();
        }
    }
}
