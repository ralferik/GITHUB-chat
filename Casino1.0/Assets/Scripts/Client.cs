using System;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class Client : MonoBehaviour
{
    private TcpClient client;
    private NetworkStream stream;
    private byte[] buffer = new byte[1024];

    void Start()
    {
        ConnectToServer();
    }

    void ConnectToServer()
    {
        try
        {
            client = new TcpClient("127.0.0.1", 8080);
            stream = client.GetStream();
            Debug.Log("Connected to server");

            stream.BeginRead(buffer, 0, buffer.Length, ReceiveMessage, null);
        }
        catch (Exception e)
        {
            Debug.LogError("Error connecting to server: " + e.Message);
        }
    }


    void ReceiveMessage(IAsyncResult ar)
    {
        try
        {
            int bytesRead = stream.EndRead(ar);
            if (bytesRead == 0)
            {
                Debug.LogWarning("Disconnected from server.");
                Cleanup();
                return;
            }

            string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Debug.Log("Received from server: " + message);

            stream.BeginRead(buffer, 0, buffer.Length, ReceiveMessage, null);
        }
        catch (Exception)
        {
            Debug.LogWarning("Server has shut down. Disconnected.");
            Cleanup();
        }
    }

    public void SendMessageToServer(string message)
    {
        if (client == null || !client.Connected) 
        {
            Debug.LogError("Cannot send message: Not connected to server.");
            return;
        }

        byte[] data = Encoding.UTF8.GetBytes(message);
        try
        {
            stream.Write(data, 0, data.Length);
            Debug.Log("Sent to server: " + message);
        }
        catch (Exception)
        {
            Debug.LogWarning("Failed to send message. Server might be down.");
            Cleanup();
        }
    }

    void Cleanup()
    {
        if (client != null)
        {
            stream?.Close();
            client?.Close();
            client = null;
            Debug.Log("Disconnected from server.");
        }
    }

    void OnApplicationQuit()
    {
        Cleanup();
    }
}
