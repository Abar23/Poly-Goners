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
    }

    void LateUpdate() {
        if (type == Type.OnTimer) {
            if (elapsedTime >= timerOffset && !this.animator.GetCurrentAnimatorStateInfo(0).IsName("SpearTrap")) {
                animator.SetTrigger("isActive");
            }
        }

        if (!this.animator.GetCurrentAnimatorStateInfo(0).IsName("SpearTrap")) {
            elapsedTime += Time.deltaTime;
        } else {
            elapsedTime = 0;
        }
    }

    void OnTriggerEnter(Collider other) {
        if (type == Type.Trigger && !this.animator.GetCurrentAnimatorStateInfo(0).IsName("SpearTrap") && other.gameObject.tag == "Player") {
            animator.SetTrigger("isActive");
        }
    }
}
