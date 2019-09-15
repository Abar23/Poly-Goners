using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour, IWeapon
{
    bool beingHeld = false;
    bool swingingWeapon = false;
    private bool ableToHitEnemy = false;
    public GameObject playerHand;
    private Collider collider;
    public GameObject player;
    private float swingTime;
    private float elapsedTime;

    private Vector3 pickupPosition = new Vector3(0.058f, 0.037f, 0.173f);
    private Vector3 pickupRotation = new Vector3(-71.232f, -119.827f, 89.33301f);

    void Start() {
        collider = GetComponent<Collider>();
    }

    void Update() {
        if (swingingWeapon) {
            elapsedTime += Time.deltaTime;

            if (elapsedTime > swingTime) {
                elapsedTime = 0;
                ableToHitEnemy = true;
                swingingWeapon = false;
                ableToHitEnemy = false;
                collider.enabled = false;
            }
        }
    }

    void PickUpByPlayer() {
        this.transform.parent = playerHand.transform;
        this.transform.localPosition = pickupPosition;
        this.transform.localEulerAngles = pickupRotation;
        player.GetComponent<Player>().ChangeCurrentWeapon(this);
        collider.enabled = false; // turn hitbox off until weapon is swung
    }

    void ToggleHitbox() {
        collider.enabled = !collider.enabled;
    }

    public void SwingWeapon(float animationTime) {
        swingingWeapon = true;
        swingTime = animationTime;
        ableToHitEnemy = true;
        collider.enabled = true;
    }

    void OnTriggerEnter() {
        if (!beingHeld) {
            beingHeld = true;
            PickUpByPlayer();
        } else if (ableToHitEnemy) {
            ableToHitEnemy = false;
            collider.enabled = false;
        }
    }
}
