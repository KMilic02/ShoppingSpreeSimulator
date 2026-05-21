using System.Collections.Generic;
using Oculus.Interaction;
using UnityEngine;
using UnityEngine.XR;

public class ModeSelection : MonoBehaviour
{
    enum Selected
    {
        None,
        Trolley,
        Basket
    }

    public GameObject player;
    public GameObject shoppingList;
    public GameObject shoppingListPosBasketL;
    public GameObject shoppingListPosBasketR;
    public GameObject shoppingListPosTrolley;
    public GameObject instructionsCanvas;
    
    public List<GameObject> basketComponents;
    public List<GameObject> trolleyComponents;

    public List<GameObject> basketLHandComponents;
    public List<GameObject> basketRHandComponents;


    public HandVisual lHandVisual;
    public HandVisual rHandVisual;
    
    GameObject lHand;
    GameObject rHand;

    public Transform basket;
    public Transform trolley;
    
    List<InputDevice> leftHandedControllers = new List<InputDevice>();
    List<InputDevice> rightHandedControllers = new List<InputDevice>();

    bool lastFrameGripping;

    Selected selected = Selected.None;
    float timer = 0.0f;
    bool transition;
    bool fadeIn;
    
    public static bool basketRightHanded;

    public Bag bag;
    public BagTrigger basketTrigger;
    public BagTrigger trolleyTrigger;
    
    void Update()
    {
        if (transition)
        {
            doTransition();
            return;
            
        }
        leftHandedControllers = new List<InputDevice>();
        var desiredCharacteristics = InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(desiredCharacteristics, leftHandedControllers);
        
        rightHandedControllers = new List<InputDevice>();
        desiredCharacteristics = InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(desiredCharacteristics, rightHandedControllers);
        
        bool gripping = false;
        if (leftHandedControllers.Exists(x => x.TryGetFeatureValue(CommonUsages.gripButton, out gripping)))
        {
            lHand = lHandVisual.Root.gameObject;
            if (!lastFrameGripping && gripping)
            {
                basketRightHanded = false;
                select(lHand);
                return;
            }
        }

        if (rightHandedControllers.Exists(x => x.TryGetFeatureValue(CommonUsages.gripButton, out gripping)))
        {
            rHand = rHandVisual.Root.gameObject;
            if (!lastFrameGripping && gripping)
            {
                basketRightHanded = true;
                select(rHand);
                return;

            }
        }

        lastFrameGripping = gripping;
    }

    void select(GameObject hand)
    {
        var distFromBasket = Vector3.Distance(basket.position, hand.transform.position);
        var distFromTrolley = Vector3.Distance(trolley.position, hand.transform.position);

        if (distFromBasket < 0.4f)
        {
            select(Selected.Basket);
        }
        else if (distFromTrolley < 0.4f)
        {
            select(Selected.Trolley);
        }
    }

    void select(Selected selection)
    {
        selected = selection;
        transition = true;
        
        if (instructionsCanvas != null) 
        {
            instructionsCanvas.SetActive(false);
        }
        
        OVRScreenFade.instance.FadeOut();
    }

    void doTransition()
    {
        timer += Time.deltaTime;

        if (timer >= 1.5f && !fadeIn)
        {
            OVRScreenFade.instance.FadeIn();
            
            var components = selected is Selected.Basket ? basketComponents : trolleyComponents;
            components.ForEach(x => x.gameObject.SetActive(true));
            
            if (selected == Selected.Trolley)
            {
                player.GetComponent<Rigidbody>().isKinematic = false;
                player.GetComponent<Trolley>().enabled = true;
                bag.bagTrigger = trolleyTrigger;
                bag.bagTrigger.onTriggerEnterAction = bag.addItemToBag;
                bag.bagTrigger.onTriggerExitAction = bag.removeItemFromBag;
                shoppingList.transform.position = shoppingListPosTrolley.transform.position;
                shoppingList.transform.parent = shoppingListPosTrolley.transform;
                shoppingList.transform.localRotation = Quaternion.identity;
            }

            if (selected == Selected.Basket)
            {
                bag.bagTrigger = basketTrigger;
                bag.bagTrigger.onTriggerEnterAction = bag.addItemToBag;
                bag.bagTrigger.onTriggerExitAction = bag.removeItemFromBag;
                if (basketRightHanded)
                {
                    basketRHandComponents.ForEach(x => x.SetActive(true));
                    shoppingList.transform.position = shoppingListPosBasketR.transform.position;
                    shoppingList.transform.parent = shoppingListPosBasketR.transform;
                }
                else
                {
                    basketLHandComponents.ForEach(x => x.SetActive(true));
                    shoppingList.transform.position = shoppingListPosBasketL.transform.position;
                    shoppingList.transform.parent = shoppingListPosBasketL.transform;
                }
                shoppingList.transform.localRotation = Quaternion.identity;
            }
        }

        if (timer >= 2.5f)
        {
            gameObject.SetActive(false);
        }
    }
}
