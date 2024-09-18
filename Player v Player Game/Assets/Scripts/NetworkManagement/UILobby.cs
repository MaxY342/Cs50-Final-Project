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

        [Header("Host Join")]
        [SerializeField] TMP_InputField joinmatchinput;
        [SerializeField] Button joinbutton;
        [SerializeField] Button hostbutton;

        [SerializeField] Canvas lobbyCanvas;

        [Header("Lobby")]
        [SerializeField] Transform Players;

        [SerializeField] GameObject UIPlayer1Prefab;
        [SerializeField] GameObject UIPlayer2Prefab;
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
            GameObject existingUIPlayer1 = GameObject.FindWithTag("UIPlayer1"); 

            GameObject newUIPlayer;

            if (existingUIPlayer1 != null) // Check if a clone of UIPlayer1Prefab is already in the scene
            {
                newUIPlayer = Instantiate(UIPlayer2Prefab, Players);
            }
            else
            {
                newUIPlayer = Instantiate(UIPlayer1Prefab, Players);
            }

            newUIPlayer.GetComponent<UIPlayer>().SetPlayer(player);
        }

    }
}
