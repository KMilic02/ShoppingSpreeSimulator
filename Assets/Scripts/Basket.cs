using Oculus.Interaction;
using Oculus.Interaction.Input;
using UnityEngine;

public class Basket : MonoBehaviour
{
    public HandVisual handVisual; //treba napravit da bude ona ruka koja uzme kosaru
    public Rigidbody anchoredBody;
    
    Vector3 previousPosition = Vector3.zero;
    
    const float swingForce = 120.0f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var target = handVisual.GetTransformByHandJointId(HandJointId.HandMiddle2);
        transform.position = target.position + transform.forward * 0.2f;

        if (Vector3.Distance(transform.position, previousPosition) < 0.2f)
        {
            anchoredBody.AddForce((transform.position - previousPosition) * swingForce);
        }
        
        previousPosition = transform.position;
    }
}
