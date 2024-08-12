using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Player_v_Player_Game.Player;
using TMPro;
using Player_v_Player_Game.Weapons;

namespace Player_v_Player_Game.Weapons.Sword
{
    public class ChargeAttack : MonoBehaviour
    {
        [SerializeField] private GameObject chargeAttackPrefab;
        [SerializeField] private Image cooldownOverlay;
        [SerializeField] private TMP_Text cooldownText;

        private Transform firePoint;
        private TrackCursor trackCursor;
        private bool isCooldown = false;
        private float cooldownTime = 10.0f;
        private CooldownManager cooldownManager = new CooldownManager();

        void Start()
        {
            cooldownText.gameObject.SetActive(false);
            cooldownOverlay.fillAmount = 0.0f;
            trackCursor = GetComponent<TrackCursor>();
            firePoint = transform.Find("CrescentSpawnPoint");
            if (firePoint == null)
            {
                Debug.LogError("CrescentSpawnPoint not found on the sword prefab.");
            }
        }

        void Update()
        {
            if (Input.GetKeyDown("q"))
            {
                ChargeSlash();
            }
        }

        private void ChargeSlash()
        {
            if (!isCooldown)
            {
                float rotation = trackCursor.GetCursorAngleFromObj(firePoint);
                Instantiate(chargeAttackPrefab, firePoint.position, Quaternion.Euler(0, 0, rotation));
                StartCoroutine(HandleCooldown());
            }
        }

        private IEnumerator HandleCooldown()
        {
            isCooldown = false;
            yield return StartCoroutine(cooldownManager.ApplyAbilityCooldown(cooldownTime, cooldownOverlay, cooldownText));
            isCooldown = true;
        }
    }

}
