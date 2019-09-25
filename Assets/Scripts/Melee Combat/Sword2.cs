﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword2 : MonoBehaviour, IWeapon
{
    private Vector3 startPosition = new Vector3(0.058f, 0.037f, 0.173f);
    private Vector3 startRotation = new Vector3(-71.232f, -119.827f, 89.33301f);

    private bool swingingWeapon = false;
    private float swingTime;
    private float elapsedTime;
    private GameObject player;

    void Start()
    {
        this.transform.localPosition = startPosition;
        this.transform.localEulerAngles = startRotation;
        player = GameObject.Find("Player 1");
        player.GetComponent<Player>().ChangeCurrentWeapon(this);
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
        swingTime = animationTime;
    }

    public bool CheckIfAttacking()
    {
        return swingingWeapon;
    }
}
