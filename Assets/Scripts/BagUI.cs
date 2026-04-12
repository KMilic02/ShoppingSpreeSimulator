using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BagUI : MonoBehaviour
{
    public GameObject shoppingItemTextBase;
    
    public Dictionary<Item.Category, TMP_Text> shoppingList = new Dictionary<Item.Category, TMP_Text>();
    public GameObject extrasInCart;

    public void initUI(List<(Item.Category, int)> itemsRequired)
    {
        foreach (var (category, count) in itemsRequired)
        {
            var listString = $"{count}x {Item.name[category]}";
            var shoppingItemTextObject = Instantiate(shoppingItemTextBase, shoppingItemTextBase.transform.parent);
            shoppingItemTextObject.SetActive(true);
            var shoppingItemText = shoppingItemTextObject.GetComponent<TMP_Text>();
            shoppingItemText.text = listString;
            shoppingList.Add(category, shoppingItemText);
        }
    }
    
    public void updateUI(HashSet<Item> itemsInBag, List<(Item.Category, int)> itemsRequired)
    {
        GameManager.shoppingFinished = false;
        
        var checkmarks = 0;
        List<(Item.Category, int)> categorizedItemsInBag = new List<(Item.Category, int)>();
        foreach (var item in itemsInBag)
        {
            if (shoppingList.ContainsKey(item.category))
            {
                if (!categorizedItemsInBag.Exists(x => x.Item1 == item.category))
                {
                    categorizedItemsInBag.Add((item.category, 1));
                }
                else
                {
                    var categorizedItem = categorizedItemsInBag.FindIndex(x => x.Item1 == item.category);
                    categorizedItemsInBag[categorizedItem] = (item.category, categorizedItemsInBag[categorizedItem].Item2 + 1);
                }
            }
        }

        foreach (var val in shoppingList.Values)
        {
            val.fontStyle = FontStyles.Normal;
            val.color = Color.black;
        }
        
        for (int i = 0; i < categorizedItemsInBag.Count; i++)
        {
            var itemRequired = itemsRequired.Find(x => x.Item1 == categorizedItemsInBag[i].Item1);
            if (itemRequired.Item2 == categorizedItemsInBag[i].Item2)
            {
                shoppingList[categorizedItemsInBag[i].Item1].fontStyle = FontStyles.Strikethrough;
                shoppingList[categorizedItemsInBag[i].Item1].color = Color.green;
                checkmarks++;
            }
        }
        
        if (checkmarks == itemsRequired.Count)
            GameManager.shoppingFinished = true;
    }
}
