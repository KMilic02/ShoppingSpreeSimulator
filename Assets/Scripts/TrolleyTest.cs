using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class TrolleyTest : MonoBehaviour
{
    Rigidbody rb;
    const float forceMultiplier = 400.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
		rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        var leftHandedControllers = new List<InputDevice>();
        var desiredCharacteristics = InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(desiredCharacteristics, leftHandedControllers);
        
        var rightHandedControllers = new List<InputDevice>();
        desiredCharacteristics = InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(desiredCharacteristics, rightHandedControllers);

        Vector2 val = Vector2.zero;
        if (leftHandedControllers.Exists(x => x.TryGetFeatureValue(CommonUsages.primary2DAxis, out val)))
        {
            if (val != Vector2.zero)
            {
                var forwardVec = val.y * transform.forward;
                var sideVec = val.x * transform.right;
                var finalVec = forwardVec + sideVec;
                finalVec.Normalize();
                rb.AddForceAtPosition(forceMultiplier * Time.deltaTime * finalVec, transform.position - transform.forward * 0.5f - transform.right * 2.5f, ForceMode.Acceleration);       
            }
        }
        
        if (rightHandedControllers.Exists(x => x.TryGetFeatureValue(CommonUsages.primary2DAxis, out val)))
        {
            if (val != Vector2.zero)
            {
                var forwardVec = val.y * transform.forward;
                var sideVec = val.x * transform.right;
                var finalVec = forwardVec + sideVec;
                finalVec.Normalize();
                rb.AddForceAtPosition(forceMultiplier * Time.deltaTime * finalVec, transform.position - transform.forward * 0.5f + transform.right * 2.5f, ForceMode.Acceleration);       
            }
        }
    }
}
