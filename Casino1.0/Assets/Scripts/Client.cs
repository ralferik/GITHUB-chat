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

            // Start listening for messages
            stream.BeginRead(buffer, 0, buffer.Length, ReceiveMessage, null);
        }
        catch (Exception e)
        {
            Debug.LogError("Error connecting to server: " + e.Message);
        }
    }

    void ReceiveMessage(IAsyncResult ar)
    {
        int bytesRead = stream.EndRead(ar);
        if (bytesRead > 0)
        {
            string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Debug.Log("Received from server: " + message);
            stream.BeginRead(buffer, 0, buffer.Length, ReceiveMessage, null);
        }
    }

    public void SendMessageToServer(string message)
    {
        if (client == null) return;

        byte[] data = Encoding.UTF8.GetBytes(message);
        stream.Write(data, 0, data.Length);
        Debug.Log("Sent to server: " + message);
    }

    void OnApplicationQuit()
    {
        if (client != null)
        {
            stream.Close();
            client.Close();
        }
    }
}
