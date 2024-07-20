using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Player_v_Player_Game.Data;

namespace Player_v_Player_Game.Player
{
    public class HealthManager : MonoBehaviour
    {
        public Slider healthBar;
        // Start is called before the first frame update
        void Start()
        {
            GameData.Load();
            LoadHealthBar();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void RemoveHealth(int health)
        {
            GameData.Instance.health -= health;
            healthBar.value = GameData.Instance.health;
        }

        public void LoadHealthBar()
        {
            healthBar.maxValue = GameData.health;
            healthBar.value = GameData.health;
        }

        public void IncreaseHealth(int health)
        {
            GameData.Instance.health += health;
            GameData.Instance.health = Mathf.Clamp(GameData.Instance.health, 0, GameData.health);
            healthBar.value = GameData.Instance.health;
        }
    }
}