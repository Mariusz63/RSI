using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Uri baseAddress = new Uri("http://localhost:8080/Server");

            var binding = new BasicHttpBinding
            {
                MessageEncoding = WSMessageEncoding.Mtom,
                Security = { Mode = BasicHttpSecurityMode.None },
                MaxReceivedMessageSize = 10_000_000
            };

            var host = new ServiceHost(typeof(ServerInfo), baseAddress);
            host.AddServiceEndpoint(typeof(IServerInfo), binding, "");
            host.Description.Behaviors.Add(new ServiceMetadataBehavior { HttpGetEnabled = true });
            host.Open();

            Console.WriteLine("Serwer działa pod adresem: " + baseAddress);
            Console.ReadLine();
        }
    }
}
