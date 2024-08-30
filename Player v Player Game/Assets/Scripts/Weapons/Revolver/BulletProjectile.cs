using UnityEngine;
using Player_v_Player_Game.Interface;

namespace Player_v_Player_Game.Weapons.Revolver
{
    public class BulletProjectile : MonoBehaviour
    {
        [SerializeField] private float speed = 20f;
        [SerializeField] private int damageAmount = 20;
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private GameObject bulletImpactPrefab;

        void Start()
        {
            rb.velocity = transform.up * speed;
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            Instantiate(bulletImpactPrefab, transform.position, transform.rotation);
            Destroy(gameObject);
            IDamageable iDamageable = collision.gameObject.GetComponent<IDamageable>();
            if (iDamageable != null)
            {
                iDamageable.Damage(damageAmount);
            }
        }
    }
}