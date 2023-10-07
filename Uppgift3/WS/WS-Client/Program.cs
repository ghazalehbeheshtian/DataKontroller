
using System.Net.WebSockets;
using System.Text;
using Newtonsoft.Json;


var serverUri = new Uri("ws://localhost:5000/ws"); // Ange WebSocket-serverns URL här

//Vi skapar en instans av ClientWebSocket och använder using-block för att se till att resurserna frigörs korrekt när de inte längre behövs.
using (ClientWebSocket client = new ClientWebSocket())
{
    try
    {
        //Vi ansluter till WebSocket-servern med ConnectAsync-metoden och passerar serverns URI som argument.
        await client.ConnectAsync(serverUri, CancellationToken.None);
        Console.WriteLine("Ansluten till WebSocket-server.");

        //Här skapar vi en Dictionary som innehåller vårt meddelande. I detta fall skickar vi ett JSON-objekt som har en request-nyckel med ett meddelande.
        var dataToSend = new Dictionary<string, string>
        {
            { "request", "Hej server, detta är en komplex förfrågan!" }
        };

        var jsonData = JsonConvert.SerializeObject(dataToSend);

        //Vi konverterar vår JSON-sträng till en byte-array eftersom WebSocket använder byte-array för att skicka data.
        byte[] dataBytes = Encoding.UTF8.GetBytes(jsonData);

        //Vi skickar
        await client.SendAsync(new ArraySegment<byte>(dataBytes), WebSocketMessageType.Text, true, CancellationToken.None);

        // Ta emot data från servern
        var buffer = new byte[1024 * 4];
        var receiveResult = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

        if (receiveResult.MessageType == WebSocketMessageType.Text)
        {
            string responseJson = Encoding.UTF8.GetString(buffer, 0, receiveResult.Count);
            var responseData = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseJson);
            string responseMessage = responseData.ContainsKey("response") ? responseData["response"] : "";
            Console.WriteLine($"Mottagen respons från servern: {responseMessage}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Fel vid kommunikation med servern: {ex.Message}");
    }
    finally
    {
        if (client.State == WebSocketState.Open)
        {
            await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "Stängning av klient", CancellationToken.None);
        }
    }//Vi skriver ut det mottagna meddelandet från servern.
}
