using System.Collections.Generic;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum Category
    {
        Coffee,
        Soda,
        Pumpkin,
        Plate,
        Sardines,
        PeanutButter,
        Banana,
        Apple,
        Orange,
        Pineapple,
        Watermelon,
        CandyBar,
        Pot,
        Chips,
        Ketchup,
        Juice,
        GymBottle,
        WaterBottle,
        Wine,
        PinkJuice,
        RedJuice,
        CanOfBeans,
        TunaCan,
        BlackJam,
        PurpleJam,
        Steak,
        Milk,
        ToiletPaper,
        DogFood,
        Cupcake,
        Mayo
    }
    
    public static Dictionary<Category, string> name = new Dictionary<Category, string>()
    {
        { Category.Coffee,  "Coffee" },
        { Category.Soda, "Soda" },
        { Category.Pumpkin, "Pumpkin" },
        { Category.Plate, "Plate" },
        { Category.Sardines, "Sardines" },
        { Category.PeanutButter, "Peanut Butter" },
        { Category.Banana, "Banana" },
        { Category.Apple, "Apple" },
        { Category.Orange, "Orange" },
        { Category.Pineapple, "Pineapple" },
        { Category.Watermelon, "Watermelon" },
        { Category.CandyBar, "Candy Bar" },
        { Category.Pot, "Pot" },
        { Category.Chips, "Chips" },
        { Category.Ketchup, "Ketchup" },
        { Category.Juice, "Juice" },
        { Category.GymBottle, "Gym Bottle" },
        { Category.WaterBottle, "Water Bottle" },
        { Category.Wine, "Wine" },
        { Category.PinkJuice, "Pink Juice" },
        { Category.RedJuice, "Red Juice" },
        { Category.CanOfBeans, "Can of Beans" },
        { Category.TunaCan, "Tuna Can" },
        { Category.BlackJam, "Black Jam" },
        { Category.PurpleJam, "Purple Jam" },
        { Category.Steak, "Steak" },
        { Category.Milk, "Milk" },
        { Category.ToiletPaper, "Toilet Paper" },
        { Category.DogFood, "Dog Food" },
        { Category.Cupcake, "Cupcake" },
        { Category.Mayo, "Mayo" },
    };
    
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
