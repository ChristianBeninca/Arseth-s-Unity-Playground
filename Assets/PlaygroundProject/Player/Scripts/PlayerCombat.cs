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
    private Weapon withdrawnedWeapon;
    private bool canChangeWeapon = true;

    private void Update()
    {
        InputHolder();

        //Weapon Animation
        if (withdrawnedWeapon != null)
        {
            withdrawnedWeapon.UpdateSway();
            if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0) 
                withdrawnedWeapon.UpdateBreath(Weapon.WalkMode.Idle);
            else if (Input.GetKey(KeyCode.LeftShift)) 
                withdrawnedWeapon.UpdateBreath(Weapon.WalkMode.Run);
            else 
                withdrawnedWeapon.UpdateBreath(Weapon.WalkMode.Walk);
        }
    }

    void InputHolder()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) Attack();
        if (Input.GetKeyDown(KeyCode.Mouse1)) withdrawnedWeapon.Aim(true);
        if (Input.GetKeyUp(KeyCode.Mouse1)) withdrawnedWeapon.Aim(false);
        if (Input.GetKeyDown(KeyCode.R)) withdrawnedWeapon.Recharge();
        if (Input.GetKeyDown(KeyCode.Alpha1)) ChangeWeapon(WeaponSlot.Primary);
        if (Input.GetKeyDown(KeyCode.Alpha2)) ChangeWeapon(WeaponSlot.Secundary);
        if (Input.GetKeyDown(KeyCode.Alpha3)) ChangeWeapon(WeaponSlot.Special);
        if (Input.GetKeyDown(KeyCode.Z)) ChangeWeapon(WeaponSlot.None);
        if (Input.GetKeyDown(KeyCode.Q)) ChangeWeapon(lastSlot);
    }

    protected override void Attack()
    {
        //base.Attack();
        withdrawnedWeapon.Shoot();
    }

    void ChangeWeapon(WeaponSlot newWeapon)
    {
        if (!canChangeWeapon) return;

        if (newWeapon == equipedSlot)
        {
            if (newWeapon == WeaponSlot.None) return;

            Debug.Log("Guardando arma <color=purple>" + equipedSlot + "</color>");

            withdrawnedWeapon?.Sheath();
            withdrawnedWeapon = null;

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

                    withdrawnedWeapon?.Sheath();
                    withdrawnedWeapon = null;

                    break;

                case WeaponSlot.Primary:
                    Debug.Log("Trocando para arma primária");

                    withdrawnedWeapon?.Sheath();
                    withdrawnedWeapon = primaryWeapon;

                    equipedSlot = WeaponSlot.Primary;
                    break;

                case WeaponSlot.Secundary:
                    Debug.Log("Trocando para arma secundária");

                    withdrawnedWeapon?.Sheath();
                    withdrawnedWeapon = secundaryWeapon;

                    equipedSlot = WeaponSlot.Secundary;
                    break;

                case WeaponSlot.Special:
                    Debug.Log("Trocando para arma especial");

                    withdrawnedWeapon?.Sheath();
                    withdrawnedWeapon = specialWeapon;

                    equipedSlot = WeaponSlot.Special;
                    break;
            }

            if(withdrawnedWeapon != null) StartCoroutine(DrawAfterSheath());
        }
    }

    IEnumerator DrawAfterSheath()
    {
        canChangeWeapon = false;
        yield return new WaitForSeconds(.2f);
        withdrawnedWeapon.Draw();
        canChangeWeapon = true;
    }

}