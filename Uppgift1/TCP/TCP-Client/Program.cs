
using Newtonsoft.Json;
using System.Net.Sockets;
using TCP_Client;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            //Här gör jag en readkey så att servern ska hinna starta
            Console.WriteLine("Tryck på en tangent när du vill starta programmet");
            Console.ReadKey();


            // Här skapar jag en anslutning till server miljön
            TcpClient client = new TcpClient("127.0.0.1", 5000);
            Console.WriteLine("Connected to server.........");



            // Här gör jag två variabler som är valfria så att man kan variera programmet lite
            Console.Write("Skriv in ditt meddelande: ");
            string message = Console.ReadLine();
            Console.Write("Skriv in ett nummer: ");
            int number = Convert.ToInt32(Console.ReadLine());



            //Här tar jag mina variabler och gör till en klass som jag sedan konverterar om till ett JSON objekt
            var data = new Data { Message = message, Number = number };
            string jsonData = JsonConvert.SerializeObject(data);
            Console.WriteLine(jsonData);



            // Här använder jag mig utav en streamwriter och tcpclient för att skicka mitt JSON objekt till servern
            StreamWriter writer = new StreamWriter(client.GetStream());
            writer.WriteLine(jsonData);
            writer.Flush();


            // När svaret kommer tillbaka använder jag mig av streamreadern och tcp client för att ta emot det
            //Sedan gör jag om det från ett JSON till en Data klass.
            StreamReader reader = new StreamReader(client.GetStream());
            string response = reader.ReadLine();
            var responseData = JsonConvert.DeserializeObject<Data>(response);

            // Här tar jag variablarna i klassen och skriver ut det i en writeline
            Console.WriteLine($"Received: Message: {responseData.Message}, Number: {responseData.Number}");
        }
    }
}