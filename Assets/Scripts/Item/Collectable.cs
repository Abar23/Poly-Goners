using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{

    public enum Type
    {
        Coin = 0x0,
        Potion = 0x1,
    }

    public Type CollectableType;

    public PotionConfig Config;

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            Destroy(gameObject);
        }
    }

}
