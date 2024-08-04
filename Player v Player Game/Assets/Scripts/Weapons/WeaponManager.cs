using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player_v_Player_Game.Weapons
{
    public class WeaponManager : MonoBehaviour
    {
        [SerializeField] private Transform weaponSocket;
        [SerializeField] private List<WeaponConfig> weaponConfigs;

        private GameObject currentWeapon;
        private int currentWeaponIndex = 0;
        private Animator anim;

        // Start is called before the first frame update
        void Start()
        {
            anim = GetComponent<Animator>();
            EquipWeapon(0);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Attack();
            }
        }

        private void EquipWeapon(int index)
        {
            if (index < 0 || index >= weaponConfigs.Count)
            {
                return;
            }

            if (currentWeapon != null)
            {
                Destroy(currentWeapon);
            }

            WeaponConfig config = weaponConfigs[index];
            currentWeapon = Instantiate(config.weaponPreFab, weaponSocket.position + config.weaponPosition, weaponSocket.rotation * Quaternion.Euler(config.weaponRotation), weaponSocket);

            currentWeaponIndex = index;

            anim.SetInteger("weaponIndex", config.weaponIndex);
        }

        private void Attack()
        {
            anim.SetTrigger("attack");
        }
    }
}
