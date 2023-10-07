using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;


// Create a UDP client socket
var udpClient = new UdpClient();

// Define the server endpoint (IP address and port)
var serverEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5000);

// Create a sample data object
var dataObject = new DataObject { Message = "Hello, Receiver!", Value = 42 };

try
{
    // Serialize the data object to JSON
    var jsonData = JsonConvert.SerializeObject(dataObject);

    // Convert the JSON data to bytes
    byte[] sendData = Encoding.UTF8.GetBytes(jsonData);

    // Send the data to the server
    udpClient.Send(sendData, sendData.Length, serverEndPoint);

    Console.WriteLine("Data sent to receiver: " + jsonData);
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
