using Oculus.Interaction;
using Oculus.Interaction.Input;
using UnityEngine;

public class Basket : MonoBehaviour
{
    public HandVisual handVisualL;
    public HandVisual handVisualR;
    public Rigidbody anchoredBody;
    
    Vector3 previousPosition = Vector3.zero;
    
    const float swingForce = 150.0f;

    public Bag bag;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var handVisual = ModeSelection.basketRightHanded ? handVisualR : handVisualL;
        var target = handVisual.GetTransformByHandJointId(HandJointId.HandMiddle2);
        transform.position = target.position + transform.forward * 0.2f;

        var jointRotation = target.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0.0f, jointRotation.y + 90.0f, 0.0f);

        foreach (var item in bag.itemsInBagPersistent)
        {
            Debug.Log("here");
            item.transform.position += transform.position - previousPosition;
        }
        
        if (Vector3.Distance(transform.position, previousPosition) < 0.2f)
        {
            anchoredBody.AddForce((transform.position - previousPosition) * swingForce);
        }
        
        previousPosition = transform.position;
    }
}
