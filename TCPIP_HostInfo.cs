// TCT_IP_Test.cpp : Defines the entry point for the console application.
//


using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System; // For String and Console
using System.Net; // For Dns, IPHostEntry, IPAddress
using System.Net.Sockets; // For SocketException


    class IPAddressExample 
    {
        static void PrintHostInfo(String host) 
        {
            try 
            {
                IPHostEntry hostInfo;

                // Attempt to resolve DNS for given host or address
                hostInfo = Dns.GetHostEntry(host); //Dns.Resolve(host);
                // Display the primary host name

                Console.WriteLine("\tCanonical Name: " + hostInfo.HostName);

                // Display list of IP addresses for this host
                Console.Write("\tIP Addresses: ");
                foreach (IPAddress ipaddr in hostInfo.AddressList) {  Console.Write("\n\t\t" + ipaddr.ToString()); }
            
                Console.WriteLine();

                // Display list of alias names for this host
                Console.Write("\tAliases: ");
                foreach (String alias in hostInfo.Aliases) {    Console.Write("\n\t\t" + alias);  }

                Console.WriteLine("\n");
            } 
            catch (Exception) 
            {
                Console.WriteLine("\tUnable to resolve host:" + host + "\n");
            }
        }


    static void Main(string[] args) 
    {
        // Get and print local host info
        try 
        {
            Console.WriteLine("Local Host:");
            String localHostName = Dns.GetHostName();
            Console.WriteLine("\tHost Name: " + localHostName);
            PrintHostInfo(localHostName);
        } 
        catch (Exception) 
        {
            Console.WriteLine("Unable to resolve local host\n");
        }

        // Get and print info for hosts given on command line
        foreach (String arg in args) 
        {
            Console.WriteLine(arg + ":");
            PrintHostInfo(arg);
        }

        
        Console.WriteLine( "\n\nPress any key");
        Console.ReadLine(); 
    }
}
//namespace ConsoleApplication2
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//        }
//    }
//}
