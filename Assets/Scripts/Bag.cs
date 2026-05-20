using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bag : MonoBehaviour
{
    public BagTrigger bagTrigger;
    public BagUI bagUI;
    
    HashSet<Item> itemsInBag = new HashSet<Item>();
    List<(Item.Category, int)> itemsRequired = new List<(Item.Category, int)>();
    
    void Start()
    {
        GameManager.shoppingFinished = false;
        bagTrigger.onTriggerEnterAction = addItemToBag;
        bagTrigger.onTriggerExitAction = removeItemFromBag;

        randomizeRequiredItems();
        bagUI.initUI(itemsRequired);
    }

    void randomizeRequiredItems()
    {
        // Item.Category testCategory = Item.Category.Plate; 
        //
        // int quantityRequired = 1; // Force it to require exactly 1 item!
        //
        // itemsRequired.Add((testCategory, quantityRequired));
        //
        // Debug.Log($"[BAG TEST MODE] Required items overridden! You only need to collect {quantityRequired} x {testCategory} to finish.");
        
        Array values = Enum.GetValues(typeof(Item.Category));
        var randomCount = Random.Range(3, 6);
        
        int[] enumValues = new int[values.Length];
        for (int i = 0; i < values.Length; i++)
        {
            enumValues[i] = (int)values.GetValue(i);
        }
        
        for (int i = 0; i < enumValues.Length; i++ )
        {
            var temp = enumValues[i];
            var random = Random.Range(i, enumValues.Length);
            enumValues[i] = enumValues[random];
            enumValues[random] = temp;
        }
        
        for (int i = 0; i < randomCount; i++)
        {
            itemsRequired.Add(((Item.Category) enumValues[i], Random.Range(2, 5)));
        }
    }

    void addItemToBag(Item item)
    {
        itemsInBag.Add(item);
        bagUI.updateUI(itemsInBag, itemsRequired);
    }

    void removeItemFromBag(Item item)
    {
        itemsInBag.Remove(item);
        bagUI.updateUI(itemsInBag, itemsRequired);
    }
}
