using System.Collections;
using UnityEngine;


public class Target : MonoBehaviour
{

    private int health_ = 150;
    [SerializeField] private GameObject dead_body_;

    public void TakeDamage(int dmg)
    {
        health_ -= dmg;
        CheckLife();
    }

    void CheckLife()
    {
        if (health_ <= 0)
        {
            health_ = 0;
            Die();
        }
    }

    void Die()
    {
        Debug.Log("<color=red>I'm dead. Ass: " + this.name + "</color>");
        Instantiate(dead_body_, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }
}
