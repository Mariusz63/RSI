using System.ServiceModel.Dispatcher;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System;
using System.Net.NetworkInformation;
using System.Linq;

public class ClientHandler : IClientMessageInspector
{
    private readonly string _username;
    private readonly string _password;

    public ClientHandler(string username, string password)
    {
        _username = username;
        _password = password;
    }

    public object BeforeSendRequest(ref Message request, IClientChannel channel)
    {
        // ÆW.1 - adres MAC
        var macAddress = GetMacAddress();
        var header = MessageHeader.CreateHeader("macAddress", "http://tempuri.org", macAddress);
        request.Headers.Add(header);

        // ÆW.2 - autentykacja
        var usernameHeader = MessageHeader.CreateHeader("Username", "http://tempuri.org", _username);
        var passwordHeader = MessageHeader.CreateHeader("Password", "http://tempuri.org", _password);

        request.Headers.Add(usernameHeader);
        request.Headers.Add(passwordHeader);

        return null;
    }

    public void AfterReceiveReply(ref Message reply, object correlationState)
    {
        Console.WriteLine($"\nCLIENT: Received reply:\n {reply} \n");
    }

    private object GetMacAddress()
    {
        return NetworkInterface
            .GetAllNetworkInterfaces()
            .FirstOrDefault(nic => nic.OperationalStatus == OperationalStatus.Up)
            ?.GetPhysicalAddress()
            .ToString() ?? "UNKNOWN";
    }
}