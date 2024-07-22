using UnityEngine;
using UnityEngine.UI;

public class JoinRoom : MonoBehaviour
{
    public InputField roomCodeInputField;

    public void OnJoinRoomButtonClicked()
    {
        string roomCode = roomCodeInputField.text;
        CustomNetworkManager.Instance.JoinGameRoom(roomCode);
    }
}
