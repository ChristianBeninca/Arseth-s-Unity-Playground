using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItem : MonoBehaviour, IInteractable
{

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Interact ()
    {
        CollectItem();
    }

    public void CollectItem() { }
}
