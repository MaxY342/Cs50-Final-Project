using System;
using UnityEngine;

namespace Player_v_Player_Game.Data
{
    [Serializable]
    public class GameData
    {
        public static GameData Instance;
        public bool isTouchingGround = false;
        public int health = 100;
        public int jumps = 1;
        public float dmgAmp = 0f;

        public static void Load()
        {
            Instance = SaveSystem.LoadGameData();
        }

        public static void Save()
        {
            SaveSystem.SaveGameData(Instance);
        }
    }
}