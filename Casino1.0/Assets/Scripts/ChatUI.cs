using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class ChatUI : MonoBehaviour
{
    public static ChatUI Instance;

    public InputField chatInput;
    public Text chatLog;
    public Button sendButton;

    private void Awake()
    {
        Instance = this;
    }

    public void SendChatMessage()
    {
        string message = chatInput.text;
        if (!string.IsNullOrEmpty(message))
        {
            CmdSendChatMessage(message);
            chatInput.text = "";
        }
    }

    [Command]
    void CmdSendChatMessage(string message, NetworkConnectionToClient sender = null)
    {
        string fullMessage = $"Player {sender.connectionId}: {message}";
        FindObjectOfType<CustomNetworkManager>().SendChatMessage(fullMessage);
    }

    public void DisplayMessage(string message)
    {
        chatLog.text += "\n" + message;
    }
}
