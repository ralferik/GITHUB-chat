using UnityEngine;
using Mirror;

public class NetworkChatManager : NetworkManager
{
    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        base.OnServerConnect(conn);
        Debug.Log($"Client {conn.connectionId} connected");
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        Debug.Log($"Client {conn.connectionId} disconnected");
        base.OnServerDisconnect(conn);
    }
}
