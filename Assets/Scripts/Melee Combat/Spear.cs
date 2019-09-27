using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour, IWeapon
{
    private Vector3 startPosition = new Vector3(-0.474f, 0.033f, 0.964f);
    private Vector3 startRotation = new Vector3(-71.232f, -119.827f, 89.33301f);
    //private Vector3 startRotation = new Vector3(36.411f, -125.179f, -252.613f);

    private bool swingingWeapon = false;
    private bool ableToHitEnemy = false;
    private float swingTime;
    private float elapsedTime;
    private GameObject player;
    private Collider collider;

    void Start()
    {
        this.transform.localPosition = startPosition;
        this.transform.localEulerAngles = startRotation;
        player = GameObject.Find("Player 1");
        player.GetComponent<Player>().ChangeCurrentWeapon(this);
        collider = GetComponent<Collider>();
    }

    void Update()
    {
        if (swingingWeapon)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime > swingTime)
            {
                elapsedTime = 0;
                swingingWeapon = false;
            }
        }
    }

    public void SwingWeapon(float animationTime)
    {
        swingingWeapon = true;
        ableToHitEnemy = true;
        swingTime = animationTime;
    }

    public bool CheckIfAttacking()
    {
        return swingingWeapon;
    }

    void OnTriggerEnter(Collider other)
    {
        if (ableToHitEnemy)
        {
            ableToHitEnemy = false;
            collider.enabled = false;
        }
    }
}
