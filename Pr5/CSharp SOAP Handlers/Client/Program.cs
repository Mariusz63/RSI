using System;
using System.ServiceModel;

[ServiceContract]
public interface IServerInfo
{
    [OperationContract]
    string GetPersonalisedServerName(string name);
}

class Program
{
    static void Main()
    {
        while (true)
        {
            Console.Write("Username: ");
            string username = Console.ReadLine();

            Console.Write("Password: ");
            string password = Console.ReadLine();

            var factory = new ChannelFactory<IServerInfo>(
                new BasicHttpBinding(),
                new EndpointAddress("http://localhost:8080/ws/server"));

            factory.Endpoint.EndpointBehaviors.Add(new ClientHandlerBehavior(username, password));
            var proxy = factory.CreateChannel();

            try
            {
                string response = proxy.GetPersonalisedServerName(username);
                Console.WriteLine("Odpowiedź serwera: " + response);
            }
            catch (FaultException ex)
            {
                Console.WriteLine("Serwer odrzucił żądanie:");
                Console.WriteLine("" + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Błąd: " + ex.Message);
            }
        }
    }
}