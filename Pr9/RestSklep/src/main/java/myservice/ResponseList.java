package myservice;

import jakarta.xml.bind.annotation.XmlAccessType;
import jakarta.xml.bind.annotation.XmlAccessorType;
import jakarta.xml.bind.annotation.XmlElement;
import jakarta.xml.bind.annotation.XmlRootElement;
import java.util.List;
import model.Produkt;

@XmlRootElement
@XmlAccessorType(XmlAccessType.NONE)
public class ResponseList {
    
    @XmlElement(name="produkty")
    private List<Produkt> list;

    public ResponseList() {}

    public List<Produkt> getList() {
        return list;
    }

    public void setList(List<Produkt> list) {
        this.list = list;
    }
}
