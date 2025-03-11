using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;

public class ChatUI : NetworkBehaviour
{
    public static ChatUI Instance;

    public TMP_InputField chatInput;
    public TextMeshProUGUI chatLog;
    public Button sendButton;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        if (sendButton != null)
        {
            sendButton.onClick.AddListener(SendChatMessage);
        }
    }

    public void SendChatMessage()
    {
        if (!string.IsNullOrEmpty(chatInput.text))
        {
            CmdSendChatMessage(chatInput.text);
            chatInput.text = "";
            chatInput.ActivateInputField();
        }
    }

    [Command(requiresAuthority = false)]
    void CmdSendChatMessage(string message, NetworkConnectionToClient sender = null)
    {
        string fullMessage = $"Player {sender.connectionId}: {message}";
        RpcReceiveChatMessage(fullMessage);
    }

    [ClientRpc]
    void RpcReceiveChatMessage(string message)
    {
        DisplayMessage(message);
    }

    public void DisplayMessage(string message)
    {
        if (chatLog != null)
        {
            chatLog.text += "\n" + message;
        }
    }
}
