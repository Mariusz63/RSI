using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main()
        {
            var binding = new BasicHttpBinding();
            binding.MessageEncoding = WSMessageEncoding.Mtom;
            binding.Security.Mode = BasicHttpSecurityMode.None;
            binding.MaxReceivedMessageSize = 10_000_000;

            var address = new EndpointAddress("http://localhost:8080/Server");

            var factory = new ChannelFactory<IServerInfo>(binding, address);
            var proxy = factory.CreateChannel();

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
    }
}
