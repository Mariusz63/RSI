/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package resources;

import jakarta.ws.rs.container.ContainerRequestContext;
import jakarta.ws.rs.container.ContainerRequestFilter;
import jakarta.ws.rs.container.ContainerResponseContext;
import jakarta.ws.rs.container.ContainerResponseFilter;
import jakarta.ws.rs.core.HttpHeaders;
import jakarta.ws.rs.core.Response;
import jakarta.ws.rs.ext.Provider;
import java.io.IOException;
import java.nio.charset.StandardCharsets;
import java.util.Base64;

/**
 *
 * @author mariu
 */
@Provider
public class AuthFilter implements ContainerRequestFilter, ContainerResponseFilter {

    @Override
    public void filter(ContainerRequestContext requestContext) throws IOException {
        String authHeader = requestContext.getHeaderString(HttpHeaders.AUTHORIZATION);

        if (authHeader != null && authHeader.startsWith("Basic ")) {
            String encodedCredentials = authHeader.substring("Basic ".length());
            String decoded = new String(Base64.getDecoder().decode(encodedCredentials), StandardCharsets.UTF_8);
            String[] parts = decoded.split(":", 2);

            if (parts.length == 2) {
                String username = parts[0];
                String password = parts[1];

                // 🔐 Weryfikacja loginu i hasła
                if ("admin".equals(username) && "admin123".equals(password)) {
                    return; // użytkownik uwierzytelniony – przepuszczamy dalej
                }
            }
        }

        //  Nieautoryzowane – brak nagłówka lub złe dane
        requestContext.abortWith(
            Response.status(Response.Status.UNAUTHORIZED)
                    .header(HttpHeaders.WWW_AUTHENTICATE, "Basic realm=\"Access to REST API\"")
                    .entity("Unauthorized access – provide valid credentials.")
                    .build()
        );
    }

    @Override
    public void filter(ContainerRequestContext crc, ContainerResponseContext crc1) throws IOException {
        throw new UnsupportedOperationException("Not supported yet."); // Generated from nbfs://nbhost/SystemFileSystem/Templates/Classes/Code/GeneratedMethodBody
    }
}