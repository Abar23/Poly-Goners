using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            Destroy(gameObject);
        }
    }

}
