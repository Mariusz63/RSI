package model;

import java.util.ArrayList;
import java.util.List;

public class Produkty {
    public List<Produkt> getAllProdukty() {
        List<Produkt> lista = new ArrayList<>();
        lista.add(new Produkt(1, "Telefon", "Samsung", 1200.0));
        lista.add(new Produkt(2, "Laptop", "Dell", 4500.0));
        return lista;
    }
}
