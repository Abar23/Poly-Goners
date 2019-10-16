using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    private List<GameObject> shopInventory;
    private List<GameObject> activeShopItems;
    private List<GameObject> activeItemSlots = new List<GameObject>();
    const int maxNumberActiveItems = 4;
    int numberActiveItems;
    public GameObject player1;
    public GameObject player2;
    
    void Start()
    {   
        GameObject itemSlots = this.transform.Find("Active Item Slots").gameObject;
        foreach (Transform child in itemSlots.transform)
        {
            activeItemSlots.Add(child.gameObject);
        }

        ResetActiveInventory();
        RandomizeActiveItems();
    }

    private void ResetActiveInventory()
    {
        shopInventory = new List<GameObject>();
        activeShopItems = new List<GameObject>();

        GameObject inventory = this.transform.Find("Inventory").gameObject;

        foreach (Transform child in inventory.transform)
        {
            shopInventory.Add(child.gameObject);
        }

        numberActiveItems = 0;
    }

    private void RandomizeActiveItems()
    {
        for (int i = 0; i < maxNumberActiveItems; i++)
        {
            if (shopInventory.Count > 0)
            {
                int itemIndex = Random.Range(0, shopInventory.Count);
                activeShopItems.Add(shopInventory[itemIndex]);
                shopInventory.RemoveAt(itemIndex);
                numberActiveItems++;
            }
        }

        DisplayActiveItems();
    }

    private void DisplayActiveItems()
    {
        for (int i = 0; i < numberActiveItems; i++)
        {
            activeShopItems[i].transform.position = activeItemSlots[i].transform.position;
            activeShopItems[i].SetActive(true);
        }
    }
}
