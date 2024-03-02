using UnityEngine;

public class Combat : MonoBehaviour
{
    [SerializeField] int life = 100;
    Animator anim;

    protected virtual void Attack()
    {
        //anim.SetTrigger("Attack");
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
