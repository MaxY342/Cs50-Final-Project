using UnityEngine;
using UnityEngine.UI;

public class RoomCodeGenerator : MonoBehaviour
{
    public Text roomCodeText;

    void Start()
    {
        string generatedCode = GenerateRoomCode(); // Generate room code
        roomCodeText.text = generatedCode; // Display room code
    }

    string GenerateRoomCode()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        char[] code = new char[4];
        for (int i = 0; i < code.Length; i++)
        {
            code[i] = chars[Random.Range(0, chars.Length)];
        }
        return new string(code); // Convert char[] to string and return
    }
}
