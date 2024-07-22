using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class LobbyCode : MonoBehaviour
{
    public Text roomCodeText;
    private CustomNetworkManager networkManager;

    void Start()
    {
        networkManager = FindObjectOfType<CustomNetworkManager>();
        CreateGameRoom();
    }

    public void CreateGameRoom()
    {
        string roomCode = GenerateRoomCode(); // Generate a room code
        roomCodeText.text = "Code: " + roomCode; // Display room code
        networkManager.AddRoomCode(roomCode, NetworkServer.localConnection); // Add the room code to the network manager with host connection
    }

    private string GenerateRoomCode()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        char[] code = new char[6];
        for (int i = 0; i < code.Length; i++)
        {
            code[i] = chars[Random.Range(0, chars.Length)];
        }
        return new string(code);
    }
}
