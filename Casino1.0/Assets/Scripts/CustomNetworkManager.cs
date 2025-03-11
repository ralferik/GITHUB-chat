using UnityEngine;
using Mirror;

public class NetworkChatManager : NetworkManager
{
    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        base.OnServerConnect(conn);
        Debug.Log($"Client {conn.connectionId} connected");

        if (ChatUI.Instance != null)
        {
            ChatUI.Instance.DisplayMessage($"Player {conn.connectionId} joined the chat.");
        }
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        Debug.Log($"Client {conn.connectionId} disconnected");
        base.OnServerDisconnect(conn);

        if (ChatUI.Instance != null)
        {
            ChatUI.Instance.DisplayMessage($"Player {conn.connectionId} left the chat.");
        }
    }
}
