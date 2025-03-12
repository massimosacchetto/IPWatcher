// See https://aka.ms/new-console-template for more information
using System.Net.Sockets;
using System.Net;
using System;


public class Program
{
    public static void Main()
    {
        Console.WriteLine("IPWatcher 0.1");
        Console.WriteLine($"Write by Massimo Sacchetto/2025");


        string previousIP = GetLocalIPAddress();
        Console.WriteLine($"IP Address: {previousIP}");

        while (true)
        {
            string currentIP = GetLocalIPAddress();
            if (currentIP != previousIP)
            {
                Console.WriteLine($"New IP: {currentIP}");
                previousIP = currentIP;
            }
            Thread.Sleep(1000); // Pausa di 1 secondo
        }
    }
    public static string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        throw new Exception("Nessun indirizzo IPv4 trovato per la macchina.");
    }
}
