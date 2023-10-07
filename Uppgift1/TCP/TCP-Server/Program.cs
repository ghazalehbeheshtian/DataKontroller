using Newtonsoft.Json;
using System.Net.Sockets;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TCP_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            // Här startar jag servern med hjälp utav en TcpListener som använder sig utav port 5000
            TcpListener server = new TcpListener(IPAddress.Any, 5000);
            server.Start();
            Console.WriteLine("Server started...");

            //Här skapar jag en while loop som gör att programmet inte avslutas förens vi stänger av den manuellt
            while (true)
            {
                // Här skapar jag upp en tcpClient som är baserad på klienten som ansluter
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("Client connected!");

                // Här skapar jag upp en writer och en reader från anslutningen till klienten
                StreamReader reader = new StreamReader(client.GetStream());
                StreamWriter writer = new StreamWriter(client.GetStream());

                string request = reader.ReadLine();

                // Här gör jag om JSON objektet till klassen Data
                var requestData = JsonConvert.DeserializeObject<Data>(request);

                // Här gör jag tvärtom och tar klassen och gör om det till ett JSON objekt som jag kan skicka.
                string response = JsonConvert.SerializeObject(requestData);

                // Här skickar jag tillbaka ett svar till clienten.
                writer.WriteLine(response);
                writer.Flush();
            }
        }
    }
}