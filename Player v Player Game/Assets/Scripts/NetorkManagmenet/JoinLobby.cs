using UnityEngine;
using UnityEngine.UI;

public class JoinLobbyUI : MonoBehaviour
{
    public InputField roomCodeInput;
    private CustomNetworkManager networkManager;

    void Start()
    {
        networkManager = FindObjectOfType<CustomNetworkManager>();
    }

    public void JoinLobby()
    {
        string roomCode = roomCodeInput.text;
        networkManager.JoinGameRoom(roomCode);
    }
}
