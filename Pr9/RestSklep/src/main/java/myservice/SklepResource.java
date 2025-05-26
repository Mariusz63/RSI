package myservice;

import model.Produkt;
import model.Produkty;
import jakarta.ws.rs.GET;
import jakarta.ws.rs.Path;
import jakarta.ws.rs.Produces;
import jakarta.ws.rs.core.Context;
import jakarta.ws.rs.core.MediaType;
import jakarta.ws.rs.core.UriInfo;
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
}
