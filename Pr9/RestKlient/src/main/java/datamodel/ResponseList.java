package datamodel;
import java.util.List;
import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlRootElement;

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
