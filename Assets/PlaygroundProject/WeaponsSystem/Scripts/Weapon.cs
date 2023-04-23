using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject visuals;
    public float damage;
    public float rateOfFire;
    public float range;
    public float ammoCapacity;
    public float magazineSize;

    private bool onSheath = true;

    public void Shoot()
    {
        anim.SetTrigger("Shoot");
    }

    public void Sheath()
    {
        if (onSheath) return;

        anim.SetTrigger("Sheath");
        onSheath = true;
    }

    public void Draw()
    {
        if (!onSheath) return;

        anim.SetTrigger("Draw");
        onSheath = false;
    }

    public void Hide() { visuals.SetActive(false); }

    public void Show() { visuals.SetActive(true); }
}
