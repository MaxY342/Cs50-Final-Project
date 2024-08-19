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
        public List<NetworkIdentity> players = new List<NetworkIdentity>(); // Using NetworkIdentity instead of GameObject

        public Match(string matchId, NetworkIdentity player)
        {
            MatchId = matchId;
            players.Add(player);
        }

        public Match() { }
    }

    public class MatchMaker : NetworkBehaviour
    {
        public static MatchMaker Instance;

        public List<Match> Matches = new List<Match>(); // Simplified to List
        public List<string> MatchIds = new List<string>();

        private static readonly string Characters = "abcdefghijklmnopqrstuvwxyz0123456789";

        private void Awake()
        {
            Instance = this;
        }

        public bool HostGame(string matchId, NetworkIdentity player, out int playerIndex)
        {
            playerIndex = -1;
            if (player == null)
            {
                Debug.LogError("Player NetworkIdentity is null.");
                return false;
            }

            if (!MatchIds.Contains(matchId))
            {
                MatchIds.Add(matchId);
                Match newMatch = new Match(matchId, player);
                Matches.Add(newMatch);
                playerIndex = newMatch.players.Count; // Should be 1 since this is the first player
                Debug.Log($"Match generated: ID = {matchId}, Player Index = {playerIndex}");
                return true;
            }
            else
            {
                Debug.LogError("Match ID already exists");
                return false;
            }
        }

        public bool JoinGame(string matchId, NetworkIdentity player, out int playerIndex)
        {
            playerIndex = -1;
            if (player == null)
            {
                Debug.LogError("Player NetworkIdentity is null.");
                return false;
            }

            foreach (var match in Matches)
            {
                if (match.MatchId == matchId)
                {
                    match.players.Add(player);
                    playerIndex = match.players.Count;
                    Debug.Log($"Player joined match ID = {matchId}, Player Index = {playerIndex}");
                    return true;
                }
            }

            Debug.LogError("Match ID does not exist");
            return false;
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

            return new Guid(hashBytes);
        }
    }
}
