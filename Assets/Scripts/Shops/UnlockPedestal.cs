using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockPedestal : MonoBehaviour
{
    private bool isActive;
    private GameObject unlockType;
    private GameObject displayObject;
    
    void Start() {
        isActive = false;
        unlockType = transform.GetChild(0).gameObject;
        displayObject = transform.GetChild(1).gameObject;
    }

    void Update() {
        if (!isActive) {
            displayObject.SetActive(false);
            isActive = ItemUnlockManager.instance.CheckIfItemIsActive(unlockType.name);
        }

        if (isActive) {
            displayObject.SetActive(true);
        }
    }
}
