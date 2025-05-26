/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 */

package rest.restklient;

import datamodel.Produkt;
import datamodel.ResponseList;
import javax.ws.rs.client.Client;
import javax.ws.rs.client.ClientBuilder;
import javax.ws.rs.core.MediaType;

/**
 *
 * @author mariu
 */
public class RestKlient {

   public static void main(String[] args) {
        String URL = "http://localhost:8080/RESTSklep/rest/sklep/allproducts";

        Client client = ClientBuilder.newClient();

        ResponseList response = client
                .target(URL)
                .request(MediaType.APPLICATION_JSON)
                .get(ResponseList.class);

        if (response != null && response.getList() != null) {
            for (Produkt p : response.getList()) {
                System.out.println(p);
            }
        } else {
            System.out.println("Brak danych lub błąd połączenia.");
        }

        client.close();
    }
}
