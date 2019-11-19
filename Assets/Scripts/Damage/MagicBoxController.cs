using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBoxController : MonoBehaviour
{

    public MagicBox Box;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Box.FireMagic(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Box.FireMagic(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Box.FireMagic(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Box.FireMagic(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Box.FireMagic(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            Box.FireMagic(5);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            Box.FireMagic(6);
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            Box.FireMagic(7);
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            Box.FireMagic(8);
        }
        if (Input.GetKeyUp(KeyCode.Alpha6))
        {
            Box.StopMagic(5);
        }
        if (Input.GetKeyUp(KeyCode.Alpha7))
        {
            Box.StopMagic(6);
        }
        if (Input.GetKeyUp(KeyCode.Alpha8))
        {
            Box.StopMagic(7);
        }
        if (Input.GetKeyUp(KeyCode.Alpha9))
        {
            Box.StopMagic(8);
        }
    }
}
