using System.Collections;
using System.Collections.Generic;
using Mirror;
using Player_v_Player_Game.Player;
using Unity.VisualScripting;
using UnityEngine;

namespace Player_v_Player_Game.Obsticals
{
    public class SawObstacle : MonoBehaviour
    {
        private bool initiateObstacle = false;
        public float speed = 0.5f;
        // Start is called before the first frame update
        void Start()
        {
            initiateObstacle = false;
            StartCoroutine(ObstacleTimer());
        }

        // Update is called once per frame
        void Update()
        {
            if (initiateObstacle)
            {
                MoveObstacle();
            }
        }

        IEnumerator ObstacleTimer()
        {
            yield return new WaitForSeconds(15);
            initiateObstacle = true;
        }

        private void MoveObstacle()
        {
            transform.position += Vector3.down * speed * Time.deltaTime;
        }
    }
}