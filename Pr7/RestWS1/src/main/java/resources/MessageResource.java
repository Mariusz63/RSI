/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package resources;

import jakarta.enterprise.context.RequestScoped;
import jakarta.ws.rs.Consumes;
import jakarta.ws.rs.GET;
import jakarta.ws.rs.Path;
import jakarta.ws.rs.Produces;
import jakarta.ws.rs.core.MediaType;
import java.util.List;
import model.Message;
import service.MessageService;

/**
 *
 * @author mariu
 */
@Path("/messages")
public class MessageResource {

    MessageService service = new MessageService();

    @GET
    @Produces(MediaType.APPLICATION_XML)
    public List<Message> getText() {
        return service.getAllMessages();
    }

    @GET
    @Produces(MediaType.APPLICATION_JSON)
    public List<Message> getMessagesJson() {
        return service.getAllMessages();
    }
}

