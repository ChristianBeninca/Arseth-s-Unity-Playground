using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    int 
        life,
        damage;
    bool
        blocking;
    Animator 
        anim;

    public Collider
        lHand,
        rHand;

    public CombatManager(int lf, int dmg, Animator a = null)
    {
        life = lf;
        damage = dmg;
        anim = a;
    }

    public void Attack()
    {
        anim.SetTrigger("Attack");
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
