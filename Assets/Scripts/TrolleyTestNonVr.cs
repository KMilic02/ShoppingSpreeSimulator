using UnityEngine;
using UnityEngine.InputSystem;

public class TrolleyTestNonVr : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    Rigidbody rb;

    const float forceMultiplier = 1000.0f;
    // Update is called once per frame
    void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard.wKey.isPressed)
        {
            rb.AddForceAtPosition(forceMultiplier * Time.deltaTime * transform.forward, transform.position - transform.forward * 0.5f - transform.right * 2.5f, ForceMode.Acceleration);       
        }   
        
        if (keyboard.uKey.isPressed)
        {
            rb.AddForceAtPosition(forceMultiplier * Time.deltaTime * transform.forward, transform.position - transform.forward * 0.5f + transform.right * 2.5f, ForceMode.Acceleration);       
        }  
    }
}
