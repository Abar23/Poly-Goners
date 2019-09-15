using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour, IWeapon
{
    private bool beingHeld = false;
    private bool swingingWeapon = false;
    private bool ableToHitEnemy = false;
    private bool pastWindUp = false;
    public GameObject playerHand;
    private Collider collider;
    public GameObject player;
    private float swingTime;
    private float elapsedTime;
    private float windUpTime;

    private Vector3 pickupPosition = new Vector3(0.058f, 0.037f, 0.173f);
    private Vector3 pickupRotation = new Vector3(-71.232f, -119.827f, 89.33301f);

    void Start() {
        collider = GetComponent<Collider>();
    }

    void Update() {
        if (swingingWeapon) {
            elapsedTime += Time.deltaTime;

            if (!pastWindUp && elapsedTime > windUpTime) {
                ableToHitEnemy = true;
                collider.enabled = true;
                pastWindUp = true;
            }

            if (elapsedTime > swingTime) {
                elapsedTime = 0;
                ableToHitEnemy = true;
                swingingWeapon = false;
                ableToHitEnemy = false;
                collider.enabled = false;
                Debug.Log("Attack animation finished.");
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

    public void SwingWeapon(float animationTime) {
        if (!swingingWeapon) {
            swingingWeapon = true;
            windUpTime = .3f * animationTime; // probably need a better solution to this
            swingTime = animationTime;
            pastWindUp = false;
        }
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
