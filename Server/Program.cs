using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;

class Server
{
    private static List<TcpClient> clients = new List<TcpClient>();

    static void Main()
    {
        TcpListener server = new TcpListener(IPAddress.Any, 8080);
        server.Start();
        Console.WriteLine("Server started... Listening for clients.");

        while (true)
        {
            try
            {
                TcpClient client = server.AcceptTcpClient();
                IPEndPoint endPoint = (IPEndPoint)client.Client.RemoteEndPoint;

                Console.WriteLine($"Player joined from {endPoint.Address}:{endPoint.Port}");

                int activePlayers = clients.Count;
                clients.Add(client);

                SendPrivateMessage(client, $"There were {activePlayers} active players before you joined.");

                BroadcastMessage("A new player joined!", client);

                System.Threading.Thread clientThread = new System.Threading.Thread(() => HandleClient(client));
                clientThread.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accepting client connection: {ex.Message}");
            }
        }
    }

    static void HandleClient(TcpClient client)
    {
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[1024];

        try
        {
            while (true)
            {
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                if (bytesRead == 0) break;

                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Received: {message}");

                // Broadcast chat messages
                BroadcastMessage($"A player says: {message}", client);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error handling client: {ex.Message}");
        }
        finally
        {
            clients.Remove(client);
            BroadcastMessage("A player has left.", client);
            client.Close();
        }
    }

    static void BroadcastMessage(string message, TcpClient sender)
    {
        byte[] data = Encoding.UTF8.GetBytes(message);
        foreach (var client in clients)
        {
            if (client != sender)
            {
                try
                {
                    NetworkStream stream = client.GetStream();
                    stream.Write(data, 0, data.Length);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error broadcasting message: {ex.Message}");
                }
            }
        }
    }

    static void SendPrivateMessage(TcpClient client, string message)
    {
        byte[] data = Encoding.UTF8.GetBytes(message);
        try
        {
            NetworkStream stream = client.GetStream();
            stream.Write(data, 0, data.Length);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending private message: {ex.Message}");
        }
    }
}
