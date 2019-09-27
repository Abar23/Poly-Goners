﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour, IWeapon
{
    private bool swingingWeapon = false;
    private bool ableToHitEnemy = false;
    private float swingTime;
    private float elapsedTime;
    private Collider collider;
    public WeaponAnimationConfig weaponAnimationConfig;

    public void Start()
    {
        GetComponentInParent<Player>().ChangeCurrentWeapon(this);
        collider = GetComponent<Collider>();
    }

    public void Update()
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

    public void OnTriggerEnter(Collider other)
    {
        if (ableToHitEnemy)
        {
            ableToHitEnemy = false;
            collider.enabled = false;
        }
    }

    public WeaponAnimationConfig GetAnimationConfig()
    {
        return weaponAnimationConfig;
    }
}
