using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

public class ClientHandlerBehavior : IEndpointBehavior
{
    private readonly string _username;
    private readonly string _password;

    public ClientHandlerBehavior(string username, string password)
    {
        _username = username;
        _password = password;
    }

    public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
    {
        clientRuntime.ClientMessageInspectors.Add(new ClientHandler(_username, _password));
    }

    public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters) { }
    public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher) {}
    public void Validate(ServiceEndpoint endpoint) {}
}