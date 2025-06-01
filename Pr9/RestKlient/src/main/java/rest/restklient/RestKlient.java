/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 */

package rest.restklient;

import datamodel.Produkt;
import datamodel.ResponseList;

/**
 *
 * @author mariu
 */
public class RestKlient {
    public static void main(String[] args) {
        Sklep sklep = new Sklep();

        try {
            // Przykład kryteriów wyszukiwania
            Produkt search = new Produkt();
            search.setName("Laptop");

            ResponseList result = sklep.searchProdukty(search, ResponseList.class);

            if (result != null && result.getList() != null && !result.getList().isEmpty()) {
                System.out.println("Znaleziono produkty:");
                for (Produkt p : result.getList()) {
                    System.out.println(p);
                }
            } else {
                System.out.println("Nie znaleziono żadnych produktów.");
            }
        } catch (Exception e) {
            System.err.println("Błąd zapytania: " + e.getMessage());
        } finally {
            sklep.close();
        }
    }
}
