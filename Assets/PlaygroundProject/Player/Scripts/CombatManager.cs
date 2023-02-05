using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    int life;
    int damage;
    bool blocking;
    Animator anim;

    Collider lHand;
    Collider rHand;

    private void Update()
    {
        InputHolder();
    }

    void InputHolder()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) Attack();
    }

    public void Attack()
    {
        //anim.SetTrigger("Attack");
        Debug.Log("<color=green>chegou</color>");
        FindObjectOfType<AudioManager>().PlayOneShot("bang");
    }

    public void IsAttacked(int dmg)
    {
        if (blocking) Debug.Log("<color=green>avoid damage</color>");
        else TakeDamage(dmg);
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
