using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace Lobby
{
    public class Player : NetworkBehaviour
    {
        public static Player localPlayer;
        [SyncVar] public string _matchID;

        NetworkMatch networkMatch; 

        void Start()
        {
            if (isLocalPlayer)
            {
                localPlayer = this;
            }
            networkMatch = GetComponent<NetworkMatch>();
        }

        public void HostGame()
        {
            string matchID = MatchMaker.GetRandomMatchId();
            CmdHostGame(matchID);
        }

        [Command]
        void CmdHostGame(string matchID)
        {
            NetworkIdentity networkIdentity = GetComponent<NetworkIdentity>();
            if (networkIdentity == null)
            {
                Debug.LogError("NetworkIdentity component is missing.");
                return;
            }
            _matchID = matchID;
            if (MatchMaker.Instance.HostGame(matchID, networkIdentity))
            {
                Debug.Log("Game Hosted Successfully");
                networkMatch.matchId = matchID.ToGuid();
                TargetHostGame(true, matchID);
            }
            else
            {
                Debug.LogError("Game Hosting Failed");
                TargetHostGame(false, matchID);

            }
        }

        [TargetRpc]
        void TargetHostGame(bool success, string matchID)
        {
            Debug.Log($"MatchID: {_matchID} == {matchID}");
            UILobby.instance.HostSuccess(success);
        }

        public void JoinGame(string inputID)
        {
            CmdJoinGame(inputID);
        }

        [Command]
        void CmdJoinGame(string matchID)
        {
            NetworkIdentity networkIdentity = GetComponent<NetworkIdentity>();
            if (networkIdentity == null)
            {
                Debug.LogError("NetworkIdentity component is missing.");
                return;
            }
            _matchID = matchID;
            if (MatchMaker.Instance.JoinGame(matchID, networkIdentity))
            {
                Debug.Log("Game Joined Successfully");
                networkMatch.matchId = matchID.ToGuid();
                TargetJoinGame(true, matchID);
            }
            else
            {
                Debug.LogError("Game Joining Failed");
                TargetJoinGame(false, matchID);

            }
        }

        [TargetRpc]
        void TargetJoinGame(bool success, string matchID)
        {
            Debug.Log($"MatchID: {_matchID} == {matchID}");
            UILobby.instance.JoinSuccess(success);
        }
    }
}
