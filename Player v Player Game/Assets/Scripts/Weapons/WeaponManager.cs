using System.Collections;
using System.Collections.Generic;
using Player_v_Player_Game.Player;
using Unity.VisualScripting;
using UnityEngine;

namespace Player_v_Player_Game.Weapons
{
    public class WeaponManager : MonoBehaviour
    {
        [SerializeField] private Transform weaponSocket;
        [SerializeField] private List<WeaponConfig> weaponConfigs;
        [SerializeField] private Transform armSocket;
        [SerializeField] private Transform arm;

        private GameObject currentWeapon;
        private GameObject currentWeaponArm;
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
            if (Input.GetKeyDown("e"))
            {
                if (currentWeaponIndex >= weaponConfigs.Count - 1)
                {
                    EquipWeapon(0);
                }
                else
                {
                    EquipWeapon(currentWeaponIndex + 1);
                }
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
                Destroy(currentWeaponArm);
            }

            WeaponConfig config = weaponConfigs[index];
            currentWeaponArm = Instantiate(config.armPreFab, armSocket);
            currentWeaponArm.name = config.weaponName + "Arm";
            currentWeaponArm.transform.localPosition = config.armPosition;
            currentWeaponArm.transform.localRotation = Quaternion.Euler(config.armRotation);
            currentWeapon = Instantiate(config.weaponPreFab, weaponSocket);
            currentWeapon.name = config.weaponName;
            currentWeapon.transform.localPosition = config.weaponPosition;
            currentWeapon.transform.localRotation = Quaternion.Euler(config.weaponRotation);

            currentWeaponIndex = index;

            anim.Rebind();
            anim.SetInteger("weaponIndex", config.weaponIndex);
        }

        private void Attack()
        {
            anim.SetTrigger("attack");
        }
    }
}
