using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player_v_Player_Game.Player
{
    public class AimAtCursor : MonoBehaviour
    {
        public Transform arm;
        private bool flipped = false;
        private TrackCursor trackCursor;

        // Start is called before the first frame update
        void Start()
        {  
            trackCursor = GetComponent<TrackCursor>();
        }

        // Update is called once per frame
        void Update()
        {
            RotateArmTowardsCursor();
        }

        void RotateArmTowardsCursor()
        {
            float angle = trackCursor.GetCursorAngleFromObj(arm);

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