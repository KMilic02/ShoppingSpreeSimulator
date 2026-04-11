using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum Category
    {
        ChocoSprinkleDonut,
        Coffee,
        Soda,
        Pumpkin
    }
    
    public string name;
    public Category category;
    public GameObject grabInteractable;
    
    void Start()
    {
        var rigidbody = GetComponent<Rigidbody>();
        var grabbable = gameObject.AddComponent<Grabbable>();
        grabbable.TransferOnSecondSelection = true;
        grabbable.InjectOptionalRigidbody(rigidbody);
        var grabInteractableObject = Instantiate(grabInteractable, transform, false);
        var handGrab = grabInteractableObject.GetComponent<HandGrabInteractable>();
        handGrab.InjectOptionalPointableElement(grabbable);
        handGrab.InjectRigidbody(rigidbody);
    }

}
