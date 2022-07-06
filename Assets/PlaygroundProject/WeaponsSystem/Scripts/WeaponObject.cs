using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Stats", menuName = "Weapon")]
public class WeaponObject : ScriptableObject
{
    public float
        damage,
        rateOfFire,
        range,
        ammoCapacity,
        magazineSize;
    public string
        name;
    public bool
        heavy;
}
