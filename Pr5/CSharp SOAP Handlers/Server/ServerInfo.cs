public class ServerInfo : IServerInfo
{
    public string GetPersonalisedServerName(string name)
    {
        return $"Welcome to {name}'s server!";
    }
}