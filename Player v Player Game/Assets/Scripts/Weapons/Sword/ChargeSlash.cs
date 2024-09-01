using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Player_v_Player_Game.Player;
using TMPro;
using Player_v_Player_Game.Weapons;
using UnityEditor;

namespace Player_v_Player_Game.Weapons.Sword
{
    public class ChargeSlash : MonoBehaviour
    {
        [SerializeField] private GameObject chargeSlashPrefab;
        [SerializeField] private Image cooldownOverlay;
        [SerializeField] private TMP_Text cooldownText;

        private Transform firePoint;
        private TrackCursor trackCursor;
        private bool isCooldown = false;
        private float cooldownTime = 10.0f;
        private CooldownManager cooldownManager = new CooldownManager();

        void Start()
        {
            trackCursor = GetComponent<TrackCursor>();
            firePoint = transform.Find("CrescentSpawnPoint");
            cooldownText.gameObject.SetActive(false);
            cooldownOverlay.fillAmount = 0.0f;
        }

        void Update()
        {
            if (Input.GetKeyDown("q"))
            {
                CastChargeSlash();
            }
        }

        public void CastChargeSlash()
        {
            if (!isCooldown)
            {
                float rotation = trackCursor.GetCursorAngleFromObj(firePoint);
                Instantiate(chargeSlashPrefab, firePoint.position, Quaternion.Euler(0, 0, rotation));
                StartCoroutine(HandleCooldown());
            }
        }

        private IEnumerator HandleCooldown()
        {
            isCooldown = true;
            yield return StartCoroutine(cooldownManager.ApplyCooldown(cooldownTime, cooldownOverlay, cooldownText));
            isCooldown = false;
        }
    }

}
