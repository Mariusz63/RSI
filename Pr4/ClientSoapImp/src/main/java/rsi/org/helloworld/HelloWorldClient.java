package rsi.org.helloworld;

public class HelloWorldClient {
    public static void main(String[] args) {
        HelloWorldImplService service = new HelloWorldImplService();

        HelloWorld helloWorld = service.getHelloWorldImplPort();

        String response = helloWorld.getHelloWorldAsString("Klient JAX-WS");

        System.out.println("Odpowiedź z serwera: " + response);
    }
}