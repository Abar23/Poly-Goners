using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public ProjectileConfig Config;

    public GameObject AfterEffect;

    private Vector3 velocity;

    void Start()
    {
        velocity = Vector3.right * Config.Speed;
    }

    void FixedUpdate()
    {
        transform.Translate(velocity * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("entered");
        AfterEffect.SetActive(true);
    }

    void OnTriggerEnter(Collider other)
    {
        velocity = Vector3.zero;
        AfterEffect.SetActive(true);
    }
}
