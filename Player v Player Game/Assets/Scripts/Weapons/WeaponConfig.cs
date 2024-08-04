using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponConfig", menuName = "Weapon/WeaponConfig")]
public class WeaponConfig : ScriptableObject
{
    public GameObject weaponPreFab;
    public Vector3 weaponPosition;
    public Vector3 weaponRotation;
    public int weaponIndex;
}
