using System.Net.WebSockets; // Importera nödvändiga namespaces.
using WS_Server;

namespace Server;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args); // Skapa en instans av WebApplicationBuilder.

        // Lägg till tjänster i behållaren.
        builder.Services.AddRazorPages(); // Lägg till Razor Pages-tjänsten.
        builder.Services.AddCors(); // Lägg till CORS-tjänsten för korsdomänshantering.

        var app = builder.Build(); // Bygg en instans av WebApplication.

        app.UseExceptionHandler("/Error"); // Hantera undantag och dirigera till "/Error"-sidan vid fel.
        app.UseHsts(); // Använd HTTP Strict Transport Security (HSTS) för att tvinga HTTPS.
        app.UseHttpsRedirection(); // Omdirigera HTTP-förfrågningar till HTTPS.
        app.UseStaticFiles(); // Aktivera servern att serva statiska filer.
        app.UseRouting(); // Aktivera rutningsmekanismen för att hantera URL:er.
        app.UseAuthorization(); // Använd auktorisationsmekanismen om det är aktuellt.

        app.UseCors(policy =>
            policy.AllowAnyOrigin() // Tillåt förfrågningar från alla ursprung (CORS).
                  .AllowAnyMethod() // Tillåt alla HTTP-metoder (CORS).
                  .AllowAnyHeader()); // Tillåt alla HTTP-headers (CORS).

        app.UseWebSockets(); // Aktivera WebSocket-stöd på servern.

        app.Use(async (context, next) =>
        {
            if (context.Request.Path == "/ws") // Om förfrågan är för WebSocket-endpoint "/ws".
            {
                if (context.WebSockets.IsWebSocketRequest) // Kontrollera om det är en WebSocket-förfrågan.
                {
                    using WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync(); // Acceptera WebSocket-förfrågan.
                    await MyWebSocket.MyWebSocketHandler(context, webSocket); // Anropa din anpassade WebSocket-hanterare.
                }
                else
                {
                    context.Response.StatusCode = 400; // Om det inte är en WebSocket-förfrågan, sätt statuskoden till 400 (Bad Request).
                }
            }
            else
            {
                await next(); // Annars, fortsätt med nästa middleware i pipelinen.
            }
        });

        app.MapRazorPages(); // Använd Razor Pages-mappning.

        app.Run(); // Starta servern.
    }
}
