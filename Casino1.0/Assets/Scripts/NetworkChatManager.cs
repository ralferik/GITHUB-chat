using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class NetworkChatManager : NetworkBehaviour // ✅ Fix: Inherit from NetworkBehaviour
{
    public TMP_InputField chatInputField;
    public TextMeshProUGUI chatText;
    public ScrollRect scrollRect;

    private static NetworkChatManager instance;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SendChatMessage();
        }
    }

    public void SendChatMessage()
    {
        if (chatInputField.text.Trim() != "")
        {
            CmdSendChatMessage(chatInputField.text);
            chatInputField.text = "";
            chatInputField.ActivateInputField();
        }
    }

    [Command] // ✅ Fix: This must be inside a NetworkBehaviour
    void CmdSendChatMessage(string message, NetworkConnectionToClient sender = null)
    {
        RpcReceiveChatMessage($"{sender.connectionId}: {message}");
    }

    [ClientRpc] // ✅ Fix: This must be inside a NetworkBehaviour
    void RpcReceiveChatMessage(string message)
    {
        chatText.text += "\n" + message;
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
    }
}
