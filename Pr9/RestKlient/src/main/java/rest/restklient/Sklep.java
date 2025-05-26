/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/WebServices/JerseyClient.java to edit this template
 */
package rest.restklient;

import datamodel.Produkt;
import datamodel.ResponseList;
import jakarta.ws.rs.client.Client;
import jakarta.ws.rs.client.ClientBuilder;
import jakarta.ws.rs.client.WebTarget;
import jakarta.ws.rs.core.MediaType;

/**
 * Jersey REST client generated for REST resource:SklepResource []<br>
 * USAGE:
 * <pre>
 *        Sklep client = new Sklep();
 *        Object response = client.XXX(...);
 *        // do whatever with response
 *        client.close();
 * </pre>
 *
 * @author mariu
 */
public class Sklep {
    private static final String BASE_URI = "http://localhost:8080/RESTSklep/rest/sklep";
    private final Client client;
    private final WebTarget baseTarget;

    public Sklep() {
        this.client = ClientBuilder.newClient();
        this.baseTarget = client.target(BASE_URI);
    }

    public <T> T getAllProdukty(Class<T> responseType) {
        return baseTarget
                .path("allproducts")
                .request(MediaType.APPLICATION_JSON)
                .get(responseType);
    }

    public void close() {
        client.close();
    }
    
//    public <T> T searchProdukty(Object searchCriteria, Class<T> responseType) {
//    return baseTarget
//            .path("search")
//            .request(MediaType.APPLICATION_JSON)
//            .post(responseType);
//    }

    ResponseList searchProdukty(Produkt search, Class<ResponseList> aClass) {
        throw new UnsupportedOperationException("Not supported yet."); // Generated from nbfs://nbhost/SystemFileSystem/Templates/Classes/Code/GeneratedMethodBody
    }

}