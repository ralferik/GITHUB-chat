using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;

class Server
{
    private static List<TcpClient> clients = new List<TcpClient>();
    private static object lockObj = new object();

    static void Main()
    {
        int port = 8080;
        TcpListener server = new TcpListener(IPAddress.Any, port);
        server.Start();
        Console.WriteLine($"Server is running on localhost:{port}");

        while (true)
        {
            TcpClient client = server.AcceptTcpClient();
            string clientIP = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();

            lock (lockObj) { clients.Add(client); }
            Console.WriteLine($"Client connected. IP address: {clientIP}");

            Thread clientThread = new Thread(() => HandleClient(client, clientIP));
            clientThread.Start();
        }
    }

    static void HandleClient(TcpClient client, string clientIP)
    {
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[1024];

        try
        {
            string welcomeMessage = "Welcome to Princess Sofia's Casino!";
            byte[] welcomeData = Encoding.UTF8.GetBytes(welcomeMessage);
            stream.Write(welcomeData, 0, welcomeData.Length);

            while (true)
            {
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                if (bytesRead == 0) break; // Client disconnected

                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Received from client ({clientIP}): {message}");

                BroadcastMessage($"Echo: {message}", client);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error with client ({clientIP}): {ex.Message}");
        }
        finally
        {
            lock (lockObj) { clients.Remove(client); }
            Console.WriteLine($"Client disconnected. IP address: {clientIP}");
            client.Close();
        }
    }

    static void BroadcastMessage(string message, TcpClient sender)
    {
        byte[] data = Encoding.UTF8.GetBytes(message);
        lock (lockObj)
        {
            foreach (TcpClient client in clients)
            {
                if (client != sender)
                {
                    NetworkStream stream = client.GetStream();
                    stream.Write(data, 0, data.Length);
                }
            }
        }
    }
}
