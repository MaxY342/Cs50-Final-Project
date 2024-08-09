using UnityEngine;
using Player_v_Player_Game.Interface;

namespace Player_v_Player_Game.Weapons.Sword
{
    public class ChargeAttackProjectile : MonoBehaviour
    {
        [SerializeField] private float speed = 20f;
        [SerializeField] private int damageAmount = 30;
        [SerializeField] private Rigidbody2D rb;

        void Start()
        {
            rb.velocity = transform.right * speed;
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