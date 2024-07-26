using System.Collections;
using Player_v_Player_Game.Data;
using UnityEngine;

namespace Player_v_Player_Game.Player
{
    public class StickmanMovement : MonoBehaviour
    {
        public float speed = 5f;
        public float jumpForce = 60f;

        private Rigidbody2D rb;
        private int originalJumpCount = 0;
        private bool paused;
        private Animator anim;

        void Start()
        {
            anim = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            SaveSystem.RemoveData();
            GameData.Load();
            originalJumpCount = GameData.Instance.jumps;
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        void Update()
        {
            Move();
            Jump();
        }

        void Move()
        {
            float moveInput = Input.GetAxis("Horizontal");
            rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
            if (moveInput == 0)
            {
                anim.SetBool("isRunning", false);
            }
            else
            {
                anim.SetBool("isRunning", true);
            }

            if (moveInput < 0)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }

            else if (moveInput > 0)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
        }

        void Jump()
        {
            if (Input.GetKeyDown("space") && GameData.Instance.jumps > 0)
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                GameData.Instance.jumps--;
                GameData.Save();
                Debug.Log("jumped");
                StartCoroutine(PauseUpdate(0.1f));
            }
            if (!paused)
            {
                if (GameData.Instance.isTouchingGround)
                {
                    ResetJumps();
                }
            }
        }

        IEnumerator PauseUpdate(float pauseDuration)
        {
            paused = true;
            yield return new WaitForSeconds(pauseDuration);
            paused = false;
        }

        void ResetJumps()
        {
            if (GameData.Instance.jumps != originalJumpCount)
            {
                GameData.Instance.jumps = originalJumpCount;
                GameData.Save();
                Debug.Log("Jumps reset to original count: " + originalJumpCount);
            }
        }
    }
}