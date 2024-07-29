using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player_v_Player_Game.Player
{
    public class AimAtCursor : MonoBehaviour
    {
        public Transform arm;
        private bool flipped = false;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            RotateArmTowardsCursor();
        }

        void RotateArmTowardsCursor()
        {
            Vector3 mouseScreenPosition = Input.mousePosition;
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
            mouseWorldPosition.z = 0;

            Vector3 directionToMouse = mouseWorldPosition - arm.position;

            float angle = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg;

            if (angle > 90 || angle < -90)
            {
                if (!flipped)
                {
                    Flip();
                }
                angle -= 180;
            }
            else
            {
                if (flipped)
                {
                    Flip();
                }
            }

            arm.rotation = Quaternion.Euler(0, 0, angle);
        }

        void Flip()
        {
            flipped = !flipped;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }
}