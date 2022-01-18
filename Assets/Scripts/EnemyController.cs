using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] Animator anim;

    CombatManager
    combat;
    public CombatManager Combat { get => combat; }

    int a = 0;

    // Start is called before the first frame update
    void Awake()
    {
        combat = new CombatManager(999, 0);
    }

}
