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
    private HandGrabInteractable interactable;

    private void Awake()
    {
        interactable = GetComponent<HandGrabInteractable>();

        outline = gameObject.AddComponent<Outline>();
        outline.OutlineMode = Outline.Mode.OutlineAll;
        outline.OutlineWidth = outlineWidth;
        outline.enabled = false;
    }

    private void OnEnable()
    {
        interactable.WhenPointerEventRaised += HandlePointerEvent;
    }

    private void OnDisable()
    {
        interactable.WhenPointerEventRaised -= HandlePointerEvent;
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
                if(interactable.SelectingInteractors.Count == 0)
                    outline.enabled = false;
                break;

            case PointerEventType.Select:
                outline.OutlineColor = grabColor;
                break;

            case PointerEventType.Unselect:
                outline.enabled = false;
                break;

            case PointerEventType.Cancel:
                outline.enabled = false;
                break;
        }
    }
}