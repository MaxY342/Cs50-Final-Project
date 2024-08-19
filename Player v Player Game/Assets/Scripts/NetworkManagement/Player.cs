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
        [SyncVar] public int playerIndex;

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
            if (MatchMaker.Instance.HostGame(matchID, networkIdentity, out int assignedIndex))
            {
                Debug.Log("Game Hosted Successfully");
                networkMatch.matchId = matchID.ToGuid();
                playerIndex = assignedIndex;
                RpcSetMatchID(_matchID);
                RpcSpawnPlayerPrefab(playerIndex);
                TargetHostGame(true, matchID, playerIndex);
            }
            else
            {
                Debug.LogError("Game Hosting Failed");
                TargetHostGame(false, matchID, playerIndex);
            }
        }

        [TargetRpc]
        void TargetHostGame(bool success, string matchID, int _playerIndex)
        {
            playerIndex = _playerIndex;
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
            if (MatchMaker.Instance.JoinGame(matchID, networkIdentity, out int assignedIndex))
            {
                Debug.Log("Game Joined Successfully");
                networkMatch.matchId = matchID.ToGuid();
                playerIndex = assignedIndex;
                RpcSetMatchID(_matchID);
                RpcSpawnPlayerPrefab(playerIndex);
                TargetJoinGame(true, matchID, playerIndex);
            }
            else
            {
                Debug.LogError("Game Joining Failed");
                TargetJoinGame(false, matchID, playerIndex);
            }
        }

        [TargetRpc]
        void TargetJoinGame(bool success, string matchID, int _playerIndex)
        {
            playerIndex = _playerIndex;
            Debug.Log($"MatchID: {_matchID} == {matchID}");
            UILobby.instance.JoinSuccess(success);
        }

        [ClientRpc]
        void RpcSpawnPlayerPrefab(int assignedIndex)
        {
            playerIndex = assignedIndex;
            UILobby.instance.SpawnPlayerPrefab(this);
        }

        [ClientRpc]
        void RpcSetMatchID(string matchID)
        {
            _matchID = matchID;
        }

        public string GetCode()
        {
            return _matchID;
        }
    }
}