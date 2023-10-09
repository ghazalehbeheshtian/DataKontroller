using Microsoft.AspNetCore.SignalR;
using System.Text;



//public class TemperatureHub : Hub
//{
//    public async Task SendTemperature(string deviceId, string encryptedTemperature)
//    {
//        await Clients.All.SendAsync("ReceiveTemperature", deviceId, encryptedTemperature);
//    }
//}




public class TemperatureHub : Hub
{
    public async Task SendTemperature(string deviceId, string encryptedTemperature)
    {
        var decryptedTemperature = DecryptTemperature(encryptedTemperature);

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
