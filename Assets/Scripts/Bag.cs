using System;
using System.Collections.Generic;
using Oculus.Interaction.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bag : MonoBehaviour
{
    public BagTrigger bagTrigger;
    public BagUI bagUI;
    
    public AudioSource audioSource;
    public AudioClip pickupClip;
    public AudioClip completeClip;

    HashSet<Item> itemsInBag = new HashSet<Item>();
    public HashSet<Item> itemsInBagPersistent = new HashSet<Item>();
    List<(Item.Category, int)> itemsRequired = new List<(Item.Category, int)>();
    
    bool completionSoundPlayed;

    void Start()
    {
        GameManager.shoppingFinished = false;
        bagTrigger.onTriggerEnterAction = addItemToBag;
        bagTrigger.onTriggerExitAction = removeItemFromBag;

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        randomizeRequiredItems();
        bagUI.initUI(itemsRequired);
    }

    void LateUpdate()
    {
        itemsInBagPersistent = new HashSet<Item>(itemsInBag);
    }

    void randomizeRequiredItems()
    {
        // Item.Category testCategory = Item.Category.Plate; 
        //
        // int quantityRequired = 1;
        //
        // itemsRequired.Add((testCategory, quantityRequired));
        //
        // Debug.Log($"[BAG TEST MODE] Required items overridden! You only need to collect {quantityRequired} x {testCategory} to finish.");
        
        Array values = Enum.GetValues(typeof(Item.Category));
        var randomCount = Random.Range(2, 4);
        
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
            itemsRequired.Add(((Item.Category) enumValues[i], Random.Range(1, 4)));
        }
    }

    void PlaySound(AudioClip clip)
    {
        if (clip == null || audioSource == null) return;
        audioSource.PlayOneShot(clip);
    }

    public void addItemToBag(Item item)
    {
        itemsInBag.Add(item);

        if (itemsRequired.Exists(x => x.Item1 == item.category))
        {
            PlaySound(pickupClip);
        }
        
        bagUI.updateUI(itemsInBag, itemsRequired);
        
        if (GameManager.shoppingFinished && !completionSoundPlayed)
        {
            completionSoundPlayed = true;
            PlaySound(completeClip);
        }
    }

    public void removeItemFromBag(Item item)
    {
        itemsInBag.Remove(item);
        bagUI.updateUI(itemsInBag, itemsRequired);

        if (!GameManager.shoppingFinished)
        {
            completionSoundPlayed = false;
        }
    }
}
