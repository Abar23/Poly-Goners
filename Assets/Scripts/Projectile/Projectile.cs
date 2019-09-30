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
    private bool selfDestructionActive = true;

    void Update()
    {
        activeTime += Time.deltaTime;
        if (activeTime > SelfDestructionTime && selfDestructionActive)
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
        activeTime = 0f;
        MotionActive = true;
        gameObject.SetActive(false);
        magicPool.Realse(gameObject);
        selfDestructionActive = true;
    }

    public void ResetDestructionCountDown()
    {
        selfDestructionActive = false;
    }

    public void RegistMagicPool(MagicPool mp)
    {
        magicPool = mp;
    }

    public void CameraShakeEffect(float duration)
    {
        GameObject[] cameras = GameObject.FindGameObjectsWithTag("MainCamera");
        if (cameras == null || cameras.Length <= 0) return;
        foreach (GameObject camera in cameras)
        {
            CameraShake cs = camera.GetComponent<CameraShake>();
            if (cs == null) return;
            StartCoroutine(cs.CameraShakeEffect(0.1f, duration));
        }
    }

    void TriggerEvent(UnityEvent uEvent){
        if (uEvent != null)
        {
            uEvent.Invoke();
        }
    }

}
