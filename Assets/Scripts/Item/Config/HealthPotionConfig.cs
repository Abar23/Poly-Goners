using UnityEngine;

[CreateAssetMenu(fileName = "Health Potion", menuName = "ScriptableObjects/HealthPotionConfig", order = 1)]
public class HealthPotionConfig : PotionConfig
{

    public int Amount;
    public int Interval;

}