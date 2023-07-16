using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    [SerializeField] int life = 100;
    Animator anim;

    public void Attack()
    {
        //anim.SetTrigger("Attack");
        Debug.Log("<color=green>chegou</color>");
        FindObjectOfType<AudioManager>().PlayOneShot("bang");
    }

    void TakeDamage(int dmg)
    {
        life -= dmg;
        CheckLife();
    }

    void CheckLife()
    {
        if (life <= 0) Debug.Log("<color=red>morreu</color>");
    }
}
