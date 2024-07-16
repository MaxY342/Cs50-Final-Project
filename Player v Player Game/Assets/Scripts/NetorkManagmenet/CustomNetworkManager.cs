using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Mirror;


public class CustomNetworkManager : NetworkManager
{
    public Text roomCodeText;

    public void StartServer1()
    {
        StartServer(); // This starts the server using Mirror's NetworkManager method
    }
    // Override to customize player object spawning
    public void OnServerAddPlayer(NetworkConnection conn)
    {
        // Instantiate player prefab
        GameObject player = Instantiate(playerPrefab);

        // Ensure the player object is spawned on the server for the specific connection
        NetworkServer.Spawn(player, conn);
    }
    // Method to create a new game room with a code
    public void CreateGameRoom()
    {
        string roomCode = GenerateRoomCode(); // Generate a room code
        roomCodeText.text = roomCode; // Display room code

        // Optionally, you can create a room-specific object or perform other setup
        // GameObject room = Instantiate(roomPrefab);
        // NetworkServer.Spawn(room);
    }

    // Method to join an existing game room with a code
    public void JoinGameRoom(string roomCode)
    {
        // Perform validation and join logic based on room code
        // For simplicity, let's assume room code validation and joining logic here
        if (roomCode == roomCodeText.text)
        {
            // Join logic
            SceneManager.LoadScene("GameScene"); // Replace "GameScene" with your actual game scene
        }
        else
        {
            Debug.Log("Invalid room code"); // Handle invalid room code
        }
    }

    // Method to generate a random room code (for demo purposes)
    private string GenerateRoomCode()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        char[] code = new char[4];
        for (int i = 0; i < code.Length; i++)
        {
            code[i] = chars[Random.Range(0, chars.Length)];
        }
        return new string(code);
    }
}
