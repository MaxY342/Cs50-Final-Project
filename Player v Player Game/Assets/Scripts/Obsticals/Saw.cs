using UnityEngine;

namespace Player_v_Player_Game.Player
{
    public class Saw : MonoBehaviour
    {
        public float rotationSpeed = -180f; // degrees per second
        private HealthManager healthManager;

        void Start()
        {
            healthManager = FindObjectOfType<HealthManager>();
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
            if (collision.gameObject.CompareTag("Player"))
            {
                healthManager.RemoveHealth(10);
            }
        }
    }
}