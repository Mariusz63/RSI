using System.Net;
using System.Text;

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
            Console.WriteLine("Authorization header: " + context.Request.Headers["Authorization"].ToString());

            if (!context.Request.Headers.ContainsKey("Authorization"))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Response.ContentType = "text/plain; charset=utf-8";
                await context.Response.WriteAsync("Brak nagłówka Authorization");
                return;
            }


            var authHeader = context.Request.Headers["Authorization"].ToString();
            System.Diagnostics.Debug.WriteLine("Authorization header: " + authHeader);
            if (!authHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("Nieprawidłowy typ autoryzacji");
                return;
            }

            var encodedCredentials = authHeader.Substring("Basic ".Length).Trim();
            string username;
            string password;

            try
            {
                var decodedBytes = Convert.FromBase64String(encodedCredentials);
                var decodedString = Encoding.UTF8.GetString(decodedBytes);
                var credentials = decodedString.Split(':', 2); // split na 2 części, by uniknąć problemów z dwukropkami w haśle

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

            // Przykładowi użytkownicy - w prawdziwej aplikacji sprawdzać w bazie lub innym repozytorium
            var validUsers = new Dictionary<string, string>
            {
                { "admin", "admin123" },
                { "john", "doe123" }
            };

            if (!validUsers.TryGetValue(username, out var validPassword) || validPassword != password)
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
