using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class TrolleyTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var leftHandedControllers = new List<InputDevice>();
        var desiredCharacteristics = InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(desiredCharacteristics, leftHandedControllers);

        Vector2 val = Vector2.zero;
        if (leftHandedControllers.Exists(x => x.TryGetFeatureValue(CommonUsages.primary2DAxis, out val)))
        {
            if (val != Vector2.zero)
            {
                Debug.Log("I am here");
            }
        }
    }
}
