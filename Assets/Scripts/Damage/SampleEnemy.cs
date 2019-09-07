using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SampleEnemy : MonoBehaviour
{

    public AlignmentConfig Alignment;
    public Slider HealthBar;
    
    private int health;

    // Start is called before the first frame update
    void Start()
    {
        health = Alignment.MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        HealthBar.value = health / (float)Alignment.MaxHealth;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Projectile")
        {
            
        }
    }
}
