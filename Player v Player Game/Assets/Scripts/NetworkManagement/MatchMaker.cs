using System;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Security.Cryptography;
using System.Text;

namespace Lobby
{
    [Serializable]
    public class Match
    {
        public string MatchId;

        public Match(string matchId)
        {
            MatchId = matchId;
        }

        public Match() { }
    }

    [Serializable]
    public class SyncListMatch : SyncList<Match>
    {
        // Custom serialization logic can be added here if needed.
    }

    public class MatchMaker : NetworkBehaviour
    {
        public static MatchMaker Instance;

        public readonly SyncListMatch Matches = new SyncListMatch();
        public readonly SyncList<string> MatchIds = new SyncList<string>();
        public readonly SyncList<int> PlayerIds = new SyncList<int>();


        private static readonly string Characters = "abcdefghijklmnopqrstuvwxyz0123456789";

        private void Start()
        {
            Instance = this;
        }

        public bool HostGame(string matchId, NetworkIdentity player)
        {
            if (player == null)
            {
                Debug.LogError("Player NetworkIdentity is null.");
                return false;
            }

            int playerId = (int)player.netId; // Convert NetworkIdentity's netId to int

            if (!MatchIds.Contains(matchId))
            {
                MatchIds.Add(matchId);
                Matches.Add(new Match(matchId)); // Store player ID as int
                PlayerIds.Add(playerId); // Store player ID separately
                Debug.Log($"Match generated: ID = {matchId}, Player ID = {playerId}");
                return true;
            }
            else
            {
                Debug.LogError("Match ID already exists");
                return false;
            }
        }

        public bool JoinGame(string matchId, NetworkIdentity player)
        {
            if (player == null)
            {
                Debug.LogError("Player NetworkIdentity is null.");
                return false;
            }

            int playerId = (int)player.netId; // Convert NetworkIdentity's netId to int

            if (MatchIds.Contains(matchId))
            {
                for (int i = 0; i < Matches.Count; i++)
                {
                    if (Matches[i].MatchId == matchId)
                    {
                        PlayerIds.Add(playerId);
                    }
                }
                Debug.Log($"Match joined");
                return true;
            }
            else
            {
                Debug.LogError("Match ID does not exist");
                return false;
            }
        }

        public static string GetRandomMatchId()
        {
            string id = string.Empty;
            System.Random random = new System.Random();

            for (int i = 0; i < 6; i++)
            {
                int index = random.Next(Characters.Length);
                id += Characters[index];
            }
            Debug.Log($"Generated match ID: {id}");
            return id;
        }
    }

    public static class MatchExtensions
    {
        public static Guid ToGuid(this string id)
        {
            MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
            byte[] inputBytes = Encoding.Default.GetBytes(id);
            byte[] hashBytes = provider.ComputeHash(inputBytes);

            return new Guid (hashBytes);
        }
    }
}
