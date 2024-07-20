using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Mirror;
using System.Collections.Generic;

public class CustomNetworkManager : NetworkManager
{
    public Text roomCodeText;

    private Dictionary<string, RoomData> roomCodes = new Dictionary<string, RoomData>();

    public void AddRoomCode(string roomCode, NetworkConnectionToClient hostConnection)
    {
        RoomData data = new RoomData
        {
            roomCode = roomCode,
            hostConnectionId = hostConnection.connectionId // Store the connection ID
        };
        roomCodes[roomCode] = data; // Store the room data with the host connection
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        // Instantiate player prefab
        GameObject player = Instantiate(playerPrefab);

        // Ensure the player object is spawned on the server for the specific connection
        NetworkServer.AddPlayerForConnection(conn, player);
    }

    public void JoinGameRoom(string roomCode)
    {
        if (roomCodes.ContainsKey(roomCode))
        {
            RoomData roomData = roomCodes[roomCode];

            // Find the NetworkConnection associated with hostConnectionId
            NetworkConnection hostConnection = NetworkServer.connections[roomData.hostConnectionId];

            if (hostConnection != null)
            {
                // Inform the host that a new player is joining
                JoinLobbyManager joinLobbyManager = hostConnection.identity.GetComponent<JoinLobbyManager>();
                joinLobbyManager.TargetJoinLobby(roomData);
            }
            else
            {
                Debug.Log("Host connection not found for room code: " + roomCode);
            }
        }
        else
        {
            Debug.Log("Invalid room code"); // Handle invalid room code
        }
    }
}
