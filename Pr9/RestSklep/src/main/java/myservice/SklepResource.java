package myservice;

import jakarta.ws.rs.Consumes;
import model.Produkt;
import model.Produkty;
import jakarta.ws.rs.GET;
import jakarta.ws.rs.POST;
import jakarta.ws.rs.Path;
import jakarta.ws.rs.Produces;
import jakarta.ws.rs.core.Context;
import jakarta.ws.rs.core.MediaType;
import jakarta.ws.rs.core.UriInfo;
import java.util.ArrayList;
import java.util.List;

@Path("sklep")
public class SklepResource {
    private Produkty produkty = new Produkty();
    
    @Context
    private UriInfo context;
  
    public SklepResource() {}
    
    public SklepResource(UriInfo context) {
        this.context = context;
    }

    @GET
    @Path("/allproducts")
    @Produces(MediaType.APPLICATION_JSON)
    public ResponseList getAllProdukty() {
        List<Produkt> lista = produkty.getAllProdukty();
        ResponseList response = new ResponseList();
        response.setList(lista);
        return response;
    }
    
    @POST
    @Path("/search")
    @Consumes(MediaType.APPLICATION_JSON)
    @Produces(MediaType.APPLICATION_JSON)
    public ResponseList searchProducts(Produkt search) {
        List<Produkt> all = produkty.getAllProdukty();
        List<Produkt> result = new ArrayList<>();

        for (Produkt p : all) {
            boolean matches = true;
            if (search.getName() != null && !search.getName().isEmpty()) {
                matches &= p.getName().toLowerCase().contains(search.getName().toLowerCase());
            }
            if (search.getManufacturer() != null && !search.getManufacturer().isEmpty()) {
                matches &= p.getManufacturer().toLowerCase().contains(search.getManufacturer().toLowerCase());
            }
            if (search.getPrice() > 0) {
                matches &= p.getPrice() <= search.getPrice();
            }

            if (matches) result.add(p);
        }

        ResponseList response = new ResponseList();
        response.setList(result);
        return response;
    }

}
