using UnityEngine;
using Player_v_Player_Game.Interface;

namespace Player_v_Player_Game.Player
{
    public class BodyPart : MonoBehaviour, IDamageable
    {
        private HealthManager healthManager;

        public void Damage(int damageAmount)
        {
            healthManager.Damage(damageAmount);
        }
    }
}