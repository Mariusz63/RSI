using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

public class ServerHandler : IDispatchMessageInspector
{
    // PROPER macAddress
    private readonly string _allowedMac = "0A0027000006";

    // BAD macAddress
    //private readonly string _allowedMac = "0A0027000003";

    public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
    {
        // ÆW.1 - adres MAC

        int index = request.Headers.FindHeader("macAddress", "http://tempuri.org");

        if (index == -1)
        {
            throw new FaultException("Brak naglówka macAddress.");
        }

        string clientMac = request.Headers.GetHeader<string>(index);
        Console.WriteLine($"Otrzymano MAC klienta: {clientMac}");

        if (clientMac != _allowedMac)
        {
            var reason = new FaultReason("MAC klienta nieautoryzowany.");
            var code = new FaultCode("Client.AuthFailure.MacAddress");

            throw new FaultException(reason, code);
        }

        // ÆW.2 - autentykacja

        string username = null;
        string password = null;

        int usernameIndex = request.Headers.FindHeader("Username", "http://tempuri.org");
        int passwordIndex = request.Headers.FindHeader("Password", "http://tempuri.org");

        if (usernameIndex >= 0)
        {
            username = request.Headers.GetHeader<string>(usernameIndex);
        }

        if (passwordIndex >= 0)
        {
            password = request.Headers.GetHeader<string>(passwordIndex);

        }

        Console.WriteLine($"SERVER: Received request: \n {request} \n");

        if (username != "admin" || password != "password")
        {
            var reason = new FaultReason("Dane autentykacyjne nieprawidlowe.");
            var code = new FaultCode("Client.AuthFailure.Credentials");

            Console.WriteLine("Odrzucono dane uzytkownika.");
            throw new FaultException(reason, code);
        }

        return "Success";
    }

    public void BeforeSendReply(ref Message reply, object correlationState)
    {
        if (correlationState is string status && status == "Success")
        {
            var header = MessageHeader.CreateHeader("AuthStatus", "http://tempuri.org", "Success");
            reply.Headers.Add(header);
        }
    }
}