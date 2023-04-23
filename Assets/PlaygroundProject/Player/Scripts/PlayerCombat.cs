using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : Combat
{
    public enum WeaponSlot
    {
        None = 0,
        Primary = 1,
        Secundary = 2,
        Special = 3
    }

    public WeaponSlot equipedSlot;
    private WeaponSlot lastSlot;
    public Weapon primaryWeapon, secundaryWeapon, specialWeapon;
    private Weapon drawnedWeapon;
    private bool canChangeWeapon = true;

    private void Update()
    {
        InputHolder();
    }

    void InputHolder()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) Attack();
        if (Input.GetKeyDown(KeyCode.Alpha1)) ChangeWeapon(WeaponSlot.Primary);
        if (Input.GetKeyDown(KeyCode.Alpha2)) ChangeWeapon(WeaponSlot.Secundary);
        if (Input.GetKeyDown(KeyCode.Alpha3)) ChangeWeapon(WeaponSlot.Special);
        if (Input.GetKeyDown(KeyCode.Z)) ChangeWeapon(WeaponSlot.None);
        if (Input.GetKeyDown(KeyCode.Q)) ChangeWeapon(lastSlot);
    }

    void ChangeWeapon(WeaponSlot newWeapon)
    {
        if (!canChangeWeapon) return;

        if (newWeapon == equipedSlot)
        {
            if (newWeapon == WeaponSlot.None) return;

            Debug.Log("Guardando arma <color=purple>" + equipedSlot + "</color>");

            drawnedWeapon?.Sheath();
            drawnedWeapon = null;

            equipedSlot = WeaponSlot.None;
        }

        else
        {
            if(lastSlot != equipedSlot) lastSlot = equipedSlot;
            equipedSlot = newWeapon;

            switch (equipedSlot)
            {
                case WeaponSlot.None:
                    Debug.Log("Guardando arma <color=purple>" + lastSlot + "</color>");
                    equipedSlot = WeaponSlot.None;

                    drawnedWeapon?.Sheath();
                    drawnedWeapon = null;

                    break;

                case WeaponSlot.Primary:
                    Debug.Log("Trocando para arma primária");

                    drawnedWeapon?.Sheath();
                    drawnedWeapon = primaryWeapon;

                    equipedSlot = WeaponSlot.Primary;
                    break;

                case WeaponSlot.Secundary:
                    Debug.Log("Trocando para arma secundária");

                    drawnedWeapon?.Sheath();
                    drawnedWeapon = secundaryWeapon;

                    equipedSlot = WeaponSlot.Secundary;
                    break;

                case WeaponSlot.Special:
                    Debug.Log("Trocando para arma especial");

                    drawnedWeapon?.Sheath();
                    drawnedWeapon = specialWeapon;

                    equipedSlot = WeaponSlot.Special;
                    break;
            }

            if(drawnedWeapon != null) StartCoroutine(DrawAfterSheath());
        }
    }

    IEnumerator DrawAfterSheath()
    {
        canChangeWeapon = false;
        yield return new WaitForSeconds(.2f);
        drawnedWeapon.Draw();
        canChangeWeapon = true;
    }

}