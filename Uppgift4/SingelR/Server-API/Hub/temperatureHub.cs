using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using Server_API;
using System.Security.Cryptography;
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
        var decryptedTemperature = DecryptTemperature(messageRecieved.Temperature);

        //var decryptedTemperature = messageRecieved.Temperature;

        // Här kan du hantera den dekrypterade temperaturen, t.ex. lagra den i en databas, logga den, etc.

        await Clients.All.SendAsync("UpdateTemperature", deviceId, decryptedTemperature);
    }

    //private string DecryptTemperature(string encryptedTemp)
    //{
    //    // dekrypteringsmetod 

    //    byte[] data = Convert.FromBase64String(encryptedTemp);
    //    var decryptedString = Encoding.UTF8.GetString(data);
    //    return decryptedString;
    //}

    private string DecryptTemperature(string encryptedTemp)
    {
        byte[] Key = Encoding.UTF8.GetBytes("YourSuperSecretK"); // Du bör välja en säker nyckel
        byte[] IV = Encoding.UTF8.GetBytes("YourInitVectorHe"); // Init vektor bör vara 16 byte för AES


        byte[] cipherTextBytes = Convert.FromBase64String(encryptedTemp);

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Key;
            aesAlg.IV = IV;

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msDecrypt = new MemoryStream(cipherTextBytes))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
        }
    }


}

