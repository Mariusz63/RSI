package MDB;

import jakarta.ejb.ActivationConfigProperty;
import jakarta.ejb.MessageDriven;
import jakarta.jms.JMSException;
import jakarta.jms.Message;
import jakarta.jms.MessageListener;
import jakarta.jms.TextMessage;


@MessageDriven(
    activationConfig = {
        @ActivationConfigProperty(propertyName = "destinationLookup", propertyValue = "jms/myQueue"),
        @ActivationConfigProperty(propertyName = "destinationType", propertyValue = "jakarta.jms.Queue")
    }
)
public class MessageConsumerBean implements MessageListener {

    public MessageConsumerBean(){
        
    }
    
    @Override
    public void onMessage(Message message) {
        try {
            if (message instanceof TextMessage) {
                String text = ((TextMessage) message).getText();
                System.out.println("Otrzymano wiadomość z kolejki: " + text);
            } else {
                System.out.println("Odebrano nieznany typ wiadomości.");
            }
        } catch (JMSException e) {
            e.printStackTrace();
        }
    }
}