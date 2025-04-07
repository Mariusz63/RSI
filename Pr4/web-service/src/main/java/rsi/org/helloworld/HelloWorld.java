package rsi.org.helloworld;

import jakarta.jws.WebService;
import jakarta.jws.WebMethod;

import java.util.List;

@WebService
public interface HelloWorld {
    @WebMethod
    String getHelloWorldAsString(String name);
    @WebMethod
    List<Product> getProducts();
}