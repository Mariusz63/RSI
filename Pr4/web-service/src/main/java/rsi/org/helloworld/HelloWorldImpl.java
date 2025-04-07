package rsi.org.helloworld;

import jakarta.jws.WebService;

import java.util.ArrayList;
import java.util.List;

@WebService(endpointInterface = "rsi.org.helloworld.HelloWorld")
public class HelloWorldImpl implements HelloWorld {
    @Override
    public String getHelloWorldAsString(String name) {
        return "Witaj świecie JAX-WS: " + name;
    }

    @Override
    public List<Product> getProducts() {
        List<Product> products = new ArrayList<>();
        products.add(new Product("Laptop", "Dell XPS 13", 5000));
        products.add(new Product("Smartfon", "iPhone 14", 4000));
        return products;
    }
}

