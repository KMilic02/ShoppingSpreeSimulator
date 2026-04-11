using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Trolley : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float forceMultiplier;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
		rb = GetComponent<Rigidbody>();
    }

    List<InputDevice> leftHandedControllers = new List<InputDevice>();
    List<InputDevice> rightHandedControllers = new List<InputDevice>();
    
    // Update is called once per frame
    void Update()
    {
        leftHandedControllers = new List<InputDevice>();
        var desiredCharacteristics = InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(desiredCharacteristics, leftHandedControllers);
        
        rightHandedControllers = new List<InputDevice>();
        desiredCharacteristics = InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(desiredCharacteristics, rightHandedControllers);

        addForces();
    }

    void addForces()
    {
        Vector2 val = Vector2.zero;
        if (leftHandedControllers.Exists(x => x.TryGetFeatureValue(CommonUsages.primary2DAxis, out val)))
        {
            if (val != Vector2.zero)
            {
                var forwardVec = val.y * transform.forward;
                var sideVec = val.x * transform.right;
                var finalVec = forwardVec + sideVec;
                //finalVec.Normalize();
                rb.AddForceAtPosition(rb.mass * forceMultiplier * Time.deltaTime * finalVec, transform.position - transform.forward * 1.0f - transform.right * 0.5f, ForceMode.Force);       
            }
        }
        
        if (rightHandedControllers.Exists(x => x.TryGetFeatureValue(CommonUsages.primary2DAxis, out val)))
        {
            if (val != Vector2.zero)
            {
                var forwardVec = val.y * transform.forward;
                var sideVec = val.x * transform.right;
                var finalVec = forwardVec + sideVec;
                //finalVec.Normalize();
                rb.AddForceAtPosition(rb.mass * forceMultiplier * Time.deltaTime * finalVec, transform.position - transform.forward * 1.0f + transform.right * 0.5f, ForceMode.Force);       
            }
        }

        var rotationalForce = rb.angularVelocity;
        if (rotationalForce.magnitude > 1.6f)
        {
            rotationalForce = rotationalForce.normalized * 1.6f;
        }
        rb.angularVelocity = rotationalForce;
    }
}
