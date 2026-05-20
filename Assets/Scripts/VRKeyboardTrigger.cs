using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class VRKeyboardTrigger : MonoBehaviour, IPointerClickHandler
{
    private TMP_InputField inputField;
    private OVRVirtualKeyboard metaKeyboard;

    void Start()
    {
        Debug.Log("[KEYBOARD LOG] Start() initialized.");

        inputField = GetComponent<TMP_InputField>();
        if (inputField == null)
        {
            Debug.LogError("[KEYBOARD CRITICAL ERROR] TMP_InputField missing on this GameObject!");
            return;
        }

        metaKeyboard = FindObjectOfType<OVRVirtualKeyboard>();
        if (metaKeyboard != null)
        {
            Debug.Log($"[KEYBOARD LOG] Found OVRVirtualKeyboard on '{metaKeyboard.gameObject.name}'. Hooking up text streams...");
            
            // Modern Meta SDK Event Listeners
            metaKeyboard.CommitTextEvent.AddListener(OnTextCommitted);
            metaKeyboard.EnterEvent.AddListener(OnKeyboardEnter);
            metaKeyboard.BackspaceEvent.AddListener(OnKeyboardBackspace);
            
            // Keep it hidden initially until the text box is poked
            metaKeyboard.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("[KEYBOARD ERROR] OVRVirtualKeyboard prefab missing from your scene hierarchy!");
        }

        // Backup Selection handler
        inputField.onSelect.AddListener(delegate { LaunchMetaKeyboard(); });
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("[KEYBOARD LOG] Input Field Raycast Click Detected!");
        LaunchMetaKeyboard();
    }

    public void LaunchMetaKeyboard()
    {
        if (metaKeyboard != null)
        {
            Debug.Log("[KEYBOARD SUCCESS] Activating Meta VR Keyboard layout.");
            metaKeyboard.gameObject.SetActive(true);
            
            // Sync your TextMeshPro field data down to Meta's prediction dictionary context
            metaKeyboard.ChangeTextContext(inputField.text);
        }
    }

    // FIX: Modern Meta SDK relies on this string event instead of checking .Text in Update()
    private void OnTextCommitted(string committedText)
    {
        Debug.Log($"[KEYBOARD STREAM] Text received from VR layout: '{committedText}'");
        
        // Append or replace based on your text context layout
        inputField.text = committedText;
    }

    // Handles backspaces cleanly
    private void OnKeyboardBackspace()
    {
        Debug.Log("[KEYBOARD STREAM] Backspace button clicked.");
        if (inputField.text.Length > 0)
        {
            inputField.text = inputField.text.Substring(0, inputField.text.Length - 1);
            
            // Sync the change back down to Meta's memory bank
            metaKeyboard.ChangeTextContext(inputField.text);
        }
    }

    // Closes layout when pressing enter checkmark
    private void OnKeyboardEnter()
    {
        Debug.Log($"[KEYBOARD CLOSING] Enter pressed. Saved result: '{inputField.text}'");
        metaKeyboard.gameObject.SetActive(false);
    }
}