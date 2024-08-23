using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player_v_Player_Game.Interface;

namespace Player_v_Player_Game.Weapons.Spear
{
    public class SpearAttack : MonoBehaviour
    {
        [SerializeField] private int damageAmount = 20;

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
