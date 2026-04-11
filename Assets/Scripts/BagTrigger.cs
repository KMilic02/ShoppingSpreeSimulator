using System;
using UnityEngine;

public class BagTrigger : MonoBehaviour
{
    public Action<Item> onTriggerEnterAction;
    public Action<Item> onTriggerExitAction;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Item>(out var item))
        {
            onTriggerEnterAction?.Invoke(item);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Item>(out var item))
        {
            onTriggerExitAction?.Invoke(item);
        }
    }
}
