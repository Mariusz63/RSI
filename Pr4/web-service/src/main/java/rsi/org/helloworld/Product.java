package rsi.org.helloworld;

public class Product {
    private String nazwa;
    private String opis;
    private int cena;

    public Product() {}

    public Product(String nazwa, String opis, int cena) {
        this.nazwa = nazwa;
        this.opis = opis;
        this.cena = cena;
    }

    public String getNazwa() { return nazwa; }
    public void setNazwa(String nazwa) { this.nazwa = nazwa; }

    public String getOpis() { return opis; }
    public void setOpis(String opis) { this.opis = opis; }

    public int getCena() { return cena; }
    public void setCena(int cena) { this.cena = cena; }
}