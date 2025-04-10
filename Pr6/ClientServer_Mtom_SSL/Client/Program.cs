using System;
using System.IO;
using System.Net;
using System.ServiceModel;

namespace Client
{
    class Program
    {
        static void Main()
        {
            // Akceptujemy lokalny certyfikat bez podpisu (np. self-signed)
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            // Wymuszamy TLS 1.2
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var factory = new ChannelFactory<IServerInfo>("SecureEndpoint");
            var proxy = factory.CreateChannel();

            try
            {
                var response = proxy.GetPersonalisedServerName("Janek");
                Console.WriteLine("Odpowiedź serwera: " + response);

                Console.Write("Podaj nazwę pliku do wysłania: ");
                string fileName = Console.ReadLine();
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

                if (!File.Exists(filePath))
                {
                    Console.WriteLine("Plik nie istnieje.");
                }
                else
                {
                    var msg = new FileUploadMessage
                    {
                        FileName = fileName,
                        FileData = File.ReadAllBytes(filePath)
                    };
                    proxy.UploadFile(msg);
                    Console.WriteLine("Plik wysłany.");
                }

                ((IClientChannel)proxy).Close();
                factory.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Błąd: " + ex.Message);
            }
        }
    }
}
