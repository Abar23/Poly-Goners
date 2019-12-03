using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    public Player player;
    public Inventory inventory;

    public void Hit()
    {
        player.currentWeapon.GetComponent<Collider>().enabled = true;
    }

    public void Shoot()
    {
        inventory.UseMagic();
    }

    public void FootR()
    {
    }

    public void FootL()
    {
    }

    public void Land()
    {
    }

    // For the switch weapon animation event
    public void SwitchWeapon()
    {
        inventory.NextMeleeWeapon();
    }

    void OnAnimatorMove()
    {
        transform.parent.rotation = GetComponent<Animator>().rootRotation;
        transform.parent.position += GetComponent<Animator>().deltaPosition;
    }
}
