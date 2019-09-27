using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour
{

    [SerializeField] UnityEvent onTriggerInvoke;

    [SerializeField] UnityEvent onTriggerDevoke;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            TriggerEvent(onTriggerInvoke);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            TriggerEvent(onTriggerDevoke);
        }
    }

    void TriggerEvent(UnityEvent uEvent)
    {
        if (uEvent != null)
        {
            uEvent.Invoke();
        }
    }
}
