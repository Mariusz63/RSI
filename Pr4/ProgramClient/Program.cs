using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgramClient.HelloWorldService;



namespace ProgramClient
{
    class ProgramClient
    {
        static void Main()
        {
            // Utwórz klienta web serwisu
            var client = new HelloWorldImplServiceClient();

            // Wywołaj metodę getHelloWorldAsString
            string response = client.getHelloWorldAsString("C# Klient");
            Console.WriteLine("Odpowiedź: " + response);

            // Pobierz listę produktów
            var products = client.getProducts();
            Console.WriteLine("Lista produktów:");

            foreach (var p in products)
            {
                Console.WriteLine($"- {p.nazwa}: {p.opis}, Cena: {p.cena} zł");
            }

            // Zamknij klienta
            client.Close();
        }
    }
}
