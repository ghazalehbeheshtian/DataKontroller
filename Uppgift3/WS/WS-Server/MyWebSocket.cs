using System.Net.WebSockets;
using System.Text.Json;
using System.Text;

namespace WS_Server
{
    public class MyWebSocket
    {
        public static async Task MyWebSocketHandler(HttpContext context, WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            while (!result.CloseStatus.HasValue)
            {
                var clientMessageJson = Encoding.UTF8.GetString(buffer, 0, result.Count);
                var clientData = JsonSerializer.Deserialize<Dictionary<string, string>>(clientMessageJson);
                var clientMessage = clientData.ContainsKey("request") ? clientData["request"] : "";
                var responseMessage = "";

                if (clientMessage == "roll_dice")
                {
                    // Generera ett slumpmässigt tärningskast mellan 1 och 6
                    var random = new Random();
                    var diceRoll = random.Next(1, 7);

                    // Skapa ett JSON-svarmeddelande
                    var responseObject = new { response = $"Du rullade en tärning och fick: {diceRoll}" };
                    responseMessage = JsonSerializer.Serialize(responseObject);
                }
                else
                {
                    // Om klientens meddelande inte är "roll_dice", svara med ett felmeddelande
                    var responseObject = new { response = "Ogiltig förfrågan." };
                    responseMessage = JsonSerializer.Serialize(responseObject);
                }

                // Skicka svar till klienten
                byte[] responseBytes = Encoding.UTF8.GetBytes(responseMessage);
                await webSocket.SendAsync(new ArraySegment<byte>(responseBytes, 0, responseBytes.Length), WebSocketMessageType.Text, true, CancellationToken.None);

                // Lyssna på nästa meddelande
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }

            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }
    }
}
