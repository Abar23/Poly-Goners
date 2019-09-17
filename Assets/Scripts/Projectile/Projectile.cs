using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour
{

    public float Speed;

    public GameObject AfterEffect;

    public float SelfDestructionTime;

    [Header("Events")]
    public UnityEvent OnInvoke;

    public UnityEvent OnDestruction;

    public UnityEvent OnReset;

    public GameObject Sample;

    private bool MotionActive = true;
    private float activeTime = 0f;
    private MagicPool magicPool;

    void Update()
    {
        activeTime += Time.deltaTime;
        if (activeTime > SelfDestructionTime)
        {
            TriggerEvent(OnDestruction);
            Reset();
        }
    }

    void FixedUpdate()
    {
        if (MotionActive)
            transform.Translate(Vector3.right * Speed * Time.deltaTime);
    }

    public void ProjectileInvoke()
    {
        TriggerEvent(OnInvoke);
        gameObject.SetActive(true);
    }

    public void Freeze()
    {
        MotionActive = false;
    }

    public void ScheduleReset(float delay)
    {
        Invoke("Reset", delay);
    }

    public void Reset()
    {
        TriggerEvent(OnReset);
        transform.localPosition = Vector3.zero;
        activeTime = 0f;
        MotionActive = true;
        gameObject.SetActive(false);
        magicPool.Realse(gameObject);
    }

    public void ResetDestructionCountDown()
    {
        activeTime = 0f;
    }

    public void RegistMagicPool(MagicPool mp)
    {
        magicPool = mp;
    }

    void TriggerEvent(UnityEvent uEvent){
        if (uEvent != null)
        {
            uEvent.Invoke();
        }
    }

}
