using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;


// Create a UDP client socket
var udpClient = new UdpClient(5000); // Listen on port 12345

try
{
    while (true)
    {
        // Receive data from the sender
        IPEndPoint senderEndPoint = new IPEndPoint(IPAddress.Any, 0);
        byte[] receivedData = udpClient.Receive(ref senderEndPoint);
        var receivedJson = Encoding.UTF8.GetString(receivedData);

        // Deserialize the received JSON data
        var receivedObject = JsonConvert.DeserializeObject<DataObject>(receivedJson);

        Console.WriteLine("Data received from sender: " + receivedJson);
        Console.WriteLine("Deserialized Message: " + receivedObject.Message);
        Console.WriteLine("Deserialized Value: " + receivedObject.Value);
    }
}
catch (Exception e)
{
    Console.WriteLine("Error: " + e.Message);
}
finally
{
    // Close the UDP client
    udpClient.Close();
}

class DataObject
{
    public string Message { get; set; }
    public int Value { get; set; }
}
