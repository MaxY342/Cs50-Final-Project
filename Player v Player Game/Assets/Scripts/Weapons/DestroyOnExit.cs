using UnityEngine;

namespace Player_v_Player_Game.Weapons
{
    public class DestroyOnExit : MonoBehaviour
    {
        void OnTriggerExit2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Background"))
            {
                Destroy(gameObject);
            }
        }
    }
}