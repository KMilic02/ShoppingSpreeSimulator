using System;
using UnityEngine;

public class GrandmaCollision : MonoBehaviour
{
    public Grandma grandmaRef;

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag != "Baba")
        {
            grandmaRef.onGrandmaHit(other.impulse);
        }
    }
}
