/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package resources;

import jakarta.enterprise.context.RequestScoped;
import jakarta.ws.rs.Consumes;
import jakarta.ws.rs.DELETE;
import jakarta.ws.rs.GET;
import jakarta.ws.rs.HeaderParam;
import jakarta.ws.rs.MatrixParam;
import jakarta.ws.rs.POST;
import jakarta.ws.rs.PUT;
import jakarta.ws.rs.Path;
import jakarta.ws.rs.PathParam;
import jakarta.ws.rs.Produces;
import jakarta.ws.rs.QueryParam;
import jakarta.ws.rs.core.Context;
import jakarta.ws.rs.core.HttpHeaders;
import jakarta.ws.rs.core.MediaType;
import jakarta.ws.rs.core.UriInfo;
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

    @GET
    @Path("/{messageId}")
    @Produces(MediaType.APPLICATION_JSON)
    public Message getMessage(@PathParam("messageId") long id) {
        return service.getMessage(id);
    }

    @POST
    @Consumes(MediaType.APPLICATION_JSON)
    @Produces(MediaType.APPLICATION_JSON)
    public Message createMessage(Message message) {
        return service.createMessage(message);
    }
    
    @POST
    @Consumes(MediaType.APPLICATION_XML)
    @Produces(MediaType.APPLICATION_XML)
    public Message createMessageXML(Message message) {
        return service.createMessage(message);
    }
    

    @PUT
    @Path("/{messageId}")
    @Consumes(MediaType.APPLICATION_JSON)
    @Produces(MediaType.APPLICATION_JSON)
    public Message updateMessage(@PathParam("messageId") long id, Message message) {
        message.setId((int) id);
        return service.updateMessage(message);
    }
    
    @PUT
    @Path("/{messageId}")
    @Consumes(MediaType.APPLICATION_XML)
    @Produces(MediaType.APPLICATION_XML)
    public Message updateMessageXML(@PathParam("messageId") long id, Message message) {
        message.setId((int) id);
        return service.updateMessage(message);
    }

    @DELETE
    @Path("/{messageId}")
    public void deleteMessage(@PathParam("messageId") long id) {
        service.removeMessage(id);
    }

    @GET
    @Path("/filter")
    @Produces(MediaType.TEXT_PLAIN)
    public String filterMessages(@QueryParam("year") int year) {
        return "Filtruję wiadomości z roku: " + year;
    }

    @GET
    @Path("/header")
    @Produces(MediaType.TEXT_PLAIN)
    public String getHeader(@HeaderParam("User-Agent") String userAgent) {
        return "Twoja przeglądarka: " + userAgent;
    }

    @GET
    @Path("/matrix")
    @Produces(MediaType.TEXT_PLAIN)
    public String getMatrixParam(@MatrixParam("param") String value) {
        return "Matrix param: " + value;
    }

    @GET
    @Path("/context")
    @Produces(MediaType.TEXT_PLAIN)
    public String getContextExample(@Context UriInfo uriInfo, @Context HttpHeaders headers) {
        String path = uriInfo.getAbsolutePath().toString();
        String agent = headers.getRequestHeader("User-Agent").get(0);
        return "URL: " + path + "\nUser-Agent: " + agent;
    }

}
