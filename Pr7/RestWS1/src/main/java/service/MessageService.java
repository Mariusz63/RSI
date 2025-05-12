/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package service;

import java.util.ArrayList;
import java.util.List;
import model.Message;

/**
 *
 * @author mariu
 */
public class MessageService {
    public List<Message> getAllMessages() {
        List<Message> messages = new ArrayList<>();
        messages.add(new Message(1, "Hello", "Jacek"));
        messages.add(new Message(2, "World", "Michał"));
        messages.add(new Message(2, "World1", "Ola"));
        return messages;
    }
}