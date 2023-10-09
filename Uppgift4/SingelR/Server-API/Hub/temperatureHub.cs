using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Server_API;
using System.Text;
using System.Text.Json.Serialization;



//public class TemperatureHub : Hub
//{
//    public async Task SendTemperature(string deviceId, string encryptedTemperature)
//    {
//        await Clients.All.SendAsync("ReceiveTemperature", deviceId, encryptedTemperature);
//    }
//}




public class TemperatureHub : Hub
{
   
    public async Task SendTemperature(string deviceId, string dto)
    {

        var messageRecieved = JsonConvert.DeserializeObject<DTO>(dto);
        //var decryptedTemperature = DecryptTemperature(encryptedTemperature);

        var decryptedTemperature = messageRecieved.Temperature;

        // Här kan du hantera den dekrypterade temperaturen, t.ex. lagra den i en databas, logga den, etc.

        await Clients.All.SendAsync("UpdateTemperature", deviceId, decryptedTemperature);
    }

    private string DecryptTemperature(string encryptedTemp)
    {
        // dekrypteringsmetod 

        byte[] data = Convert.FromBase64String(encryptedTemp);
        var decryptedString = Encoding.UTF8.GetString(data);
        return decryptedString;
    }
}

