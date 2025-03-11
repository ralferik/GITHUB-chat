using Mirror;
using UnityEngine;

public class CustomNetworkManager : NetworkManager
{
    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        base.OnServerConnect(conn);
        Debug.Log($"Player connected: {conn.connectionId}");
        SendChatMessage($"[SERVER]: Player {conn.connectionId} joined the game.");
    }

    [Server]
    public void SendChatMessage(string message)
    {
        RpcReceiveChatMessage(message);
    }

    [ClientRpc]
    void RpcReceiveChatMessage(string message)
    {
        Debug.Log(message);
        ChatUI.Instance.DisplayMessage(message);
    }
}
