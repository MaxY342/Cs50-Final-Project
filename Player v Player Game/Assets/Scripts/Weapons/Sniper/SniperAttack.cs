using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player_v_Player_Game.Player;
using Unity.VisualScripting;

namespace Player_v_Player_Game.Weapons.Revolver
{
    public class SniperAttack : MonoBehaviour
    {
        [SerializeField] private GameObject bulletPreFab;
        [SerializeField] private Transform firePoint;

        private TrackCursor trackCursor;
        private AmmoManager ammoManager;
        // Start is called before the first frame update
        void Start()
        {
            trackCursor = GetComponent<TrackCursor>();
            ammoManager = GetComponent<AmmoManager>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!ammoManager.OutOfAmmo() && !ammoManager.reloading)
                {
                    Shoot();
                }
            }
        }

        public void Shoot()
        {
            float rotation = trackCursor.GetCursorAngleFromObj(firePoint) - 90;
            Instantiate(bulletPreFab, firePoint.position, Quaternion.Euler(0, 0, rotation));
            ammoManager.UseAmmo(1);
        }
    }
}
