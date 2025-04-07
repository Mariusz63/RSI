
package rsi.org.helloworld;

import jakarta.xml.bind.JAXBElement;
import jakarta.xml.bind.annotation.XmlElementDecl;
import jakarta.xml.bind.annotation.XmlRegistry;
import javax.xml.namespace.QName;


/**
 * This object contains factory methods for each 
 * Java content interface and Java element interface 
 * generated in the rsi.org.helloworld package. 
 * <p>An ObjectFactory allows you to programatically 
 * construct new instances of the Java representation 
 * for XML content. The Java representation of XML 
 * content can consist of schema derived interfaces 
 * and classes representing the binding of schema 
 * type definitions, element declarations and model 
 * groups.  Factory methods for each of these are 
 * provided in this class.
 * 
 */
@XmlRegistry
public class ObjectFactory {

    private final static QName _GetHelloWorldAsString_QNAME = new QName("http://helloworld.org.rsi/", "getHelloWorldAsString");
    private final static QName _GetHelloWorldAsStringResponse_QNAME = new QName("http://helloworld.org.rsi/", "getHelloWorldAsStringResponse");

    /**
     * Create a new ObjectFactory that can be used to create new instances of schema derived classes for package: rsi.org.helloworld
     * 
     */
    public ObjectFactory() {
    }

    /**
     * Create an instance of {@link GetHelloWorldAsString }
     * 
     */
    public GetHelloWorldAsString createGetHelloWorldAsString() {
        return new GetHelloWorldAsString();
    }

    /**
     * Create an instance of {@link GetHelloWorldAsStringResponse }
     * 
     */
    public GetHelloWorldAsStringResponse createGetHelloWorldAsStringResponse() {
        return new GetHelloWorldAsStringResponse();
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link GetHelloWorldAsString }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://helloworld.org.rsi/", name = "getHelloWorldAsString")
    public JAXBElement<GetHelloWorldAsString> createGetHelloWorldAsString(GetHelloWorldAsString value) {
        return new JAXBElement<GetHelloWorldAsString>(_GetHelloWorldAsString_QNAME, GetHelloWorldAsString.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link GetHelloWorldAsStringResponse }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://helloworld.org.rsi/", name = "getHelloWorldAsStringResponse")
    public JAXBElement<GetHelloWorldAsStringResponse> createGetHelloWorldAsStringResponse(GetHelloWorldAsStringResponse value) {
        return new JAXBElement<GetHelloWorldAsStringResponse>(_GetHelloWorldAsStringResponse_QNAME, GetHelloWorldAsStringResponse.class, null, value);
    }

}
