using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class JoinLobbyManager : NetworkBehaviour
{
    [TargetRpc]
    public void TargetJoinLobby(RoomData roomData)
    {
        // Load the lobby scene for both the host and the new player
        SceneManager.LoadScene("Lobby");

        // Optionally, use roomData.roomCode and roomData.hostConnectionId to process further logic
    }
}
