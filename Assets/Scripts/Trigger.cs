using System;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public Action<Collider> onTriggerEnterAction;
    public Action<Collider> onTriggerExitAction;
    
    void OnTriggerEnter(Collider other)
    {
        onTriggerEnterAction?.Invoke(other);
    }

    void OnTriggerExit(Collider other)
    {
        onTriggerExitAction?.Invoke(other);
    }
}
