package client;

import jakarta.ws.rs.GET;
import jakarta.ws.rs.Path;
import jakarta.ws.rs.Produces;
import jakarta.ws.rs.QueryParam;
import jakarta.ws.rs.client.Client;
import jakarta.ws.rs.client.ClientBuilder;
import jakarta.ws.rs.client.Entity;
import jakarta.ws.rs.client.WebTarget;
import jakarta.ws.rs.core.GenericType;
import jakarta.ws.rs.core.MediaType;
import jakarta.ws.rs.core.Response;

import java.util.List;

/**
 *
 * @author mariu
 */
@Path("/messages")
public class Rest_Client {

    private static final String BASE_URL = "http://localhost:8080/RestWS1/webresources/messages";

    public static void main(String[] args) {
        System.out.println("=== GET /messages/1 ===");
        getMessageById(1);

        System.out.println("\n=== POST /messages ===");
        postMessage();

        System.out.println("\n=== GET /messages ===");
        getAllMessages();

        System.out.println("\n=== PUT /messages/5 ===");
       // updateMessage(5);

        System.out.println("\n=== DELETE /messages/5 ===");
       // deleteMessage(5);

        System.out.println("\n=== GET /messages?zaczynasie=he ===");
        getMessagesStartingWith("he");
    }

    @GET
    @Produces(MediaType.APPLICATION_JSON)
    private static void getMessageById(int id) {
        Client client = ClientBuilder.newClient();
        WebTarget target = client.target(BASE_URL + "/" + id);

        Response response = target.request(MediaType.APPLICATION_JSON).get();
        System.out.println("Status: " + response.getStatus());

        if (response.getStatus() == 200) {
            String message = response.readEntity(String.class);
            System.out.println("Body: " + message);
        } else {
            System.out.println("Błąd pobierania wiadomości.");
        }
    }

    private static void postMessage() {
        Client client = ClientBuilder.newClient();
        WebTarget target = client.target(BASE_URL);

        Message newMessage = new Message(6, "Wiadomość od klienta!", "Tadeusz2");

        Response response = target.request(MediaType.APPLICATION_JSON)
                .post(Entity.json(newMessage));

        System.out.println("Status: " + response.getStatus());
        System.out.println("Body: " + response.readEntity(String.class));
    }

    @GET
    @Produces(MediaType.APPLICATION_JSON)
    private static void getAllMessages() {
        Client client = ClientBuilder.newClient();
        WebTarget target = client.target(BASE_URL);

        List<Message> messages = target.request(MediaType.APPLICATION_JSON)
                .get(new GenericType<List<Message>>() {});

        System.out.println("Ilość wiadomości: " + messages.size());
        messages.forEach(System.out::println);
    }

    private static void updateMessage(int id) {
        Client client = ClientBuilder.newClient();
        WebTarget target = client.target(BASE_URL + "/" + id);

        Message updated = new Message(id, "Zaktualizowana treść", "Tadeusz");

        Response response = target.request(MediaType.APPLICATION_JSON)
                .put(Entity.json(updated));

        System.out.println("Status: " + response.getStatus());
        if (response.getStatus() == 200 || response.getStatus() == 204) {
            System.out.println("Zaktualizowano wiadomość.");
            try {
                System.out.println("Body: " + response.readEntity(String.class));
            } catch (Exception e) {
                System.out.println("Brak treści odpowiedzi.");
            }
        } else {
            System.out.println("Błąd aktualizacji.");
        }
    }

    private static void deleteMessage(int id) {
        Client client = ClientBuilder.newClient();
        WebTarget target = client.target(BASE_URL + "/" + id);

        Response response = target.request().delete();

        System.out.println("Status: " + response.getStatus());
        if (response.getStatus() == 204) {
            System.out.println("Wiadomość usunięta.");
        } else {
            System.out.println("Błąd usuwania.");
        }
    }

    private static void getMessagesStartingWith(String prefix) {
        Client client = ClientBuilder.newClient();
        WebTarget target = client.target(BASE_URL).queryParam("zaczynasie", prefix);

        List<Message> messages = target.request(MediaType.APPLICATION_JSON)
                .get(new GenericType<List<Message>>() {});

        System.out.println("Wiadomości zaczynające się od \"" + prefix + "\":");
        messages.forEach(System.out::println);
    }
    


}