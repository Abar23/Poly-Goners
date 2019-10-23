using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stamina : MonoBehaviour
{
    public Slider StaminaSlider;
    public int MaxStamina;

    private float currentStamina;
    private float elapsedTime = 0f;
    private float timeToRecharge = 1.75f;

    private void Start()
    {
        currentStamina = MaxStamina;
    }

    private void Update()
    {
        StaminaSlider.value = currentStamina / MaxStamina;

        if (elapsedTime < timeToRecharge)
            elapsedTime += Time.deltaTime;

        if (elapsedTime >= timeToRecharge && currentStamina < MaxStamina)
        {
            currentStamina += 0.5f;
        }

    }

    public float CurrentStaminaValue()
    {
        return currentStamina;
    }

    public void DecreaseStamina(float value)
    {
        elapsedTime = 0f;
        currentStamina -= value;
        if (currentStamina < 0)
            currentStamina = 0;
    }

    public void ResetStaminaToFull()
    {
        currentStamina = MaxStamina;
    }
}
