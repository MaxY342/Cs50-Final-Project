using System.Collections;
using System.Collections.Generic;
using Player_v_Player_Game.Data;
using UnityEngine;

namespace Player_v_Player_Game.Player
{
    public class GroundCheck : MonoBehaviour
    {
        public LayerMask groundLayer;

        private Collider2D foot;
        // Start is called before the first frame update
        void Start()
        {
            foot = GetComponent<Collider2D>();
            GameData.Load();
        }

        // Update is called once per frame
        void Update()
        {
            bool isTouchingGround = foot.IsTouchingLayers(groundLayer);
            if (GameData.Instance.isTouchingGround != isTouchingGround)
            {
                GameData.Instance.isTouchingGround = isTouchingGround;
                GameData.Save();
            }
        }
    }
}
