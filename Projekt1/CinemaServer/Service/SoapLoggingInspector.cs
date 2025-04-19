using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CinemaServer.Service
{
    public class SoapLoggingInspector : IDispatchMessageInspector
    {
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            string xml = request.ToString();
            File.WriteAllText("soap_request.xml", xml);
            return null;
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            string xml = reply.ToString();
            File.WriteAllText("soap_response.xml", xml);
        }
    }

}
