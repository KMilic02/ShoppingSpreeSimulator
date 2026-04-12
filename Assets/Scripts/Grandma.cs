using System;
using UnityEditor.Animations;
using UnityEngine;

public class Grandma : MonoBehaviour
{
    public Rigidbody[] ragdollRbs;

    public Transform checkpointEnd;

    Quaternion rotation;
    Vector3 startPos;
    Vector3 endPos;
    Vector3 targetPos;

    bool hit;

    public Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (var rb in ragdollRbs)
        {
            var grandmaCollision = rb.gameObject.AddComponent<GrandmaCollision>();
            grandmaCollision.grandmaRef = this;
        }
        rotation = transform.rotation;
        startPos = transform.position;
        endPos = checkpointEnd.position;
        targetPos = endPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (hit)
            return;
        
        transform.rotation = rotation;

        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
        {
            targetPos = targetPos == startPos ? endPos : startPos;
            rotation = Quaternion.LookRotation(targetPos - transform.position);
        }
    }

    public void onGrandmaHit(Vector3 impulse)
    {
        if (hit)
            return;
        
        hit = true;
        animator.enabled = false;
        foreach (var rb in ragdollRbs)
        {
            rb.isKinematic = false;
        }
        
        ragdollRbs[1].AddForce(-impulse * 0.25f, ForceMode.Impulse);
    }
}
