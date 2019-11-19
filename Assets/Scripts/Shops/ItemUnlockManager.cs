using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUnlockManager : MonoBehaviour
{
    public static ItemUnlockManager instance = null;
    public Dictionary<string, bool> unlockedItems;

    void Awake() {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
    }

    void Start() {
        unlockedItems = new Dictionary<string, bool>();
    }

    public void AddPickupToUnlocks(string pickupName) {
        if (!unlockedItems.ContainsKey(pickupName))
            unlockedItems.Add(pickupName, true);
    }

    public bool CheckIfItemIsActive(string itemName) {
        bool isActive = false;
        
        if (unlockedItems.ContainsKey(itemName)) {
            if (unlockedItems[itemName] == true)
                isActive = true;
        }

        return isActive;
    }
}
