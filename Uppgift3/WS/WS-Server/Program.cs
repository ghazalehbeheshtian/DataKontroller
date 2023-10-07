using System.Net.WebSockets; // Importera n�dv�ndiga namespaces.
using WS_Server;

namespace Server;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args); // Skapa en instans av WebApplicationBuilder.

        // L�gg till tj�nster i beh�llaren.
        builder.Services.AddRazorPages(); // L�gg till Razor Pages-tj�nsten.
        builder.Services.AddCors(); // L�gg till CORS-tj�nsten f�r korsdom�nshantering.

        var app = builder.Build(); // Bygg en instans av WebApplication.

        app.UseExceptionHandler("/Error"); // Hantera undantag och dirigera till "/Error"-sidan vid fel.
        app.UseHsts(); // Anv�nd HTTP Strict Transport Security (HSTS) f�r att tvinga HTTPS.
        app.UseHttpsRedirection(); // Omdirigera HTTP-f�rfr�gningar till HTTPS.
        app.UseStaticFiles(); // Aktivera servern att serva statiska filer.
        app.UseRouting(); // Aktivera rutningsmekanismen f�r att hantera URL:er.
        app.UseAuthorization(); // Anv�nd auktorisationsmekanismen om det �r aktuellt.

        app.UseCors(policy =>
            policy.AllowAnyOrigin() // Till�t f�rfr�gningar fr�n alla ursprung (CORS).
                  .AllowAnyMethod() // Till�t alla HTTP-metoder (CORS).
                  .AllowAnyHeader()); // Till�t alla HTTP-headers (CORS).

        app.UseWebSockets(); // Aktivera WebSocket-st�d p� servern.

        app.Use(async (context, next) =>
        {
            if (context.Request.Path == "/ws") // Om f�rfr�gan �r f�r WebSocket-endpoint "/ws".
            {
                if (context.WebSockets.IsWebSocketRequest) // Kontrollera om det �r en WebSocket-f�rfr�gan.
                {
                    using WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync(); // Acceptera WebSocket-f�rfr�gan.
                    await MyWebSocket.MyWebSocketHandler(context, webSocket); // Anropa din anpassade WebSocket-hanterare.
                }
                else
                {
                    context.Response.StatusCode = 400; // Om det inte �r en WebSocket-f�rfr�gan, s�tt statuskoden till 400 (Bad Request).
                }
            }
            else
            {
                await next(); // Annars, forts�tt med n�sta middleware i pipelinen.
            }
        });

        app.MapRazorPages(); // Anv�nd Razor Pages-mappning.

        app.Run(); // Starta servern.
    }
}
