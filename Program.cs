// See https://aka.ms/new-console-template for more information
using System.Net.Sockets;
using System.Net;
using System;
using System.Xml.Linq;
using System.Net.Http;
using System.Threading.Tasks;

public class Program
{
    public static void Main()
    {
        MainAsync().GetAwaiter().GetResult();
    }

    public static async Task MainAsync()
    {
        Console.WriteLine("IPWatcher 0.1");
        Console.WriteLine($"Write by Massimo Sacchetto/2025");

        // Read configuration from XML file
        (int checkInterval, string url, string subnet) = ReadConfiguration("config.xml");
        Console.WriteLine($"checkInterval: {checkInterval}");
        Console.WriteLine($"URL: {url}");
        string previousIP = GetLocalIPAddress();
        Console.WriteLine($"IP Address: {previousIP}");

        string IPZ = LoadNetworkInterfacesData();
        /*

        while (true)
        {
            string currentIP = GetLocalIPAddress();
            if (currentIP != previousIP)
            {
                Console.WriteLine($"New IP: {currentIP}");
                await FetchUrlOnIPChange(url);
                previousIP = currentIP;
            }
            Thread.Sleep(checkInterval); // Use the interval from the configuration file
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

    public static (int, string) ReadConfiguration(string filePath)
    {
        try
        {
            XDocument doc = XDocument.Load(filePath);
            XElement? configurationElement = doc.Element("configuration");
            if (configurationElement == null)
            {
                throw new Exception("Configuration element not found.");
            }

            XElement? settingsElement = configurationElement.Element("settings");
            if (settingsElement == null)
            {
                throw new Exception("Settings element not found.");
            }

            int checkInterval = int.Parse(settingsElement.Element("checkInterval")?.Value ?? throw new Exception("CheckInterval element not found."));
            string url = settingsElement.Element("url")?.Value ?? throw new Exception("URL element not found.");
            return (checkInterval, url);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading configuration: {ex.Message}");
            return (1000, "http://example.com"); // Default to 1 second and example URL if there's an error
        }
    }

    public static async Task FetchUrlOnIPChange(string url)
    {
        using HttpClient client = new HttpClient();
        try
        {
            HttpResponseMessage response = await client.GetAsync(url); // Use the URL from the configuration file
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Fetched URL content: {responseBody}");
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Request error: {e.Message}");
        }
    }
}
