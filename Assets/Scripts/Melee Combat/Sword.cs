using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour, IWeapon
{
    bool beingHeld = false;
    bool swingingWeapon = false;
    public GameObject playerHand;
    private Collider collider;
    public GameObject player;
    private float swingTime;
    private float elapsedTime;
    private bool ableToHitEnemy;

    void Start() {
        collider = GetComponent<Collider>();
    }

    void Update() {
        if (swingingWeapon) {
            elapsedTime += Time.deltaTime;

            if (elapsedTime > swingTime) {
                elapsedTime = 0;
                ableToHitEnemy = true;
                
                if (!collider.enabled)
                    ToggleHitbox();

                swingingWeapon = false;
                ableToHitEnemy = false;
            }
        }
    }

    void PickUpByPlayer() {
        this.transform.parent = playerHand.transform;
        //this.transform.position = new Vector3(0.08472525f, 0.07799925f, 0.179994f);
        this.transform.position = new Vector3(playerHand.transform.position.x + 0.08472525f, playerHand.transform.position.y + 0.07799925f, playerHand.transform.position.z + 0.179994f);
        //Quaternion rotation = Quaternion.Euler(602.791f - playerHand.transform.rotation.x, 69.25199f - playerHand.transform.rotation.y, 69.32098f + playerHand.transform.rotation.z);
        Quaternion rotation = Quaternion.Euler(45, 45, 45);
        this.transform.rotation = rotation;
        player.GetComponent<Player>().ChangeCurrentWeapon(this);
    }

    void ToggleHitbox() {
        collider.enabled = !collider.enabled;
    }

    public void SwingWeapon(float animationTime) {
        swingingWeapon = true;
        swingTime = animationTime;
        ableToHitEnemy = true;
    }

    void OnTriggerEnter() {
        if (!beingHeld) {
            beingHeld = true;
            PickUpByPlayer();
        } else if (ableToHitEnemy) {
            ToggleHitbox();
            ableToHitEnemy = false;
        }
    }
}
