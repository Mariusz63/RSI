
package rsi.org.helloworld;

import jakarta.jws.WebMethod;
import jakarta.jws.WebParam;
import jakarta.jws.WebResult;
import jakarta.jws.WebService;
import jakarta.xml.bind.annotation.XmlSeeAlso;
import jakarta.xml.ws.Action;
import jakarta.xml.ws.RequestWrapper;
import jakarta.xml.ws.ResponseWrapper;


/**
 * This class was generated by the JAX-WS RI.
 * JAX-WS RI 2.2.9-b130926.1035
 * Generated source version: 2.2
 * 
 */
@WebService(name = "HelloWorld", targetNamespace = "http://helloworld.org.rsi/")
@XmlSeeAlso({
    ObjectFactory.class
})
public interface HelloWorld {


    /**
     * 
     * @param arg0
     * @return
     *     returns java.lang.String
     */
    @WebMethod
    @WebResult(targetNamespace = "")
    @RequestWrapper(localName = "getHelloWorldAsString", targetNamespace = "http://helloworld.org.rsi/", className = "rsi.org.helloworld.GetHelloWorldAsString")
    @ResponseWrapper(localName = "getHelloWorldAsStringResponse", targetNamespace = "http://helloworld.org.rsi/", className = "rsi.org.helloworld.GetHelloWorldAsStringResponse")
    @Action(input = "http://helloworld.org.rsi/HelloWorld/getHelloWorldAsStringRequest", output = "http://helloworld.org.rsi/HelloWorld/getHelloWorldAsStringResponse")
    public String getHelloWorldAsString(
        @WebParam(name = "arg0", targetNamespace = "")
        String arg0);

}
