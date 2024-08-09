using UnityEngine;

namespace Player_v_Player_Game.Player
{
    public class TrackCursor : MonoBehaviour
    {
        public float GetCursorAngleFromObj(Transform obj)
        {
            Vector3 mouseScreenPosition = Input.mousePosition;
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
            mouseWorldPosition.z = 0;

            Vector3 directionToMouse = mouseWorldPosition - obj.position;

            float angle = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg;

            return angle;
        }
    }
}