package obe;

import jakarta.annotation.Resource;
import jakarta.enterprise.context.RequestScoped;
import jakarta.faces.application.FacesMessage;
import jakarta.faces.context.FacesContext;
import jakarta.inject.Named;
import jakarta.jms.ConnectionFactory;
import jakarta.jms.JMSContext;
import jakarta.jms.Queue;


@Named("messageProducerBean")
@RequestScoped
public class MessageProducerBean {
    
    private String message;

    @Resource(lookup = "jms/myQueueFactory")
    private ConnectionFactory connectionFactory;

    @Resource(lookup = "jms/myQueue")
    private Queue queue;

    public String getMessage() {
        return message;
    }
    public void setMessage(String message) {
        this.message = message;
    }
    public void send() {
        if (message == null || message.trim().isEmpty()) {
            FacesContext.getCurrentInstance().addMessage(null,
                new FacesMessage(FacesMessage.SEVERITY_WARN, "Wiadomość jest pusta!", null));
            return;
        }
        try (JMSContext context = connectionFactory.createContext()) {
            context.createProducer().send(queue, message);
            FacesContext.getCurrentInstance().addMessage(null,
                new FacesMessage("Wiadomość wysłana: " + message));
            System.out.println("Wiadomość wysłana: " + message);
            message = ""; // wyczyść pole po wysłaniu
        } catch (Exception e) {
            e.printStackTrace();
            FacesContext.getCurrentInstance().addMessage(null,
                new FacesMessage(FacesMessage.SEVERITY_ERROR, "Błąd podczas wysyłania wiadomości.", null));
        }
    }
}

