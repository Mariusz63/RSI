
using System.Runtime.Serialization;
using System.ServiceModel;

[MessageContract]
public class FileUploadMessage
{
    [MessageHeader]
    public string FileName;

    [MessageBodyMember]
    public byte[] FileData;
}
