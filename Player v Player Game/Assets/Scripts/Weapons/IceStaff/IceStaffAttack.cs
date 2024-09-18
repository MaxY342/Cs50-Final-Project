using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player_v_Player_Game.Player;
using Unity.VisualScripting;

namespace Player_v_Player_Game.Weapons.IceStaff
{
    public class IceStaffAttack : MonoBehaviour
    {
        [SerializeField] private GameObject attackPrefab;
        [SerializeField] private Transform firePoint;

        private TrackCursor trackCursor;
        private AmmoManager ammoManager;

        void Start()
        {
            trackCursor = GetComponent<TrackCursor>();
            ammoManager = GetComponent<AmmoManager>();
        }

        void Update()
        {
            if (GetMouseButtonDown(0))
            {
                if (!ammoManager.reloading)
                {
                    Attack();
                }
            }
        }

        private Attack()
        {
            float rotation = trackCursor.GetCursorAngleFromObj(firePoint) - 90;
            Instantiate(bulletPreFab, firePoint.position, Quaternion.Euler(0, 0, rotation));
            ammoManager.UseAmmo(1);
        }
    }
}