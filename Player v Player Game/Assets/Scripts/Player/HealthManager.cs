using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Player_v_Player_Game.Data;
using Player_v_Player_Game.Interface;

namespace Player_v_Player_Game.Player
{
    public class HealthManager : MonoBehaviour, IDamageable
    {
        public Slider healthBar;
        private int currentHealth;
        // Start is called before the first frame update
        void Start()
        {
            GameData.Load();
            currentHealth = GameData.Instance.maxHealth;
            LoadHealthBar();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void RemoveHealth(int health)
        {
            currentHealth -= health;
            currentHealth = Mathf.Clamp(currentHealth, 0, GameData.Instance.maxHealth);
            healthBar.value = currentHealth;
            if (currentHealth == 0)
            {
                Die();
            }
        }

        public void LoadHealthBar()
        {
            healthBar.maxValue = GameData.Instance.maxHealth;
            healthBar.value = GameData.Instance.maxHealth;
        }

        public void IncreaseHealth(int health)
        {
            currentHealth += health;
            currentHealth = Mathf.Clamp(currentHealth, 0, GameData.Instance.maxHealth);
            healthBar.value = currentHealth;
        }

        public void Damage(int damageAmount)
        {
            RemoveHealth(damageAmount);
        }

        private void Die()
        {
            Destroy(gameObject);
        }
    }
}