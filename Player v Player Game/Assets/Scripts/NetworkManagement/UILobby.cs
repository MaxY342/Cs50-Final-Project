using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Lobby
{
    public class UILobby : MonoBehaviour
    {

        public static UILobby instance;

        [Header ("Host Join")]
        [SerializeField] TMP_InputField joinmatchinput; 
        [SerializeField] Button joinbutton;
        [SerializeField] Button hostbutton;

        [SerializeField] Canvas lobbyCanvas;

        [Header("Lobby")]
        [SerializeField] Transform Players;

        [SerializeField] GameObject UIPlayerPrefab;
        [SerializeField] TMP_Text Code;
        [SerializeField] TMP_Text text;
        [SerializeField] TMP_Text matchIDText;

        void Start()
        {
            instance = this;
        }

        // Method to host a match
        public void Host()
        {
            joinmatchinput.interactable = false;
            joinbutton.interactable = false;
            hostbutton.interactable = false;
            Player.localPlayer.HostGame();
        }

        public void HostSuccess(bool success)
        {
            if (success)
            {
                lobbyCanvas.enabled = true;
                SpawnPlayerPrefab(Player.localPlayer);
                matchIDText.text = Player.localPlayer._matchID;
            }
            else
            {
                joinmatchinput.interactable = true;
                joinbutton.interactable = true;
                hostbutton.interactable = true;
            }
        }

        // Method to join a match
        public void Join()
        {
            joinmatchinput.interactable = false;
            joinbutton.interactable = false;
            hostbutton.interactable = false;
            Player.localPlayer.JoinGame(joinmatchinput.text);
        }

        public void JoinSuccess(bool success)
        {
            if (success)
            {
                lobbyCanvas.enabled = true;
                SpawnPlayerPrefab(Player.localPlayer);
                matchIDText.text = Player.localPlayer._matchID;
            }
            else
            {
                joinmatchinput.interactable = true;
                joinbutton.interactable = true;
                hostbutton.interactable = true;
            }
        }

        public void SpawnPlayerPrefab(Player player)
        {
            GameObject newUIPlayer = Instantiate(UIPlayerPrefab, Players);
            newUIPlayer.GetComponent<UIPlayer>().SetPlayer(player);
        }
    }
}
