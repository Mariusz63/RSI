/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package model;

import jakarta.xml.bind.annotation.XmlRootElement;
import jakarta.xml.bind.annotation.XmlElement;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;

/**
 *
 * @author mariu
 */
@XmlRootElement(name = "message")
public class Message {
    private int id;
    private String message;
    private Date created;
    private String author;
    private List<Link> links = new ArrayList<>();

    public Message() {}
    
    public Message(int par, String hello) {} 

    public Message(int id, String text, String author) {
        this.id = id;
        this.message = text;
        this.created = new Date();
        this.author = author;
    }
    
    public Message(int id, String text, String author, List<Link> lista) {
        this.id = id;
        this.message = text;
        this.created = new Date();
        this.author = author;
        this.links=lista;
    }

    public List<Link> getLinks() {
       return links;
    }

    public void setLinks(List<Link> links) {
        this.links = links;
    }
        
    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public String getMessage() {
        return message;
    }

    public void setMessage(String message) {
        this.message = message;
    }

    public Date getCreated() {
        return created;
    }

    public void setCreated(Date created) {
        this.created = created;
    }

    public String getAuthor() {
        return author;
    }

    public void setAuthor(String author) {
        this.author = author;
    }
}