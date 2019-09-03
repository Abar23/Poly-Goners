using CSE5912;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeSystem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameConstants.Instance.timingClass = this;
    }
}
