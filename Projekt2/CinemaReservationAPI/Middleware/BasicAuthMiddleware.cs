using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using CinemaReservationAPI.Data; 
using System.Linq;

namespace CinemaReservationAPI.Middleware
{
    public class BasicAuthMiddleware
    {
        private readonly RequestDelegate _next;

        public BasicAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.Request.Headers.ContainsKey("Authorization"))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Response.ContentType = "text/plain; charset=utf-8";
                await context.Response.WriteAsync("Brak nagłówka Authorization");
                return;
            }

            var authHeader = context.Request.Headers["Authorization"].ToString();
            if (!authHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("Nieprawidłowy typ autoryzacji");
                return;
            }

            var encodedCredentials = authHeader.Substring("Basic ".Length).Trim();

            string username, password;

            try
            {
                var decodedBytes = Convert.FromBase64String(encodedCredentials);
                var decodedString = Encoding.UTF8.GetString(decodedBytes);
                var credentials = decodedString.Split(':', 2);

                if (credentials.Length != 2)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    await context.Response.WriteAsync("Nieprawidłowy format poświadczeń");
                    return;
                }

                username = credentials[0];
                password = credentials[1];
            }
            catch (FormatException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("Nieprawidłowe zakodowanie poświadczeń");
                return;
            }

            // Sprawdź użytkownika w DataStore
            var user = SampleUsers.Users.FirstOrDefault(u => u.Name == username && u.Password == password);
            if (user == null)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("Błędny login lub hasło");
                return;
            }

            // Zapisz użytkownika w kontekście dla dalszych middleware/endpointów
            context.Items["User"] = username;

            await _next(context);
        }
    }
}
