using System;
using System.Net;
using System.ServiceModel;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            ServicePointManager.ServerCertificateValidationCallback += (s, cert, chain, sslErrors) => true;

            using (var host = new ServiceHost(typeof(ServerInfo)))
            {
                host.Open();
                Console.WriteLine("Serwer działa pod adresem: https://localhost:8443/Server");
                Console.ReadLine();
            }
        }
    }
}
