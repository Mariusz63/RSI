using System;
using System.ServiceModel;
using System.ServiceModel.Description;

class Program
{
    static void Main()
    {
        Uri baseAddress = new Uri("http://localhost:8080/ws/server");

        using (ServiceHost host = new ServiceHost(typeof(ServerInfo), baseAddress))
        {
            var smb = new ServiceMetadataBehavior
            {
                HttpGetEnabled = true,
                HttpGetUrl = baseAddress
            };

            host.Description.Behaviors.Add(smb);

            host.AddServiceEndpoint(
                typeof(IServerInfo),
                new BasicHttpBinding(),
                ""
            );

            //host.AddServiceEndpoint(ServiceMetadataBehavior.MexContractName, MetadataExchangeBindings.CreateMexHttpBinding(), "mex");

            if (host.Description.Endpoints.Count > 0)
            {
                host.Description.Endpoints[0].EndpointBehaviors.Add(new ServerHandlerBehavior());

                host.Open();
                Console.WriteLine("Serwer dzia³a pod adresem: " + baseAddress);
                Console.WriteLine("Naciœnij ENTER, aby zakoñczyæ...");
                Console.ReadLine();
                host.Close();
            }
            else
            {
                Console.WriteLine("Brak endpointów!");
            }
        }
    }
}