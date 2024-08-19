using UnityEngine;
using Player_v_Player_Game.Interface;

namespace Player_v_Player_Game.Weapons.Revolver
{
    public class BulletProjectile : MonoBehaviour
    {
        [SerializeField] private float speed = 20f;
        [SerializeField] private int damageAmount = 20;
        [SerializeField] private Rigidbody2D rb;

        void Start()
        {
            rb.velocity = transform.up * speed;
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            Destroy(gameObject);
            IDamageable iDamageable = col.gameObject.GetComponent<IDamageable>();
            if (iDamageable != null)
            {
                iDamageable.Damage(damageAmount);
            }
        }
    }
}