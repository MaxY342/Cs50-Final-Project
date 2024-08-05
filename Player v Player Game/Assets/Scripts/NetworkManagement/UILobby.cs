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

        [SerializeField] TMP_InputField joinmatchinput; 
        [SerializeField] Button joinbutton;
        [SerializeField] Button hostbutton;

        [SerializeField] Canvas lobbyCanvas;


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
            }
            else
            {
                joinmatchinput.interactable = true;
                joinbutton.interactable = true;
                hostbutton.interactable = true;
            }
        }
    }
}
