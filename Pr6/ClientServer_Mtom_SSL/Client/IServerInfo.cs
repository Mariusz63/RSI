
using System.ServiceModel;

[ServiceContract]
public interface IServerInfo
{
    [OperationContract]
    string GetPersonalisedServerName(string name);

    [OperationContract]
    void UploadFile(FileUploadMessage request);
}
