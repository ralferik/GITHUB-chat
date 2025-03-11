using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Net.Sockets;
using System.Text;

public class Client : MonoBehaviour
{
    public TMP_InputField messageInputField;
    public Button sendButton;
    public TextMeshProUGUI chatLog;  // <-- Chat log text element

    private TcpClient client;
    private NetworkStream stream;
    private byte[] buffer = new byte[1024];

    void Start()
    {
        ConnectToServer();

        if (sendButton != null)
        {
            sendButton.onClick.AddListener(SendMessageFromUI);
        }
    }

    void ConnectToServer()
    {
        try
        {
            client = new TcpClient("127.0.0.1", 8080);
            stream = client.GetStream();

            Debug.Log("Connected to server!");
            AppendMessage("Connected to server!");

            stream.BeginRead(buffer, 0, buffer.Length, ReceiveMessage, null);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error connecting to server: " + e.Message);
        }
    }

    void ReceiveMessage(System.IAsyncResult ar)
    {
        try
        {
            int bytesRead = stream.EndRead(ar);
            if (bytesRead > 0)
            {
                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Debug.Log("Received: " + message);
                AppendMessage(message);  // Show in chat log

                stream.BeginRead(buffer, 0, buffer.Length, ReceiveMessage, null);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error receiving message: " + e.Message);
        }
    }

    public void SendMessageFromUI()
    {
        if (messageInputField != null && !string.IsNullOrWhiteSpace(messageInputField.text))
        {
            SendMessageToServer(messageInputField.text);
            messageInputField.text = ""; // Clear input
        }
    }

    public void SendMessageToServer(string message)
    {
        if (client == null || string.IsNullOrEmpty(message)) return;

        byte[] data = Encoding.UTF8.GetBytes(message);
        stream.Write(data, 0, data.Length);
        Debug.Log("Sent: " + message);
        AppendMessage("You: " + message);
    }

    void AppendMessage(string message)
    {
        if (chatLog != null)
        {
            chatLog.text += message + "\n";
        }
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
