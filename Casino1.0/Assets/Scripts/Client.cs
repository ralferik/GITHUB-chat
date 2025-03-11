using System;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class Client : MonoBehaviour
{
    private TcpClient client;
    private NetworkStream stream;
    private byte[] buffer = new byte[1024];
    private bool isConnected = false;

    void Start()
    {
        Debug.Log("Client script started.");
        ConnectToServer();
    }

    void ConnectToServer()
    {
        try
        {
            Debug.Log("Attempting to connect to server...");
            client = new TcpClient("127.0.0.1", 8080);
            stream = client.GetStream();
            isConnected = true;
            Debug.Log("Connected to server!");

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
            if (!isConnected || client == null || !client.Connected)
                return;

            int bytesRead = stream.EndRead(ar);
            if (bytesRead > 0)
            {
                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Debug.Log($"[SERVER]: {message}");

                stream.BeginRead(buffer, 0, buffer.Length, ReceiveMessage, null);
            }
        }
        catch (Exception e)
        {
            if (isConnected)
            {
                Debug.LogError("Error receiving message: " + e.Message);
            }
        }
    }

    public void SendMessageToServer(string message)
    {
        if (client == null || !client.Connected) return;

        byte[] data = Encoding.UTF8.GetBytes(message);
        stream.Write(data, 0, data.Length);
        Debug.Log("Sent to server: " + message);
    }

    void OnApplicationQuit()
    {
        if (client != null && isConnected)
        {
            SendMessageToServer("Player has left the game.");
            Debug.Log("Disconnected from the server");
            isConnected = false;
            stream.Close();
            client.Close();
        }
    }
}
