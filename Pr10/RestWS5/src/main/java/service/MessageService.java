/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package service;

import java.util.ArrayList;
import java.util.Date;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import model.Message;

/**
 *
 * @author mariu
 */
public class MessageService {
    
    static private Map<Long,Message> messages = new HashMap<Long, Message>();
        
    public MessageService() {
        messages.put(1L, new Message(1, "Hello", "Jacek"));
        messages.put(2L, new Message(2, "World", "Michał"));
        messages.put(3L, new Message(3, "World1", "Ola"));
    }
    
    public List<Message> getAllMessages(){
        return new ArrayList<>(messages.values());
    }
       
    public Message getMessage(Long id){
        return messages.get(id);
    }
    
   public Message createMessage(Message message) {
        long newId = messages.size() + 1;
        message.setId((int) newId);
        message.setCreated(new Date());
        messages.put(newId, message);
        return message;
    }

    public Message updateMessage(Message message) {
        if (!messages.containsKey((long) message.getId())) return null;
        messages.put((long) message.getId(), message);
        return message;
    }

    public void removeMessage(long id) {
        messages.remove(id);
    }
    
    public List<Message> getMessagesStartingWith(String prefix) {
    List<Message> result = new ArrayList<>();
    for (Message msg : messages.values()) {
        if (msg.getMessage().toLowerCase().startsWith(prefix.toLowerCase())) {
            result.add(msg);
            }
        }
    return result;
    }
}