package org.jg.rsi;

import javax.jws.WebService;

@WebService(endpointInterface = "org.jg.rsi.HelloWorld")
public class HelloWorldImpl implements HelloWorld {
    @Override
    public String getHelloWorldAsString(String name) {
        System.out.println("Serwer otrzymał: " + name);
        return "Witaj świecie JAX-WS: " + name;
    }
}