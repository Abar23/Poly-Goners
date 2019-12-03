using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearTrap : MonoBehaviour
{
    public enum Type { Trigger, OnTimer }

    private Animator animator;
    private GameObject spears;
    public Type type;
    public float timerOffset;
    public float cooldownTime;
    private float cooldownTimeAfterFirst;
    private float elapsedTime;
    private bool isActive;
    
    [Range(1.0f, 3.0f)]
    public float trapSpeed = 1f;

    void Start() {
        animator = GetComponent<Animator>();
        spears = transform.GetChild(0).gameObject;
        elapsedTime = 0;
        isActive = false;
        animator.speed = trapSpeed;
        cooldownTimeAfterFirst = cooldownTime;
        cooldownTime = 0;
    }

    void Update() {
        if (type == Type.OnTimer) {
            if (elapsedTime >= timerOffset + cooldownTime && !this.animator.GetCurrentAnimatorStateInfo(0).IsName("SpearTrap")) {
                animator.SetTrigger("isActive");
                isActive = true;
                elapsedTime = 0;
                cooldownTime = cooldownTimeAfterFirst;
            }

            else if (!this.animator.GetCurrentAnimatorStateInfo(0).IsName("SpearTrap")) {
                isActive = false;
            }

            if (!isActive) {
                elapsedTime += Time.deltaTime;
            }
        }
    }

    void OnTriggerEnter(Collider other) {
        if (type == Type.Trigger && !this.animator.GetCurrentAnimatorStateInfo(0).IsName("SpearTrap") && other.gameObject.tag == "Player") {
            animator.SetTrigger("isActive");
        }
    }
}
