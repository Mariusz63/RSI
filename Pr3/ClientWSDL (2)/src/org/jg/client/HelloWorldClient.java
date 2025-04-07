package org.jg.client;

import java.net.URL;
import javax.xml.namespace.QName;
import org.jg.client.HelloWorld;
import org.jg.client.HelloWorldImplService;

public class HelloWorldClient {
    public static void main(String[] args) {
        try {
            String endpointURL = "http://localhost:8080/ws/hello";
            HelloWorldImplService service = new HelloWorldImplService(new URL(endpointURL), new QName("http://rsi.jg.org/", "HelloWorldImplService"));
            HelloWorld hello = service.getHelloWorldImplPort();
            System.out.println(hello.getHelloWorldAsString("Pokazanie działania klienta"));
        } catch (Exception e) {
            e.printStackTrace();
        }
    }
}