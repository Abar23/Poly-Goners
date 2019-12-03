using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    public Player player;
    public Inventory inventory;

    private Coroutine pulseCoroutine;

    public void Hit()
    {
        player.currentWeapon.GetComponent<Collider>().enabled = true;
    }

    public void Shoot()
    {
        pulseCoroutine = StartCoroutine(StartMagic());
    }
    public void StopShoot()
    {
        pulseCoroutine = StartCoroutine(StopMagic());
    }

    IEnumerator StartMagic()
    {
        yield return new WaitForSeconds(0.33f);
        inventory.UseMagic();
    }

    IEnumerator StopMagic()
    {
        yield return new WaitForSeconds(0.33f);
        inventory.StopMagic();
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
