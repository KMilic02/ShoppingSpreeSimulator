using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HandGrabInteractable))]
public class GrabHighlight : MonoBehaviour
{
    public Color hoverColor = Color.green;
    public Color grabColor = Color.white;
    public float outlineWidth = 5f;

    private Outline outline;
    private Grabbable interactable;

    bool initialized;

    private void Awake()
    {
    }

    void Update()
    {
        if (initialized)
            return;
        
        if (TryGetComponent<Grabbable>(out interactable))
        {
            outline = gameObject.AddComponent<Outline>();
            outline.OutlineMode = Outline.Mode.OutlineAll;
            outline.OutlineWidth = outlineWidth;
            outline.enabled = false;
            interactable.WhenPointerEventRaised += HandlePointerEvent;
            initialized = true;
        }
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
        //interactable.WhenPointerEventRaised -= HandlePointerEvent;
    }

    private void HandlePointerEvent(PointerEvent evt)
    {
        switch (evt.Type) 
        {
            case PointerEventType.Hover:
                outline.OutlineColor = hoverColor;
                outline.enabled = true;
                break;

            case PointerEventType.Unhover:
                outline.enabled = false;
                break;
            case PointerEventType.Select:
                outline.OutlineColor = grabColor;
                break;

            case PointerEventType.Unselect:
                outline.OutlineColor = hoverColor;
                //outline.enabled = false;
                break;

            case PointerEventType.Cancel:
                outline.enabled = false;
                break;
        }
    }
}