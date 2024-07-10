using UnityEngine;

namespace Player_v_Player_Game.Data
{
    public static class SaveSystem
    {
        private static string dataKey = "GameData";

        public static void SaveGameData(GameData data)
        {
            string json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(dataKey, json);
            PlayerPrefs.Save();
        }

        public static Gamedata LoadGameData()
        {
            if (PlayerPrefs.HasKey(dataKey))
            {
                string json = PlayerPrefs.GetString(dataKey);
                return JsonUtility.FromJson<GameData>(json);
            }
            else
            {
                return new Gamedata();
            }
        }
    }
}
