/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package resources;

import jakarta.ws.rs.GET;
import jakarta.ws.rs.PathParam;
import jakarta.ws.rs.Produces;
import jakarta.ws.rs.core.MediaType;

/**
 *
 * @author mariu
 */
public class CommentResource {
    @GET
    @Produces(MediaType.APPLICATION_JSON)
    public String getComments(@PathParam("messageId") long messageId) {
        return "Lista komentarzy dla message ID: " + messageId;
    }
    
    
}
