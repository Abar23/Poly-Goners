using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockPedestal : MonoBehaviour
{
    private bool isActive;
    private GameObject unlockType;
    
    void Start() {
        isActive = false;
        unlockType = transform.GetChild(0).gameObject;
    }

    void Update() {
        if (!isActive) {
            isActive = ItemUnlockManager.instance.CheckIfItemIsActive(unlockType.name);

            if (isActive) {
                Instantiate(unlockType, new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), Quaternion.identity).SetActive(true);
            }
        }
    }
}
