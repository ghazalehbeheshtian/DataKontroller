using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.SignalR.Client;
using System.Threading.Tasks;
using Newtonsoft.Json;
using IOT_Client;

class Program
{
    private static readonly byte[] Key = Encoding.UTF8.GetBytes("YourSuperSecretK"); // Du bör välja en säker nyckel
    private static readonly byte[] IV = Encoding.UTF8.GetBytes("YourInitVectorHe"); // Init vektor bör vara 16 byte för AES

    static async Task Main()
    {
        var connection = new HubConnectionBuilder()
            .WithUrl("https://localhost:7107/temperatureHub")
            .Build();

        await connection.StartAsync();

        while (true)
        {
            var temperature = GetRandomTemperature();
            var encryptedTemperature = EncryptTemperature(temperature);

            var dto = new DTO { Temperature = encryptedTemperature, TimeStamp = DateTime.Now };

            var jsonToSend = JsonConvert.SerializeObject(dto);


            await connection.SendAsync("SendTemperature", "Device1", jsonToSend);
            await Task.Delay(5000);
        }
    }

    static double GetRandomTemperature()
    {
        return new Random().NextDouble() * 40.0;
    }

    static string EncryptTemperature(double temperature)
    {
        using (var aes = Aes.Create())
        {
            aes.Key = Key;
            aes.IV = IV;

            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using (var msEncrypt = new MemoryStream())
            {
                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                using (var swEncrypt = new StreamWriter(csEncrypt))
                {
                    swEncrypt.Write(temperature.ToString());
                }
                return Convert.ToBase64String(msEncrypt.ToArray());
            }
        }




        //static string DecryptTemperature(string encryptedTemp)
        //{
        //    using (Aes aes = Aes.Create())
        //    {
        //        aes.Key = Key;
        //        aes.IV = IV;

        //        ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

        //        using (MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(encryptedTemp)))
        //        {
        //            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
        //            {
        //                using (StreamReader streamReader = new StreamReader(cryptoStream))
        //                {
        //                    return streamReader.ReadToEnd();
        //                }
        //            }
        //        }
        //    }
        //}
    }
}
