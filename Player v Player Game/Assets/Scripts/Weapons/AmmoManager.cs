using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Player_v_Player_Game.Weapons
{
    public class AmmoManager : MonoBehaviour
    {
        [SerializeField] private GameObject ammoPrefab;
        [SerializeField] private Transform position;
        [SerializeField] private int numOfAmmo;
        [SerializeField] private Vector3 ammoVerticalOffset;
        [SerializeField] private Vector3 ammoHorizontalOffset;
        [SerializeField] private float reloadSpeed;
        [SerializeField] private Image cooldownOverlay;
        public bool reloading = false;

        private List<GameObject> totalAmmo;
        private GameObject currentAmmo;
        private CooldownManager cooldownManager = new CooldownManager();

        void Start()
        {
            cooldownOverlay.fillAmount = 0.0f;
            totalAmmo = new List<GameObject>();
            LoadAmmo(numOfAmmo, position);
        }

        void Update()
        {
            if (OutOfAmmo() || Input.GetKeyDown("r"))
            {
                if (!reloading && !FullAmmo())
                {
                    StartCoroutine(Reload(reloadSpeed));
                }
            }
        }

        public void LoadAmmo(int amount, Transform position)
        {
            int counter = 0;
            for (int i = 0; i < amount; i++)
            {
                // instantiate ammo at position
                currentAmmo = Instantiate(ammoPrefab, position);

                // move ammo based on offset and counter
                currentAmmo.transform.localPosition += ammoHorizontalOffset * counter;

                // add ammo to the total list
                totalAmmo.Add(currentAmmo);

                // increment counter
                counter++;

                // when counter reaches 5, vertically shift all ammo
                if (counter == 5)
                {
                    foreach (GameObject ammo in totalAmmo)
                    {
                        ammo.transform.localPosition += ammoVerticalOffset;
                    }

                    // reset counter to 0 after a vertical shift
                    counter = 0;
                }
            }
        }

        public void UseAmmo(int amount)
        {
            int counter = 0;
            for (int i = totalAmmo.Count - 1; i >= 0; i--)
            {
                if (totalAmmo[i].activeSelf)
                {
                    totalAmmo[i].SetActive(false);
                    counter++;
                }
                if (counter == amount)
                {
                    break;
                }
            }
        }

        public void ReloadAmmo(int amount)
        {
            int counter = 0;
            for (int i = 0; i < totalAmmo.Count; i++)
            {
                if (!totalAmmo[i].activeSelf)
                {
                    totalAmmo[i].SetActive(true);
                    counter++;
                }
                if (counter == amount)
                {
                    break;
                }
            }
        }

        public bool OutOfAmmo()
        {
            foreach(GameObject ammo in totalAmmo)
            {
                if (ammo.activeSelf)
                {
                    return false;
                }
            }
            return true;
        }

        public bool FullAmmo()
        {
            foreach(GameObject ammo in totalAmmo)
            {
                if (!ammo.activeSelf)
                {
                    return false;
                }
            }
            return true;
        }

        public IEnumerator Reload(float time)
        {
            reloading = true;
            yield return StartCoroutine(cooldownManager.ApplyCooldown(time, cooldownOverlay));
            ReloadAmmo(numOfAmmo);
            reloading = false;
        }
    }
}
