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
        
        Dictionary<Item.Category, int> currentCountsInBag = new Dictionary<Item.Category, int>();
        foreach (var item in itemsInBag)
        {
            if (shoppingList.ContainsKey(item.category))
            {
                if (!currentCountsInBag.ContainsKey(item.category))
                    currentCountsInBag[item.category] = 1;
                else
                    currentCountsInBag[item.category]++;
            }
        }

        var completedCategories = 0;

        foreach (var (category, requiredCount) in itemsRequired)
        {
            if (!shoppingList.ContainsKey(category)) continue;

            int collectedCount = currentCountsInBag.ContainsKey(category) ? currentCountsInBag[category] : 0;
            TMP_Text uiText = shoppingList[category];

            string tallyString = ConvertToTallies(collectedCount);
            if (collectedCount == 0) tallyString = "";

            uiText.text = $"{requiredCount}x {Item.name[category]} {tallyString}";

            if (collectedCount == requiredCount)
            {
                uiText.fontStyle = FontStyles.Normal; 
                uiText.color = Color.green;
                completedCategories++;
            }
            else if (collectedCount > requiredCount)
            {
                uiText.fontStyle = FontStyles.Bold;
                uiText.color = new Color(0.9f, 0.2f, 0.2f);
            }
            else
            {
                uiText.fontStyle = FontStyles.Normal;
                uiText.color = Color.black;
            }
        }
        
        if (completedCategories == itemsRequired.Count)
            GameManager.shoppingFinished = true;
    }
    
    private string ConvertToTallies(int number)
    {
        if (number <= 0) return "";

        string result = "";
        int fives = number / 5;
        int remainder = number % 5;

        string crossedFive = "I\u0336I\u0336I\u0336I\u0336"; 

        for (int i = 0; i < fives; i++)
        {
            result += crossedFive + "   ";
        }
        
        for (int i = 0; i < remainder; i++)
        {
            result += "I";
        }

        return result.Trim();
    }
}