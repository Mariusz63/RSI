using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using CinemaServer.Models;
using CinemaServer.Service;
using CinemaServer.Utils;

namespace CinemaServer
{
    class Program
    {
        static void Main(string[] args)
        {
            // Akceptujemy certyfikaty SSL bez weryfikacji (dla testów lokalnych)
            ServicePointManager.ServerCertificateValidationCallback += (s, cert, chain, sslErrors) => true;

            // Tworzymy i uruchamiamy host serwisu
            using (var host = new ServiceHost(typeof(CinemaServer.Service.CinemaReservationService)))
            {
                try
                {
                    host.Open();
                    Console.WriteLine("Serwer działa pod adresem: https://localhost:8443/Server");
                    Console.WriteLine("Naciśnij Enter, aby zatrzymać serwer...");
                    Console.ReadLine();
                    host.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Błąd uruchamiania serwera: " + ex.Message);
                }
            }
        }
    }
}
