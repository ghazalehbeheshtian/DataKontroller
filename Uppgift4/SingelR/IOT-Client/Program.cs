using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        var connection = new HubConnectionBuilder()
            //.WithUrl("https://localhost:7191/temperatureHub")
            .WithUrl("https://localhost:7107/temperatureHub")
            //.WithUrl("/temperatureHub")
            .Build();

        await connection.StartAsync();

        while (true)
        {
            var temperature = GetRandomTemperature();
            var encryptedTemperature = EncryptTemperature(temperature); // Implementera kryptering här
            await connection.SendAsync("SendTemperature", "Device1", encryptedTemperature);
            await Task.Delay(5000);
        }
    }

    static double GetRandomTemperature()
    {
        // Generera ett slumpmässigt temperaturvärde
        return new Random().NextDouble() * 40.0;
    }

    static string EncryptTemperature(double temperature)
    {
        // Implementera krypteringslogik här
        return temperature.ToString(); // Denna bör ändras till riktig kryptering
    }
}
