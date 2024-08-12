using System.Collections;
using System.Collections.Generic;
using Player_v_Player_Game.Interface;
using UnityEditor;
using UnityEngine;
using Player_v_Player_Game.Player;

namespace Player_v_Player_Game.Weapons.Sword
{
    public class SwordAttack : MonoBehaviour
    {
        [SerializeField] private int damageAmount = 20;

        private ChargeSlash chargeSlash;

        void Start()
        {
            chargeSlash = GetComponent<ChargeSlash>();
            if (firePoint == null)
            {
                Debug.LogError("CrescentSpawnPoint not found on the sword prefab.");
            }
        }

        void Update()
        {
            if (Input.GetKeyDown("q"))
            {
                chargeSlash.ChargeSlash();
            }
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            IDamageable iDamageable = col.gameObject.GetComponent<IDamageable>();
            if (iDamageable != null)
            {
                iDamageable.Damage(damageAmount);
            }
        }
    }
}
