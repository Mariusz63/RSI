/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/WebServices/GenericResource.java to edit this template
 */
package service;

import jakarta.ws.rs.core.Context;
import jakarta.ws.rs.core.UriInfo;
import jakarta.ws.rs.Consumes;
import jakarta.ws.rs.GET;
import jakarta.ws.rs.Produces;
import jakarta.ws.rs.PUT;
import jakarta.ws.rs.Path;
import jakarta.enterprise.context.RequestScoped;
import jakarta.ws.rs.PathParam;
import jakarta.ws.rs.core.MediaType;

/**
 * REST Web Service
 *
 * @author mariu
 */
@Path("hello")
@RequestScoped
public class HelloWorldService {

    @Context
    private UriInfo context;

    /**
     * Creates a new instance of HelloWorldService
     */
    public HelloWorldService() {
    }

    /**
     * Retrieves representation of an instance of service.HelloWorldService
     * @return an instance of java.lang.String
     */
    @GET
    @Produces(MediaType.TEXT_HTML)
    public String getHtml() {
        return "Witaj JAX-RS";
    }
    
    @GET
    @Path("/echo")
    @Produces(MediaType.TEXT_PLAIN)
    public String echo() {
        return "Witaj Echo";
    }
    
    @GET
    @Path("/echo2/{parametr}")
    @Produces(MediaType.TEXT_PLAIN)
    public String echo(@PathParam("parametr") String name) {
      return "Otrzymano parametr: " + name;
    }        


    /**
     * PUT method for updating or creating an instance of HelloWorldService
     * @param content representation for the resource
     */
    @PUT
    @Consumes(MediaType.TEXT_HTML)
    public void putHtml(String content) {
    }
}
