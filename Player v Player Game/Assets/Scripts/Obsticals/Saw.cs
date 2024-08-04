using UnityEngine;
using Player_v_Player_Game.Player;
using Player_v_Player_Game.Interface;

namespace Player_v_Player_Game.Obsticals
{
    public class Saw : MonoBehaviour
    {
        [SerializeField] private float rotationSpeed = -180f; // degrees per second
        [SerializeField] private int damageAmount = 20;

        void Start()
        {

        }

        void Update()
        {
            SpinSaw();
        }

        private void SpinSaw()
        {
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            IDamageable iDamageable = collision.gameObject.GetComponent<IDamageable>();
            if (iDamageable != null)
            {
                iDamageable.Damage(damageAmount);
            }
        }
    }
}